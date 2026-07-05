import { create } from "zustand";
import { persist } from "zustand/middleware";
import { useStatisticsStore } from "./statisticsStore";

export type ActivityType = "WorkingTime" | "ShortRest" | "LongRest";

export interface TimerState {
  // Timer values
  secondsLeft: number;
  isWorking: boolean;
  isShortRest: boolean;
  isLongRest: boolean;
  breakCounter: number;
  isPaused: boolean;
  isStarted: boolean;
  startTimeMs: number | null;

  // Settings
  workTimeMinutes: number;
  shortRestMinutes: number;
  longRestMinutes: number;
  intervalForLongRest: number;
  volume: number;

  // Actions
  start: () => void;
  pause: () => void;
  resume: () => void;
  stop: () => void;
  tick: () => void;
  catchUp: () => void;
  updateSettings: (settings: Partial<TimerSettingsPayload>) => void;
}

export interface TimerSettingsPayload {
  workTimeMinutes: number;
  shortRestMinutes: number;
  longRestMinutes: number;
  intervalForLongRest: number;
  volume: number;
}

function changeActivityPeriod(state: TimerState): Partial<TimerState> {
  if (state.isWorking && state.isStarted) {
    const newBreakCounter = state.breakCounter + 1;
    if (newBreakCounter >= state.intervalForLongRest) {
      return {
        isWorking: false,
        isShortRest: false,
        isLongRest: true,
        breakCounter: 0,
        secondsLeft: state.longRestMinutes * 60,
      };
    } else {
      return {
        isWorking: false,
        isShortRest: true,
        isLongRest: false,
        breakCounter: newBreakCounter,
        secondsLeft: state.shortRestMinutes * 60,
      };
    }
  } else if (state.isStarted && (state.isShortRest || state.isLongRest)) {
    return {
      isWorking: true,
      isShortRest: false,
      isLongRest: false,
      secondsLeft: state.workTimeMinutes * 60,
    };
  }
  return {};
}

/** Replay ticks from startTime to reconstruct current timer position */
function restoreFromStartTime(
  startTimeMs: number,
  workTimeMinutes: number,
  shortRestMinutes: number,
  longRestMinutes: number,
  intervalForLongRest: number,
  onPeriodComplete?: (activityType: ActivityType, durationSeconds: number) => void
): Partial<TimerState> {
  const elapsedSeconds = Math.floor((Date.now() - startTimeMs) / 1000);

  let temp: TimerState = {
    secondsLeft: workTimeMinutes * 60,
    isWorking: true,
    isShortRest: false,
    isLongRest: false,
    breakCounter: 0,
    isPaused: false,
    isStarted: true,
    startTimeMs,
    workTimeMinutes,
    shortRestMinutes,
    longRestMinutes,
    intervalForLongRest,
    volume: 50,
    start: () => ({}),
    pause: () => ({}),
    resume: () => ({}),
    stop: () => ({}),
    tick: () => ({}),
    updateSettings: () => ({}),
    catchUp: () => ({}),
  };

  for (let i = 0; i < elapsedSeconds; i++) {
    if (temp.secondsLeft <= 0) {
      if (onPeriodComplete) {
        const activityType: ActivityType = temp.isWorking
          ? "WorkingTime"
          : temp.isShortRest
            ? "ShortRest"
            : "LongRest";
        const pd = temp.isWorking
          ? workTimeMinutes * 60
          : temp.isShortRest
            ? shortRestMinutes * 60
            : longRestMinutes * 60;
        onPeriodComplete(activityType, pd);
      }
      temp = { ...temp, ...changeActivityPeriod(temp) };
    }
    temp.secondsLeft--;
  }

  return {
    secondsLeft: temp.secondsLeft,
    isWorking: temp.isWorking,
    isShortRest: temp.isShortRest,
    isLongRest: temp.isLongRest,
    breakCounter: temp.breakCounter,
    isStarted: true,
    isPaused: false,
    startTimeMs,
  };
}

export const useTimerStore = create<TimerState>()(
  persist(
    (set, get) => ({
      // Initial state
      secondsLeft: 25 * 60,
      isWorking: true,
      isShortRest: false,
      isLongRest: false,
      breakCounter: 0,
      isPaused: false,
      isStarted: false,
      startTimeMs: null,

      // Settings
      workTimeMinutes: 25,
      shortRestMinutes: 5,
      longRestMinutes: 15,
      intervalForLongRest: 4,
      volume: 50,

      start: () =>
        set({
          breakCounter: 0,
          secondsLeft: get().workTimeMinutes * 60,
          isPaused: false,
          isStarted: true,
          isWorking: true,
          isShortRest: false,
          isLongRest: false,
          startTimeMs: Date.now(),
        }),

      pause: () => set({ isPaused: true }),

      resume: () => set({ isPaused: false, startTimeMs: Date.now() - (get().workTimeMinutes * 60 - get().secondsLeft) * 1000 }),

      stop: () => {
        const state = get();
        // Log partial progress if timer was running
        if (state.isStarted) {
          const activityType: ActivityType = state.isWorking
            ? "WorkingTime"
            : state.isShortRest
              ? "ShortRest"
              : "LongRest";
          const periodDuration = state.isWorking
            ? state.workTimeMinutes * 60
            : state.isShortRest
              ? state.shortRestMinutes * 60
              : state.longRestMinutes * 60;
          const elapsed = periodDuration - state.secondsLeft;
          if (elapsed > 0) {
            useStatisticsStore.getState().addEntry({
              startTime: Date.now() - elapsed * 1000,
              durationSeconds: elapsed,
              activityType,
            });
          }
        }
        set({
          isPaused: false,
          isStarted: false,
          isWorking: true,
          isShortRest: false,
          isLongRest: false,
          breakCounter: 0,
          secondsLeft: state.workTimeMinutes * 60,
          startTimeMs: null,
        });
      },

      tick: () => {
        const state = get();
        if (!state.isStarted || state.isPaused) return;

        if (state.secondsLeft <= 0) {
          // Log the completed period
          const activityType: ActivityType = state.isWorking
            ? "WorkingTime"
            : state.isShortRest
              ? "ShortRest"
              : "LongRest";
          const periodDuration = state.isWorking
            ? state.workTimeMinutes * 60
            : state.isShortRest
              ? state.shortRestMinutes * 60
              : state.longRestMinutes * 60;
          useStatisticsStore.getState().addEntry({
            startTime: Date.now() - periodDuration * 1000,
            durationSeconds: periodDuration,
            activityType,
          });

          set(changeActivityPeriod(state));
          return;
        }

        set({ secondsLeft: state.secondsLeft - 1 });
      },

      catchUp: () => {
        const state = get();
        if (!state.isStarted || state.isPaused || !state.startTimeMs) return;

        const restored = restoreFromStartTime(
          state.startTimeMs,
          state.workTimeMinutes,
          state.shortRestMinutes,
          state.longRestMinutes,
          state.intervalForLongRest,
          (activityType, durationSeconds) => {
            useStatisticsStore.getState().addEntry({
              startTime: Date.now() - durationSeconds * 1000,
              durationSeconds,
              activityType,
            });
          }
        );

        set({
          secondsLeft: restored.secondsLeft,
          isWorking: restored.isWorking,
          isShortRest: restored.isShortRest,
          isLongRest: restored.isLongRest,
          breakCounter: restored.breakCounter,
          isPaused: false,
          startTimeMs: restored.startTimeMs,
        });
      },

      updateSettings: (settings) => {
        const newState: Partial<TimerState> = {};
        if (settings.workTimeMinutes !== undefined)
          newState.workTimeMinutes = settings.workTimeMinutes;
        if (settings.shortRestMinutes !== undefined)
          newState.shortRestMinutes = settings.shortRestMinutes;
        if (settings.longRestMinutes !== undefined)
          newState.longRestMinutes = settings.longRestMinutes;
        if (settings.intervalForLongRest !== undefined)
          newState.intervalForLongRest = settings.intervalForLongRest;
        if (settings.volume !== undefined)
          newState.volume = settings.volume;
        set(newState as TimerState);

        const updated = get();
        if (!updated.isPaused && !updated.isStarted) {
          set({ secondsLeft: updated.workTimeMinutes * 60 });
        }
      },
    }),
    {
      name: "concentrateon-timer",
      partialize: (state) => ({
        workTimeMinutes: state.workTimeMinutes,
        shortRestMinutes: state.shortRestMinutes,
        longRestMinutes: state.longRestMinutes,
        intervalForLongRest: state.intervalForLongRest,
        isStarted: state.isStarted,
        isPaused: state.isPaused,
        startTimeMs: state.startTimeMs,
        volume: state.volume,
      }),
      merge: (persisted, current) => {
        const p = persisted as Partial<TimerState>;
        const merged = { ...current, ...p };

        if (p.isStarted && p.startTimeMs) {
          // Timer was running — restore position from start time
          const restored = restoreFromStartTime(
            p.startTimeMs,
            merged.workTimeMinutes,
            merged.shortRestMinutes,
            merged.longRestMinutes,
            merged.intervalForLongRest
          );
          Object.assign(merged, restored);
        } else if (p.isPaused && p.startTimeMs) {
          // Timer was paused — restore state but keep paused
          const restored = restoreFromStartTime(
            p.startTimeMs,
            merged.workTimeMinutes,
            merged.shortRestMinutes,
            merged.longRestMinutes,
            merged.intervalForLongRest
          );
          Object.assign(merged, restored);
          merged.isPaused = true;
        } else {
          // Timer was stopped — show duration from settings
          merged.secondsLeft = merged.workTimeMinutes * 60;
          merged.isStarted = false;
          merged.isPaused = false;
          merged.startTimeMs = null;
        }

        return merged;
      },
    }
  )
);
