interface ProgressBarProps {
  secondsLeft: number;
  totalSeconds: number;
  isWorking: boolean;
}

export function ProgressBar({ secondsLeft, totalSeconds, isWorking }: ProgressBarProps) {
  const progress = totalSeconds > 0 ? ((totalSeconds - secondsLeft) / totalSeconds) * 100 : 0;
  const color = isWorking ? "var(--color-error)" : "var(--color-success)";
  const trackColor = isWorking ? "rgba(255,0,0,0.12)" : "rgba(0,200,80,0.12)";

  return (
    <div
      className="rounded-xl p-[3px]"
      style={{
        background: `conic-gradient(${color} ${progress}%, ${trackColor} ${progress}%)`,
        transition: "background 0.5s ease",
      }}
    >
      <div className="rounded-[9px] bg-base-100/80 backdrop-blur-sm px-4 py-2">
        {/* Inner content rendered by parent */}
      </div>
    </div>
  );
}

/** Wraps children with a conic-gradient progress border */
export function ProgressBorder({
  secondsLeft,
  totalSeconds,
  isWorking,
  children,
}: ProgressBarProps & { children: React.ReactNode }) {
  const progress = totalSeconds > 0 ? ((totalSeconds - secondsLeft) / totalSeconds) * 100 : 0;
  const color = isWorking ? "var(--color-error)" : "var(--color-success)";
  const trackColor = isWorking ? "rgba(255,0,0,0.12)" : "rgba(0,200,80,0.12)";

  return (
    <div
      className="rounded-xl p-[2px] inline-flex"
      style={{
        background: `conic-gradient(${color} ${progress}%, ${trackColor} ${progress}%)`,
        transition: "background 0.5s ease",
      }}
    >
      <div className="rounded-[10px] bg-base-100/90 backdrop-blur-sm">
        {children}
      </div>
    </div>
  );
}
