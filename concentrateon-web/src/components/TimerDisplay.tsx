import { useTimerStore } from "../stores/timerStore";
import { useTranslation } from "react-i18next";
import { ProgressRing } from "./ProgressRing";

export function TimerDisplay() {
  const { secondsLeft, isWorking, isShortRest, isStarted, workTimeMinutes, shortRestMinutes, longRestMinutes } = useTimerStore();
  const { t } = useTranslation();

  const totalSeconds = isStarted
    ? secondsLeft
    : workTimeMinutes * 60;

  // Period total for progress calculation
  const periodTotal = !isStarted
    ? workTimeMinutes * 60
    : isWorking
      ? workTimeMinutes * 60
      : isShortRest
        ? shortRestMinutes * 60
        : longRestMinutes * 60;

  const hours = Math.floor(totalSeconds / 3600);
  const minutes = Math.floor((totalSeconds % 3600) / 60);
  const seconds = totalSeconds % 60;

  const timeStr =
    hours > 0
      ? `${hours}:${String(minutes).padStart(2, "0")}:${String(seconds).padStart(2, "0")}`
      : `${String(minutes).padStart(2, "0")}:${String(seconds).padStart(2, "0")}`;

  const statusText = !isStarted
    ? t("timer.ready")
    : isWorking
      ? t("timer.concentrate")
      : isShortRest
        ? t("timer.shortRest")
        : t("timer.longRest");

  const counterColor = isWorking ? "text-error" : "text-success";

  return (
    <div className="flex portrait:flex-col items-center gap-2">
      <div className="badge badge-outline landscape:badge-sm landscape:text-sm portrait:badge-lg portrait:text-lg px-3 py-2">
        {statusText}
      </div>
      {/* Timer with embedded progress ring */}
      <div className="relative flex items-center justify-center">
        <ProgressRing
          secondsLeft={secondsLeft}
          totalSeconds={periodTotal}
          isWorking={isWorking}
        />
        <div
          className={`font-mono landscape:text-4xl portrait:text-6xl font-bold tabular-nums ${counterColor} relative z-10`}
        >
          {timeStr}
        </div>
      </div>
    </div>
  );
}
