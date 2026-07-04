export interface AnimationClip {
  landscape: string;
  portrait: string;
  loop: boolean;
}

export interface AnimationManifest {
  id: string;
  name: string;
  orientations: {
    landscape: { width: number; height: number };
    portrait: { width: number; height: number };
  };
  clips: {
    resting: AnimationClip;
    startWorking: AnimationClip;
    working: AnimationClip;
    stopWorking: AnimationClip;
  };
  thumbnails: {
    rest: string;
    work: string;
  };
  audio: {
    bell: string;
    bellFinish: string;
  };
}

export type ClipState = "resting" | "startWorking" | "working" | "stopWorking";
export type Orientation = "landscape" | "portrait";
