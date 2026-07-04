import { useEffect, useRef } from "react";
import { useAnimationStore } from "../stores/animationStore";
import { useTimerStore } from "../stores/timerStore";

// ── Shared AudioContext ──────────────────────────────────────────
// Created + resumed inside the first user gesture (Start click).
// Once unlocked, Web Audio API playback works even outside gesture chains
// (e.g. from a useEffect/worker callback minutes after the click).

let audioCtx: AudioContext | null = null;
let gainNode: GainNode | null = null;

/**
 * Call inside a user-gesture handler (Start button onClick).
 * Creates the AudioContext and resumes it, unlocking audio for the page lifetime.
 * Safe to call multiple times – no-op after first invocation.
 */
export function ensureAudioUnlocked(): void {
  if (audioCtx) return;
  audioCtx = new AudioContext();
  gainNode = audioCtx.createGain();
  gainNode.connect(audioCtx.destination);
  gainNode.gain.value = 0; // updated to actual volume on each play()
}

function playUrl(url: string, volume: number): void {
  if (!audioCtx || !gainNode) return;

  // Set gain for this playback
  gainNode.gain.value = volume / 100;

  // Fetch + decode + play via the unlocked context
  fetch(url)
    .then((res) => res.arrayBuffer())
    .then((buf) => audioCtx!.decodeAudioData(buf))
    .then((decoded) => {
      const src = audioCtx!.createBufferSource();
      src.buffer = decoded;
      src.connect(gainNode!);
      src.start(0);
    })
    .catch(() => {
      // Fetch or decode failed – silently ignore
    });
}

// ── Hook ──────────────────────────────────────────────────────────

/**
 * Plays timer notification sounds based on state changes.
 * Bell = work starting / rest starting.
 * Requires ensureAudioUnlocked() to have been called within a user gesture first.
 */
export function useTimerSounds(
  isStarted: boolean,
  isWorking: boolean,
  isShortRest: boolean,
  isLongRest: boolean
) {
  const prevWorking = useRef(isWorking);
  const prevStarted = useRef(isStarted);

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

    const manifest = useAnimationStore.getState().getSelectedManifest();
    if (!manifest) return;

    const soundSrc = isWorking
      ? `/animations/${manifest.id}/${manifest.audio.bell}`
      : `/animations/${manifest.id}/${manifest.audio.bellFinish}`;

    const volume = useTimerStore.getState().volume;
    if (volume === 0) return;

    playUrl(soundSrc, volume);
  }, [isStarted, isWorking, isShortRest, isLongRest]);
}