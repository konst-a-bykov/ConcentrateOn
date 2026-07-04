import { create } from "zustand";
import { persist } from "zustand/middleware";
import type { AnimationManifest } from "../engine/types";

interface AnimationState {
  selectedPackId: string;
  packs: Record<string, AnimationManifest>;
  setPack: (id: string) => void;
  registerPack: (manifest: AnimationManifest) => void;
  getSelectedManifest: () => AnimationManifest | undefined;
}

export const useAnimationStore = create<AnimationState>()(
  persist(
    (set, get) => ({
      selectedPackId: "ancient-man",
      packs: {},

      setPack: (id) => set({ selectedPackId: id }),

      registerPack: (manifest) =>
        set((state) => ({
          packs: { ...state.packs, [manifest.id]: manifest },
        })),

      getSelectedManifest: () => {
        const state = get();
        return state.packs[state.selectedPackId];
      },
    }),
    {
      name: "concentrateon-animation",
      partialize: (state) => ({ selectedPackId: state.selectedPackId }),
    }
  )
);
