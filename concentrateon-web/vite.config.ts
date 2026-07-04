import { defineConfig } from "vite";
import react from "@vitejs/plugin-react";
import tailwindcss from "@tailwindcss/vite";
import { VitePWA } from "vite-plugin-pwa";

export default defineConfig({
  plugins: [
    react(),
    tailwindcss(),
    VitePWA({
      registerType: "autoUpdate",
      includeAssets: [
        "animations/ancient-man/*.webm",
        "animations/ancient-man/*.png",
        "animations/ancient-man/*.mp3",
      ],
      manifest: {
        name: "ConcentrateOn",
        short_name: "ConcentrateOn",
        description: "Pomodoro timer with animated characters",
        theme_color: "#1d232a",
        background_color: "#ffffff",
        display: "standalone",
        start_url: "/",
        icons: [
          {
            src: "/icon-192.png",
            sizes: "192x192",
            type: "image/png",
          },
          {
            src: "/icon-512.png",
            sizes: "512x512",
            type: "image/png",
          },
        ],
      },
      workbox: {
        globPatterns: ["**/*.{js,css,html,webm,png,mp3,json}"],
      },
    }),
  ],
  worker: {
    format: "es",
  },
});
