import { useTranslation } from "react-i18next";

export function About() {
  const { t } = useTranslation();

  return (
    <div className="container mx-auto max-w-2xl p-6">
      <h1 className="text-3xl font-bold mb-6">{t("about.title")}</h1>

      <div className="space-y-6">
        <div className="card bg-base-100 shadow-md">
          <div className="card-body">
            <h2 className="card-title">{t("about.technique")}</h2>
            <p>{t("about.techniqueText")}</p>
          </div>
        </div>

        <div className="card bg-base-100 shadow-md">
          <div className="card-body">
            <h2 className="card-title">{t("about.app")}</h2>
            <p>{t("about.appText")}</p>
          </div>
        </div>

        <div className="card bg-base-100 shadow-md">
          <div className="card-body">
            <h2 className="card-title">{t("about.defaults")}</h2>
            <ul className="list-disc list-inside space-y-1">
              <li>{t("about.workPeriod")}</li>
              <li>{t("about.shortRestDefault")}</li>
              <li>{t("about.longRestDefault")}</li>
            </ul>
          </div>
        </div>
      </div>
    </div>
  );
}