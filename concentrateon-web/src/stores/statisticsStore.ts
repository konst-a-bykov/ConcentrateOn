import { create } from "zustand";
import { persist } from "zustand/middleware";
import type { ActivityType } from "./timerStore";

export interface StatisticsEntry {
  startTime: number; // unix ms
  durationSeconds: number;
  activityType: ActivityType;
}

interface StatisticsState {
  entries: StatisticsEntry[];
  addEntry: (entry: StatisticsEntry) => void;
  clearEntries: () => void;
}

export const useStatisticsStore = create<StatisticsState>()(
  persist(
    (set) => ({
      entries: [],

      addEntry: (entry) =>
        set((state) => ({ entries: [...state.entries, entry] })),

      clearEntries: () => set({ entries: [] }),
    }),
    { name: "concentrateon-statistics" }
  )
);
