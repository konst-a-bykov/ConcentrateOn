import { useEffect, useRef, useState } from "react";
import { Link } from "react-router-dom";
import { useTimerStore } from "../stores/timerStore";
import { useAnimationStore } from "../stores/animationStore";
import { AnimationPlayer } from "../engine/AnimationPlayer";
import { useTimerSounds } from "../engine/useTimerSounds";
import { TimerDisplay } from "../components/TimerDisplay";
import { ControlButtons } from "../components/ControlButtons";
import { ThemeToggle } from "../components/ThemeToggle";

const navItems = [
  { path: "/settings", label: "Settings" },
  { path: "/statistics", label: "Statistics" },
  { path: "/about", label: "About" },
];

export function Home() {
  const { isStarted, isWorking, isShortRest, isLongRest, isPaused, tick } = useTimerStore();
  const manifest = useAnimationStore((s) => s.getSelectedManifest());
  const workerRef = useRef<Worker | null>(null);

  // Play sounds on work/rest transitions
  useTimerSounds(isStarted, isWorking, isShortRest, isLongRest);
  const [menuOpen, setMenuOpen] = useState(false);
  const menuRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    if (isStarted && !isPaused) {
      if (!workerRef.current) {
        workerRef.current = new Worker(
          new URL("../engine/timer.worker.ts", import.meta.url),
          { type: "module" }
        );
        workerRef.current.onmessage = () => {
          tick();
        };
      }
      workerRef.current.postMessage({ command: "start" });
    } else {
      workerRef.current?.postMessage({ command: "stop" });
    }

    return () => {
      workerRef.current?.postMessage({ command: "stop" });
    };
  }, [isStarted, isPaused, tick]);

  useEffect(() => {
    return () => {
      workerRef.current?.terminate();
      workerRef.current = null;
    };
  }, []);

  // Close menu on outside click
  useEffect(() => {
    if (!menuOpen) return;
    const handler = (e: MouseEvent) => {
      if (menuRef.current && !menuRef.current.contains(e.target as Node)) {
        setMenuOpen(false);
      }
    };
    document.addEventListener("mousedown", handler);
    return () => document.removeEventListener("mousedown", handler);
  }, [menuOpen]);

  return (
    <div className="relative h-screen w-screen overflow-hidden">
      <AnimationPlayer
        manifest={manifest}
        isWorking={isWorking}
        isStarted={isStarted}
        isPaused={isPaused}
      />

      {/* Floating menu button — top left */}
      <div ref={menuRef} className="absolute top-3 left-3 z-30">
        <button
          className="btn btn-circle btn-sm bg-base-100/80 backdrop-blur-sm shadow-md border-none"
          onClick={() => setMenuOpen(!menuOpen)}
        >
          <svg xmlns="http://www.w3.org/2000/svg" className="h-5 w-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M4 6h16M4 12h16M4 18h16" />
          </svg>
        </button>
        {menuOpen && (
          <ul className="menu bg-base-100/90 backdrop-blur-sm rounded-box mt-2 w-44 p-2 shadow-lg">
            {navItems.map((item) => (
              <li key={item.path}>
                <Link to={item.path} onClick={() => setMenuOpen(false)}>
                  {item.label}
                </Link>
              </li>
            ))}
            <li>
              <div className="flex justify-between items-center">
                <span>Theme</span>
                <ThemeToggle onToggle={() => setMenuOpen(false)} />
              </div>
            </li>
          </ul>
        )}
      </div>

      {/* Timer + controls — top center */}
      <div className="absolute top-3 left-12 right-3 flex justify-center z-10">
        <div className="card bg-base-100/80 backdrop-blur-sm shadow-xl px-4 py-3">
          <div className="flex flex-col portrait:flex-col landscape:flex-row items-center gap-2 landscape:gap-4">
            <TimerDisplay />
            <ControlButtons />
          </div>
        </div>
      </div>
    </div>
  );
}
