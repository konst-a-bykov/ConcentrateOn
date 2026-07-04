# ConcentrateOn Web — Finalized Plan

## Summary

Port ConcentrateOn to a web app. Pure SPA (no backend), React + Vite + DaisyUI + Tailwind. Core differentiator: plugin-based animation system with video state machine.

## Confirmed Decisions

| Decision | Choice |
|----------|--------|
| Backend | None for v1 (pure SPA, localStorage) |
| Video format | WebM (VP9), 30-40% smaller than MP4 |
| Audio format | MP3 (transcode from WAV) |
| Animation assets | Bundled in `public/` |
| Animation picker | Settings dropdown (gallery later) |
| Themes | DaisyUI light/dark toggle, persisted to localStorage |
| PWA | Yes — offline, installable, Web Worker for timer |
| File naming | Lowercase kebab-case, English |

## Tech Stack

- Vite + React 19 + TypeScript
- Tailwind CSS v4 + DaisyUI v5
- Zustand (state management + localStorage persistence)
- React Router v7
- react-i18next (5 languages: en, de, fr, he, ru)
- vite-plugin-pwa (service worker + manifest)

## Animation Architecture

Each animation is a folder with a manifest + video assets. Adding a new animation = drop a folder, no code changes.

### File naming convention

```
animations/<pack-id>/
├── manifest.json
├── resting.mp4                  # idle state, landscape, loops
├── resting-portrait.mp4         # idle state, portrait, loops
├── start-working.mp4            # transition: rest → work, landscape, plays once
├── start-working-portrait.mp4   # transition: rest → work, portrait, plays once
├── working.mp4                  # work state, landscape, loops
├── working-portrait.mp4         # work state, portrait, loops
├── stop-working.mp4             # transition: work → rest, landscape, plays once
├── stop-working-portrait.mp4    # transition: work → rest, portrait, plays once
├── thumbnail-rest.png           # preview image (rest state)
├── thumbnail-work.png           # preview image (work state)
├── bell.mp3                     # transition sound
└── bell-finish.mp3              # completion sound
```

### Manifest format

```json
{
  "id": "ancient-man",
  "name": "Ancient Man",
  "orientations": {
    "landscape": { "width": 1280, "height": 720 },
    "portrait":  { "width": 720, "height": 1280 }
  },
  "clips": {
    "resting":       { "landscape": "resting.mp4",              "portrait": "resting-portrait.mp4",              "loop": true },
    "startWorking":  { "landscape": "start-working.mp4",        "portrait": "start-working-portrait.mp4",        "loop": false },
    "working":       { "landscape": "working.mp4",              "portrait": "working-portrait.mp4",              "loop": true },
    "stopWorking":   { "landscape": "stop-working.mp4",         "portrait": "stop-working-portrait.mp4",         "loop": false }
  },
  "thumbnails": {
    "rest": "thumbnail-rest.png",
    "work": "thumbnail-work.png"
  },
  "audio": {
    "bell": "bell.mp3",
    "bellFinish": "bell-finish.mp3"
  }
}
```

### State machine

```
[Resting] ←→ [StartWorking] → [Working] ←→ [StopWorking]
  loop           once           loop          once
```

- Timer enters "working" → play `startWorking` clip → on end, switch to `working` loop
- Timer enters "resting" → play `stopWorking` clip → on end, switch to `resting` loop
- Orientation selected by viewport aspect ratio (`matchMedia`)

## Project Structure

```
concentrateon-web/
├── public/
│   └── animations/
│       └── ancient-man/          # WebM videos + manifest + assets
├── src/
│   ├── main.tsx                  # entry point, register service worker
│   ├── App.tsx                   # router + theme provider
│   ├── stores/
│   │   ├── timerStore.ts         # Zustand: timer state machine
│   │   ├── settingsStore.ts      # Zustand: settings (persisted to localStorage)
│   │   ├── statisticsStore.ts    # Zustand: activity log (persisted)
│   │   └── themeStore.ts         # Zustand: light/dark (persisted)
│   ├── engine/
│   │   ├── timer.worker.ts       # Web Worker: 1-second tick (unaffected by tab throttling)
│   │   ├── TimerEngine.ts        # orchestrator: worker ↔ store ↔ UI
│   │   ├── AnimationPlayer.tsx   # <video> state machine component
│   │   └── animationLoader.ts    # fetch manifests, register packs, select orientation
│   ├── pages/
│   │   ├── Home.tsx              # animation player (background) + timer overlay + controls
│   │   ├── Settings.tsx          # durations, notifications, animation picker, theme toggle
│   │   ├── Statistics.tsx        # activity log table, daily/weekly summaries
│   │   └── About.tsx             # credits, Pomodoro info
│   ├── components/
│   │   ├── TimerDisplay.tsx      # countdown mm:ss + status text
│   │   ├── ControlButtons.tsx    # start / pause / continue / stop
│   │   ├── ThemeToggle.tsx       # sun/moon icon button
│   │   ├── AnimationPicker.tsx   # dropdown of registered animation packs
│   │   └── Layout.tsx            # nav bar + DaisyUI drawer + page content
│   └── i18n/
│       ├── index.ts              # i18next config
│       └── locales/
│           ├── en.json
│           ├── de.json
│           ├── fr.json
│           ├── he.json
│           └── ru.json
├── index.html
├── package.json
├── vite.config.ts                # PWA plugin, worker support
├── tailwind.config.ts
└── tsconfig.json
```

## Implementation Phases

### Phase 1 — Scaffold & Timer Core

1. `npm create vite@latest concentrateon-web -- --template react-ts`
2. Install: `tailwindcss @tailwindcss/vite daisyui zustand react-router-dom react-i18next i18next vite-plugin-pwa`
3. Configure: Tailwind + DaisyUI plugin, Vite PWA manifest, worker support
4. Port `TimerEngine` from `Core.cs`:
   - Work/rest cycle: 25min work → 5min short rest (×3) → 15min long rest → repeat
   - Web Worker runs `setInterval(1000)` — posts `{ secondsLeft, state }` to main thread
   - State: `{ secondsLeft, isWorking, isShortRest, isLongRest, breakCounter, isPaused, isStarted }`
5. Port settings from `ConcentrateOnSettings.cs`:
   - Defaults: workTime=25, shortRest=5, longRest=15, intervalForLongRest=4
   - Zustand `persist` middleware → localStorage
6. Port statistics from `StatisticsLog.cs`:
   - Log entries: `{ startTime, duration, activityType }` → localStorage
7. Theme store: Zustand + persist, reads `prefers-color-scheme` on first load
8. DaisyUI theme: `data-theme` on `<html>`, toggle via ThemeStore

### Phase 2 — Animation Player

9. Build `AnimationPlayer` component:
   - Two `<video>` elements (landscape + portrait), show one based on viewport
   - `muted` + `playsInline` (required for autoplay)
   - State machine: track current clip, `onended` handler for transitions
   - Mute/unmute sync with bell sounds
10. Build `animationLoader`:
    - Fetch `manifest.json` from `/animations/<id>/`
    - Register available packs
    - Orientation via `matchMedia('(orientation: portrait)')` + resize listener
11. Wire timer state → animation state:
    - `isWorking` changes → trigger `startWorking` / `stopWorking` clip
    - Pause → pause video; Resume → resume video

### Phase 3 — UI Pages

12. **Layout**: DaisyUI navbar (app name + ThemeToggle) + drawer for mobile nav
13. **Home**: AnimationPlayer as full-screen background, TimerDisplay + ControlButtons as floating overlay (DaisyUI card with `bg-base-100/80` backdrop)
14. **Settings**: DaisyUI form — number inputs for durations, toggle for notifications, AnimationPicker dropdown, theme toggle
15. **Statistics**: DaisyUI table of activity log, filter by date range
16. **About**: Static content, credits, link to original UWP app

### Phase 4 — Polish & PWA

17. **PWA**: `vite-plugin-pwa` config — precache app shell + animation assets
18. **Web Worker timer**: Move `setInterval` to worker, post messages to main thread
19. **Notifications**: `Notification` API + `bell.mp3` / `bell-finish.mp3` via Web Audio
20. **Localization**: Port strings from `Strings/*/Resources.resw` to JSON, 5 languages
21. **Responsive**: Animation adapts to portrait/landscape, controls reflow for mobile
22. **State restore on reload**: Read `startTime` from localStorage, replay ticks (same as `Core.RestoreState()`)

## Encoding Pipeline

Convert existing assets to web-optimized formats:

```bash
# Video: MP4 → WebM (VP9)
ffmpeg -i Resting.mp4 -c:v libvpx-vp9 -crf 35 -b:v 0 -an resting.webm

# Audio: WAV → MP3
ffmpeg -i bell.wav -codec:a libmp3lame -qscale:a 2 bell.mp3

# Update manifest to use .webm extensions
```

Use `.webm` extension (not `.mp4`) in manifests. Keep original MP4s as source in a `_source/` folder outside `public/`.

## Risks & Mitigations

| Risk | Mitigation |
|------|-----------|
| Browser blocks autoplay | Videos are `muted` + `playsInline` — matches UWP behavior |
| Timer drift in background tab | Web Worker runs independently, unaffected by throttling |
| Large video files (~18MB WebM) | Acceptable for single pack. Lazy-load future packs from CDN |
| Safari WebM quirks | VP9 in Safari 15+ is solid. Fallback to MP4 only if needed |
| Orientation flicker on resize | Debounce orientation detection (300ms) |

## Files Changed

All new — `concentrateon-web/` directory, sibling to existing `ConcentrateOn/` and `Encryptor/`. No changes to existing UWP code. Animation source files copied from `ConcentrateOn/Animations/AncientMan/` and re-encoded.
