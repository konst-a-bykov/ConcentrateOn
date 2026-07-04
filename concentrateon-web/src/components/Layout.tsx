import { Link, useLocation } from "react-router-dom";
import { ThemeToggle } from "./ThemeToggle";

const navItems = [
  { path: "/", label: "Home" },
  { path: "/settings", label: "Settings" },
  { path: "/statistics", label: "Statistics" },
  { path: "/about", label: "About" },
];

export function Layout({ children }: { children: React.ReactNode }) {
  const location = useLocation();

  // Home page handles its own layout (full viewport video)
  if (location.pathname === "/") {
    return <>{children}</>;
  }

  return (
    <div className="flex flex-col h-screen">
      <div className="navbar bg-base-100 shadow-md shrink-0">
        {/* Hamburger: narrow screens */}
        <div className="flex-none sm:hidden">
          <div className="dropdown">
            <label tabIndex={0} className="btn btn-square btn-ghost">
              <svg
                xmlns="http://www.w3.org/2000/svg"
                className="h-5 w-5"
                fill="none"
                viewBox="0 0 24 24"
                stroke="currentColor"
              >
                <path
                  strokeLinecap="round"
                  strokeLinejoin="round"
                  strokeWidth={2}
                  d="M4 6h16M4 12h16M4 18h16"
                />
              </svg>
            </label>
            <ul
              tabIndex={0}
              className="dropdown-content menu bg-base-200 rounded-box z-10 w-52 p-2 shadow"
            >
              {navItems.map((item) => (
                <li key={item.path}>
                  <Link
                    to={item.path}
                    className={location.pathname === item.path ? "active" : ""}
                  >
                    {item.label}
                  </Link>
                </li>
              ))}
            </ul>
          </div>
        </div>

        <div className="flex-1 min-w-0">
          <span className="text-lg font-bold px-2 truncate">ConcentrateOn</span>
        </div>

        {/* Inline links: wide screens */}
        <div className="flex-none hidden sm:flex">
          <ul className="menu menu-horizontal px-1 gap-1">
            {navItems.map((item) => (
              <li key={item.path}>
                <Link
                  to={item.path}
                  className={location.pathname === item.path ? "active" : ""}
                >
                  {item.label}
                </Link>
              </li>
            ))}
          </ul>
        </div>

        <div className="flex-none">
          <ThemeToggle />
        </div>
      </div>

      <main className="flex-1 min-h-0 overflow-auto">{children}</main>
    </div>
  );
}
