import { BrowserRouter, Routes, Route } from "react-router-dom";
import { useEffect } from "react";
import { Layout } from "./components/Layout";
import { Home } from "./pages/Home";
import { Settings } from "./pages/Settings";
import { Statistics } from "./pages/Statistics";
import { About } from "./pages/About";
import { loadAllAnimationPacks } from "./engine/animationLoader";
import { useThemeStore } from "./stores/themeStore";

export default function App() {
  // Initialize theme on mount
  const theme = useThemeStore((s) => s.theme);
  useEffect(() => {
    document.documentElement.setAttribute("data-theme", theme);
  }, [theme]);

  // Load animation packs on mount
  useEffect(() => {
    loadAllAnimationPacks();
  }, []);

  return (
    <BrowserRouter>
      <Layout>
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/settings" element={<Settings />} />
          <Route path="/statistics" element={<Statistics />} />
          <Route path="/about" element={<About />} />
        </Routes>
      </Layout>
    </BrowserRouter>
  );
}
