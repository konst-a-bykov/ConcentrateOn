import { useEffect, useRef } from "react";
import { useAnimationStore } from "../stores/animationStore";
import { useTimerStore } from "../stores/timerStore";

/**
 * Plays timer notification sounds based on state changes.
 * Bell = work starting, Bell finish = rest starting.
 * Requires prior user interaction (browser autoplay policy).
 */
export function useTimerSounds(
  isStarted: boolean,
  isWorking: boolean,
  isShortRest: boolean,
  isLongRest: boolean
) {
  const prevWorking = useRef(isWorking);
  const prevStarted = useRef(isStarted);
  const audioCtxRef = useRef<AudioContext | null>(null);

  useEffect(() => {
    // Only play on actual state transitions, not initial mount
    if (!isStarted || !prevStarted.current) {
      prevWorking.current = isWorking;
      prevStarted.current = isStarted;
      return;
    }

    const workChanged = prevWorking.current !== isWorking;
    prevWorking.current = isWorking;
    prevStarted.current = isStarted;

    if (!workChanged) return;

    // Lazily create AudioContext on first transition (after user has clicked Start)
    if (!audioCtxRef.current) {
      audioCtxRef.current = new AudioContext();
    }

    const manifest = useAnimationStore.getState().getSelectedManifest();
    if (!manifest) return;

    const soundSrc = isWorking
      ? `/animations/${manifest.id}/${manifest.audio.bell}`
      : `/animations/${manifest.id}/${manifest.audio.bellFinish}`;

    const volume = useTimerStore.getState().volume;
    if (volume === 0) return;
    const audio = new Audio(soundSrc);
    audio.volume = volume / 100;
    audio.play().catch(() => {
      // Autoplay blocked — user hasn't interacted yet
    });
  }, [isStarted, isWorking, isShortRest, isLongRest]);
}
