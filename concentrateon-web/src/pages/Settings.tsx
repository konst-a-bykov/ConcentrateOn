import { useTimerStore } from "../stores/timerStore";
import { useAnimationStore } from "../stores/animationStore";
import { useTranslation } from "react-i18next";
import { setLanguage } from "../i18n";

export function Settings() {
  const {
    workTimeMinutes,
    shortRestMinutes,
    longRestMinutes,
    intervalForLongRest,
    volume,
    updateSettings,
  } = useTimerStore();

  const { selectedPackId, setPack, packs } = useAnimationStore();
  const packList = Object.values(packs);

  const { t, i18n } = useTranslation();

  return (
    <div className="container mx-auto max-w-2xl p-6">
      <h1 className="text-3xl font-bold mb-6">{t("settings.title")}</h1>

      <div className="space-y-6">
        {/* Timer durations */}
        <div className="card bg-base-100 shadow-md">
          <div className="card-body">
            <h2 className="card-title">{t("settings.timer")}</h2>
            <div className="form-control">
              <label className="label">
                <span className="label-text">{t("settings.workDuration")}</span>
              </label>
              <input
                type="number"
                className="input input-bordered w-32"
                value={workTimeMinutes}
                min={1}
                max={120}
                onChange={(e) =>
                  updateSettings({
                    workTimeMinutes: parseInt(e.target.value) || 25,
                  })
                }
              />
            </div>
            <div className="form-control">
              <label className="label">
                <span className="label-text">{t("settings.shortRest")}</span>
              </label>
              <input
                type="number"
                className="input input-bordered w-32"
                value={shortRestMinutes}
                min={1}
                max={30}
                onChange={(e) =>
                  updateSettings({
                    shortRestMinutes: parseInt(e.target.value) || 5,
                  })
                }
              />
            </div>
            <div className="form-control">
              <label className="label">
                <span className="label-text">{t("settings.longRest")}</span>
              </label>
              <input
                type="number"
                className="input input-bordered w-32"
                value={longRestMinutes}
                min={1}
                max={60}
                onChange={(e) =>
                  updateSettings({
                    longRestMinutes: parseInt(e.target.value) || 15,
                  })
                }
              />
            </div>
            <div className="form-control">
              <label className="label">
                <span className="label-text">
                  {t("settings.intervalForLongRest")}
                </span>
              </label>
              <input
                type="number"
                className="input input-bordered w-32"
                value={intervalForLongRest}
                min={1}
                max={10}
                onChange={(e) =>
                  updateSettings({
                    intervalForLongRest: parseInt(e.target.value) || 4,
                  })
                }
              />
            </div>
          </div>
        </div>

        {/* Animation picker */}
        <div className="card bg-base-100 shadow-md">
          <div className="card-body">
            <h2 className="card-title">{t("settings.animation")}</h2>
            <select
              className="select select-bordered w-full max-w-xs"
              value={selectedPackId}
              onChange={(e) => setPack(e.target.value)}
            >
              {packList.length === 0 ? (
                <option disabled>Loading animations...</option>
              ) : (
                packList.map((pack) => (
                  <option key={pack.id} value={pack.id}>
                    {pack.name}
                  </option>
                ))
              )}
            </select>
          </div>
        </div>
        {/* Sound volume */}
        <div className="card bg-base-100 shadow-md">
          <div className="card-body">
            <h2 className="card-title">{t("settings.sound")}</h2>
            <div className="form-control">
              <label className="label">
                <span className="label-text">{t("settings.volume")}</span>
              </label>
              <select
                className="select select-bordered w-full max-w-xs"
                value={volume}
                onChange={(e) =>
                  updateSettings({ volume: parseInt(e.target.value) })
                }
              >
                <option value={0}>{t("settings.mute")}</option>
                <option value={25}>25%</option>
                <option value={50}>50%</option>
                <option value={75}>75%</option>
                <option value={100}>100%</option>
              </select>
            </div>
          </div>
        </div>

        {/* Language */}
        <div className="card bg-base-100 shadow-md">
          <div className="card-body">
            <h2 className="card-title">{t("settings.language")}</h2>
            <select
              className="select select-bordered w-full max-w-xs"
              value={i18n.language}
              onChange={(e) => setLanguage(e.target.value)}
            >
              <option value="en">English</option>
              <option value="de">Deutsch</option>
              <option value="fr">Français</option>
              <option value="he">עברית</option>
              <option value="ru">Русский</option>
            </select>
          </div>
        </div>

      </div>
    </div>
  );
}
