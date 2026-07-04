interface ProgressRingProps {
  secondsLeft: number;
  totalSeconds: number;
  isWorking: boolean;
}

export function ProgressRing({ secondsLeft, totalSeconds, isWorking }: ProgressRingProps) {
  const progress = totalSeconds > 0 ? secondsLeft / totalSeconds : 0;
  const color = isWorking ? "var(--color-error)" : "var(--color-success)";
  const trackColor = isWorking ? "rgba(255,0,0,0.15)" : "rgba(0,200,80,0.15)";

  // Compact ring — wraps around the timer text
  const size = 120;
  const strokeWidth = 4;
  const radius = (size - strokeWidth) / 2;
  const circumference = 2 * Math.PI * radius;
  const dashOffset = circumference * (1 - progress);

  return (
    <svg
      width={size}
      height={size}
      className="absolute inset-0 m-auto pointer-events-none"
      style={{ transform: "rotate(-90deg)" }}
    >
      {/* Track */}
      <circle
        cx={size / 2}
        cy={size / 2}
        r={radius}
        fill="none"
        stroke={trackColor}
        strokeWidth={strokeWidth}
      />
      {/* Progress */}
      <circle
        cx={size / 2}
        cy={size / 2}
        r={radius}
        fill="none"
        stroke={color}
        strokeWidth={strokeWidth}
        strokeDasharray={circumference}
        strokeDashoffset={dashOffset}
        strokeLinecap="round"
        style={{ transition: "stroke-dashoffset 0.5s ease" }}
      />
    </svg>
  );
}
