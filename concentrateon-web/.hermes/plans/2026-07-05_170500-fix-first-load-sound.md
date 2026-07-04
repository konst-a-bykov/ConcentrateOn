# Plan: Fix missing sound on first download — sound appears only after moving volume slider

## Goal

Investigate and fix the bug where notification sounds (bell/bellFinish) don't play on the first download/visit, but start working after the user moves the volume slider in Settings.

---

## Current context

### How sounds work today (`src/engine/useTimerSounds.ts`)

- Hook called from `Home.tsx` with `isStarted`, `isWorking`, `isShortRest`, `isLongRest`
- On work→rest or rest→work transitions, it creates a **new `Audio()` element** and calls `.play()`
- Volume is read from `useTimerStore.getState().volume` at play time (default: 50)
- An `AudioContext` is created but **never used** — it's dead code

### Volume state persistence (`src/stores/timerStore.ts`)

- Zustand with `persist` middleware, stored under `concentrateon-timer` key in localStorage
- `partialize` includes `volume`
- `merge` does `{ ...current, ...persisted }` — defaults apply when localStorage is empty (fresh first visit)
- Default `volume: 50`
- Volume can also be set via `updateSettings({ volume: N })`

### What the volume slider does

- In `Settings.tsx`, an `<input type="range">` calls `updateSettings({ volume: parseInt(e.target.value) })`
- This writes to the store and triggers persist to localStorage
- Does **not** create any Audio, AudioContext, or trigger any playback

### Audio playback mechanism

- Uses raw `new Audio(soundSrc)` — no AudioContext, no Web Audio API routing
- Sound source: `/animations/${manifest.id}/${manifest.audio.bell}` or `...bellFinish`
- Audio paths are served as static assets from `public/` (or possibly via animation pack loader)

---

## Hypotheses (ordered by likelihood)

### Hypothesis 1 [most likely]: Browser autoplay policy blocks audio outside user gesture
The timer runs in a Web Worker (`timer.worker.ts`) and fires via `setInterval`. When a period transitions, the sound plays from a React `useEffect`, **not** from a user-gesture event handler chain. Modern browsers (Chrome, Safari, especially PWA on iOS/Android) require audio playback to be initiated within 1–2 seconds of a user gesture (`click`, `touchstart`). Because the sound fires 25+ minutes after the Start click, the gesture chain is long gone, and the browser silently blocks the `audio.play()` promise (caught by `.catch()`).

**Why moving the slider fixes it**: Touching the volume slider creates a fresh user gesture. This resets the autoplay timer in the browser, putting it in a "temporarily allowed" state. If the timer transitions soon after (within the grace period), the gesture chain is still alive, so audio plays.

**Evidence check**: The `.catch()` on line 50–52 silently swallows the rejection with a comment "Autoplay blocked". This exactly matches the symptom.

### Hypothesis 2 [less likely]: Race with async animation pack loading
`loadAllAnimationPacks()` in `App.tsx` fires asynchronously on mount. If the timer transitions before packs finish loading, `getSelectedManifest()` returns `undefined` and sound is skipped.

**Why moving the slider might help**: The user navigated to Settings, moved the slider (takes some seconds), then navigated back. By then, packs have loaded. But this is weak — packs load within milliseconds from cache on second visit, so it doesn't explain why first-visit sound permanently fails even on subsequent transitions.

### Hypothesis 3 [less likely]: Volume state desync on first rehydration
Zustand persist rehydrates asynchronously. On first visit the localStorage key doesn't exist, so merge should use defaults correctly. But if there's a `null` or `undefined` value that survives the merge, `volume / 100` becomes `NaN`, which sets `audio.volume = NaN` and may cause silent failure.

---

## Proposed approach

### Step 1: Verify browser autoplay diagnosis

- Add a `console.warn` inside the `.catch()` handler to confirm autoplay blocking
- Add a `console.log` for `audioCtxRef.current?.state` to check AudioContext state on each transition
- Test on fresh incognito/PWA install

**Files changed**: `src/engine/useTimerSounds.ts` (add logging only)

### Step 2 (if Hypothesis 1 confirmed): Fix autoplay by creating a single shared `AudioContext` and unlocking it on first user click

- Remove the current dead `AudioContext` that's created but never used
- Create a **persistent, module-level shared AudioContext** that gets created and unlocked on the very first user interaction (Start button click)
- Instead of `new Audio()`, decode and play via Web Audio API through this unlocked context

**Files changed**:
- `src/engine/useTimerSounds.ts` — rewrite to use a shared AudioContext
- Possibly: `src/components/ControlButtons.tsx` — fire a user-gesture event that unlocks the AudioContext

### Step 3 (alternative lighter fix): Gate audio creation in the click handler

If the full AudioContext approach is over-engineering, a lighter fix:
- When the user clicks Start, pre-create and preload the Audio elements within the gesture handler
- Play them later from a ref (the `useEffect` checks the ref, the ref was created within a gesture)

This exploits the rule that creating `Audio` elements within a gesture gives them "autoplay permission" for their lifetime on some browsers.

### Step 4: Edge case — missing animation pack fallback

Add a graceful fallback if `manifest` or `manifest.audio` is missing — use a hardcoded path to bundled audio files.

### Step 5: Clean up dead code

Remove the unused `AudioContext` creation from `useTimerSounds.ts`.

---

## Files likely to change

| File | Change |
|---|---|
| `src/engine/useTimerSounds.ts` | Main fix — rewrite audio playback logic |
| `src/pages/Home.tsx` | Possibly pass `isStarted` click handler to unlock AudioContext synchronously |
| `src/components/ControlButtons.tsx` | Possibly fire AudioContext resume on Start click |

---

## Verification

1. Open app in incognito/private window (fresh no localStorage)
2. Click Start → wait for work period to end → sound should play
3. Repeat for rest→work transition
4. Verify volume slider still works correctly
5. Test on desktop Chrome, mobile Safari/PWA
6. Run `npm run build && npm run preview` to test production build

---

## Risks / tradeoffs

- **AudioContext approach** adds complexity but is the correct fix for autoplay policy
- **Pre-load within gesture** is simpler but may not work on all browsers (especially iOS Safari which requires audio to play *during* the gesture, not just be created)
- Any fix must not break the existing "muted after Start" video behavior in `AnimationPlayer.tsx`

---

## Open questions

1. Does the sound fail 100% on first visit or only on first *transition* after Start? (If the first transition is 25 min later, autoplay is almost certainly blocked.)
2. Is the issue reproducible on desktop or only on mobile/PWA?
3. Are the audio files (`.mp3`/`.wav`/`.ogg`) bundled with the app or fetched at runtime?