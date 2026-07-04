import type { AnimationManifest } from "./types";
import { useAnimationStore } from "../stores/animationStore";

const ANIMATIONS_BASE = "/animations";

export async function loadAnimationPack(packId: string): Promise<AnimationManifest> {
  const response = await fetch(`${ANIMATIONS_BASE}/${packId}/manifest.json`);
  if (!response.ok) {
    throw new Error(`Failed to load animation manifest: ${packId}`);
  }
  const manifest: AnimationManifest = await response.json();
  return manifest;
}

export async function loadAllAnimationPacks(): Promise<void> {
  // For now, load the known pack. In the future, this could read a registry.
  const knownPacks = ["ancient-man"];
  const store = useAnimationStore.getState();

  for (const packId of knownPacks) {
    try {
      const manifest = await loadAnimationPack(packId);
      store.registerPack(manifest);
    } catch (err) {
      console.warn(`Failed to load animation pack "${packId}":`, err);
    }
  }
}

export function getClipSrc(
  manifest: AnimationManifest,
  clipName: keyof AnimationManifest["clips"],
  orientation: "landscape" | "portrait"
): string {
  const clip = manifest.clips[clipName];
  return `/animations/${manifest.id}/${clip[orientation]}`;
}
