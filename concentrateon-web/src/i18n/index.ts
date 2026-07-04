import i18n from "i18next";
import { initReactI18next } from "react-i18next";
import en from "./locales/en.json";
import de from "./locales/de.json";
import fr from "./locales/fr.json";
import he from "./locales/he.json";
import ru from "./locales/ru.json";

const savedLang = localStorage.getItem("concentrateon-lang") || detectLanguage();

function detectLanguage(): string {
  const lang = navigator.language;
  if (lang.startsWith("de")) return "de";
  if (lang.startsWith("fr")) return "fr";
  if (lang.startsWith("he")) return "he";
  if (lang.startsWith("ru")) return "ru";
  return "en";
}

i18n.use(initReactI18next).init({
  resources: {
    en: { translation: en },
    de: { translation: de },
    fr: { translation: fr },
    he: { translation: he },
    ru: { translation: ru },
  },
  lng: savedLang,
  fallbackLng: "en",
  interpolation: { escapeValue: false },
});

export function setLanguage(lang: string) {
  i18n.changeLanguage(lang);
  localStorage.setItem("concentrateon-lang", lang);
  // Set document direction for Hebrew
  document.documentElement.dir = lang === "he" ? "rtl" : "ltr";
}

export default i18n;
