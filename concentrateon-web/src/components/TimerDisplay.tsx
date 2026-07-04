import { useTimerStore } from "../stores/timerStore";

export function TimerDisplay() {
  const { secondsLeft, isWorking, isShortRest, isStarted } = useTimerStore();

  const totalSeconds = isStarted
    ? secondsLeft
    : useTimerStore.getState().workTimeMinutes * 60;
  const hours = Math.floor(totalSeconds / 3600);
  const minutes = Math.floor((totalSeconds % 3600) / 60);
  const seconds = totalSeconds % 60;

  const timeStr =
    hours > 0
      ? `${hours}:${String(minutes).padStart(2, "0")}:${String(seconds).padStart(2, "0")}`
      : `${String(minutes).padStart(2, "0")}:${String(seconds).padStart(2, "0")}`;

  const statusText = !isStarted
    ? "Ready"
    : isWorking
      ? "Concentrate"
      : isShortRest
        ? "Short Rest"
        : "Long Rest";

  const counterColor = isWorking ? "text-error" : "text-success";

  return (
    <div className="flex flex-col items-center gap-2">
      <div className="badge badge-lg badge-outline text-lg px-4 py-3">
        {statusText}
      </div>
      <div
        className={`font-mono text-6xl font-bold tabular-nums ${counterColor}`}
      >
        {timeStr}
      </div>
    </div>
  );
}
