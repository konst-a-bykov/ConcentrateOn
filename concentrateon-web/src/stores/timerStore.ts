import { create } from "zustand";
import { persist } from "zustand/middleware";

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
  updateSettings: (settings: Partial<TimerSettingsPayload>) => void;
  restoreFromStartTime: (startTimeMs: number) => void;
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
    // Work period ended → start rest
    const newBreakCounter = state.breakCounter + 1;
    if (newBreakCounter >= state.intervalForLongRest) {
      // Long rest
      return {
        isWorking: false,
        isShortRest: false,
        isLongRest: true,
        breakCounter: 0,
        secondsLeft: state.longRestMinutes * 60,
      };
    } else {
      // Short rest
      return {
        isWorking: false,
        isShortRest: true,
        isLongRest: false,
        breakCounter: newBreakCounter,
        secondsLeft: state.shortRestMinutes * 60,
      };
    }
  } else if (state.isStarted && (state.isShortRest || state.isLongRest)) {
    // Rest period ended → start work
    return {
      isWorking: true,
      isShortRest: false,
      isLongRest: false,
      secondsLeft: state.workTimeMinutes * 60,
    };
  }
  return {};
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
        }),

      pause: () => set({ isPaused: true }),

      resume: () => set({ isPaused: false }),

      stop: () =>
        set({
          isPaused: false,
          isStarted: false,
          isWorking: true,
          isShortRest: false,
          isLongRest: false,
          breakCounter: 0,
          secondsLeft: get().workTimeMinutes * 60,
        }),

      tick: () => {
        const state = get();
        if (!state.isStarted || state.isPaused) return;

        if (state.secondsLeft <= 0) {
          set(changeActivityPeriod(state));
          return;
        }

        set({ secondsLeft: state.secondsLeft - 1 });
      },

      updateSettings: (settings) => {
        const current = get();
        const newState: Partial<TimerState> = {};
        if (settings.workTimeMinutes !== undefined) {
          newState.workTimeMinutes = settings.workTimeMinutes;
        }
        if (settings.shortRestMinutes !== undefined)
          newState.shortRestMinutes = settings.shortRestMinutes;
        if (settings.longRestMinutes !== undefined)
          newState.longRestMinutes = settings.longRestMinutes;
        if (settings.intervalForLongRest !== undefined)
          newState.intervalForLongRest = settings.intervalForLongRest;
        if (settings.volume !== undefined)
          newState.volume = settings.volume;
        set(newState as TimerState);

        // Always sync secondsLeft with new settings when timer is not actively counting
        const updated = get();
        if (!updated.isPaused && !updated.isStarted) {
          set({ secondsLeft: updated.workTimeMinutes * 60 });
        }
      },

      restoreFromStartTime: (startTimeMs: number) => {
        const now = Date.now();
        const elapsedSeconds = Math.floor((now - startTimeMs) / 1000);
        const state = get();

        // Reset to initial state
        let tempState: TimerState = {
          ...state,
          breakCounter: 0,
          secondsLeft: state.workTimeMinutes * 60,
          isPaused: false,
          isStarted: true,
          isWorking: true,
          isShortRest: false,
          isLongRest: false,
        };

        // Replay ticks to restore position
        for (let i = 0; i < elapsedSeconds; i++) {
          if (tempState.secondsLeft <= 0) {
            tempState = { ...tempState, ...changeActivityPeriod(tempState) };
          }
          tempState.secondsLeft--;
        }

        set({
          ...tempState,
          isStarted: true,
          isPaused: false,
        });
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
        isWorking: state.isWorking,
        isShortRest: state.isShortRest,
        isLongRest: state.isLongRest,
        isPaused: state.isPaused,
        secondsLeft: state.secondsLeft,
        breakCounter: state.breakCounter,
        volume: state.volume,
      }),
      merge: (persisted, current) => {
        const merged = { ...current, ...(persisted as Partial<TimerState>) };
        // On reload, timer isn't actively running — always sync secondsLeft with settings
        merged.secondsLeft = merged.workTimeMinutes * 60;
        merged.isStarted = false;
        merged.isPaused = false;
        return merged;
      },
    }
  )
);
