import { useEffect, useRef, useState, useCallback } from "react";
import type { AnimationManifest, ClipState, Orientation } from "./types";
import { getClipSrc } from "./animationLoader";

interface AnimationPlayerProps {
  manifest: AnimationManifest | undefined;
  isWorking: boolean;
  isStarted: boolean;
  isPaused: boolean;
}

function getOrientation(): Orientation {
  return window.matchMedia("(orientation: portrait)").matches
    ? "portrait"
    : "landscape";
}

export function AnimationPlayer({
  manifest,
  isWorking,
  isStarted,
  isPaused,
}: AnimationPlayerProps) {
  const videoRef = useRef<HTMLVideoElement>(null);
  const [orientation, setOrientation] = useState<Orientation>(getOrientation);
  const [currentClip, setCurrentClip] = useState<ClipState>("resting");
  const pendingTransition = useRef<ClipState | null>(null);

  // Track orientation changes
  useEffect(() => {
    const mql = window.matchMedia("(orientation: portrait)");
    let debounce: ReturnType<typeof setTimeout>;
    const handler = () => {
      clearTimeout(debounce);
      debounce = setTimeout(() => {
        setOrientation(mql.matches ? "portrait" : "landscape");
      }, 300);
    };
    mql.addEventListener("change", handler);
    return () => {
      mql.removeEventListener("change", handler);
      clearTimeout(debounce);
    };
  }, []);

  // Load and play a clip
  const playClip = useCallback(
    (clipName: ClipState) => {
      if (!manifest || !videoRef.current) return;
      const src = getClipSrc(manifest, clipName, orientation);
      const clip = manifest.clips[clipName];
      const video = videoRef.current;

      video.src = src;
      video.loop = clip.loop;
      video.load();
      video.play().catch(() => {
        // Autoplay may be blocked; user interaction will trigger play
      });
      setCurrentClip(clipName);
    },
    [manifest, orientation]
  );

  // Handle timer state changes → animation transitions
  useEffect(() => {
    if (!isStarted) {
      playClip("resting");
      return;
    }

    if (isWorking) {
      if (currentClip === "resting" || currentClip === "stopWorking") {
        playClip("startWorking");
        pendingTransition.current = "working";
      }
    } else {
      if (currentClip === "working" || currentClip === "startWorking") {
        playClip("stopWorking");
        pendingTransition.current = "resting";
      }
    }
  }, [isWorking, isStarted, currentClip, playClip]);

  // Handle video ended → transition to next clip
  const handleEnded = useCallback(() => {
    if (pendingTransition.current) {
      playClip(pendingTransition.current);
      pendingTransition.current = null;
    }
  }, [playClip]);

  // Handle pause/resume
  useEffect(() => {
    if (!videoRef.current) return;
    if (isPaused) {
      videoRef.current.pause();
    } else {
      videoRef.current.play().catch(() => {});
    }
  }, [isPaused]);

  // Re-load clip on orientation change
  useEffect(() => {
    if (manifest) {
      playClip(currentClip);
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [orientation]);

  // Initial load
  useEffect(() => {
    if (manifest) {
      playClip("resting");
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [manifest]);

  if (!manifest) {
    return (
      <div className="absolute inset-0 flex items-center justify-center bg-base-200">
        <span className="loading loading-spinner loading-lg"></span>
      </div>
    );
  }

  const dims = manifest.orientations[orientation];

  return (
    <div className="absolute inset-0 flex items-center justify-center overflow-hidden bg-base-200">
      <video
        ref={videoRef}
        muted
        playsInline
        autoPlay
        onEnded={handleEnded}
        style={{
          width: "100%",
          height: "100%",
          objectFit: "contain",
          maxWidth: `${dims.width}px`,
          maxHeight: `${dims.height}px`,
        }}
      />
    </div>
  );
}
