# Plan: Fix Timer Freeze on Mobile Background (Visibility Change)

## Goal

When the user backgrounds the PWA on mobile (switches to another app) and returns minutes later, the timer should fast-forward to account for elapsed time instead of resuming from the stale `secondsLeft`. Page reload already works correctly via `restoreFromStartTime` — the gap is that without a reload the Web Worker's `setInterval` is suspended by the browser and doesn't catch up on resume.

## Root Cause

- `timer.worker.ts` uses `setInterval(…, 1000)` to post ticks.
- On mobile, browsers **suspend** Web Worker timers when the tab/page is backgrounded.
- When the user returns, the worker sends only one tick (1s worth), ignoring the real elapsed time.
- Zustand's `startTimeMs` tracks the correct epoch offset, but nothing reads it during the active session except on page reload (via `merge` → `restoreFromStartTime`).

**Why reload fixes it**: the persistence layer's `merge` calls `restoreFromStartTime(startTimeMs)` which replays all elapsed ticks from `startTimeMs` to now, reconstructing the correct `secondsLeft` and activity phase.

## Approach

Add a `catchUp()` action to the timer store that uses the same `restoreFromStartTime` logic to fast-forward the timer. Call it from a `visibilitychange` event listener in `Home.tsx` when the page becomes visible again and the timer is running.

### Step-by-step plan

**Step 1 — Add `catchUp()` action to timerStore**

Add a new store action `catchUp` that:
1. Guards: only runs if `isStarted && !isPaused`
2. Calls `restoreFromStartTime` with the current `startTimeMs` and settings
3. Applies the restored state to the store
4. Also logs any completed periods to `statisticsStore` that were missed during the gap (since `restoreFromStartTime` replays period transitions but doesn't call `addEntry`)

**File:** `src/stores/timerStore.ts`

Changes:
- Add `catchUp: () => void` to the `TimerState` interface
- Implement `catchUp` in the store body, calling `restoreFromStartTime` with current state values
- Wrap period logging during the catch-up (same stats-logic as in `tick()` for period completions)

**Step 2 — Add visibilitychange listener in Home.tsx**

Add a `useEffect` that:
1. Listens to `document.visibilitychange`
2. On `document.visibilityState === "visible"`, calls `catchUp()` from the store
3. Cleans up the listener on unmount

**File:** `src/pages/Home.tsx`

Changes:
- Import `catchUp` (or the full store getter) from `useTimerStore`
- Add a `useEffect` with the visibility listener

**Step 3 — Verify**

- Test on desktop by switching to another tab for 10s+, returning to see the timer advanced
- Test on mobile (or device-emulation in Chrome DevTools) by:
  1. Start timer
  2. Switch to another app / tab for 30s+
  3. Return — timer should show correct remaining time
- Confirm no regression: pause/resume, stop, period transitions still work
- Confirm the visibility listener doesn't cause extra re-renders

### Files changed

| File | Change |
|------|--------|
| `src/stores/timerStore.ts` | Add `catchUp()` action + interface entry |
| `src/pages/Home.tsx` | Add `visibilitychange` useEffect calling `catchUp` |

### Risks & tradeoffs

- **Double-tick on first visible moment**: The worker might also send a delayed tick at the same moment. That's harmless — `catchUp` sets the correct time, and the extra tick will decrement by 1, which is negligible and corrects on the next tick.
- **Page hidden briefly**: `visibilitychange` fires even for sub-second hides (e.g. notification shade pull-down). Using `restoreFromStartTime` is cheap (plain arithmetic, no allocations) so this is fine.
- **Using `restoreFromStartTime` directly vs. replaying `tick()` N times**: The replay function is already used on reload and is O(1) per iteration. For a 5-minute gap it's 300 iterations of a loop — negligible.

### Open questions

1. Should `catchUp` be called immediately on becoming visible, or after a small debounce? Immediate seems fine since the state update is synchronous.
2. Should statistics logging for completed periods during the gap happen inside `catchUp` or be left to the first `tick` after catch-up? Better to do it inside `catchUp` so the stats are accurate even if the user navigates away again immediately.