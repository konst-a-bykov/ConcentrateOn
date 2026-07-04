export function About() {
  return (
    <div className="container mx-auto max-w-2xl p-6">
      <h1 className="text-3xl font-bold mb-6">About ConcentrateOn</h1>

      <div className="space-y-6">
        <div className="card bg-base-100 shadow-md">
          <div className="card-body">
            <h2 className="card-title">The Pomodoro Technique</h2>
            <p>
              The Pomodoro Technique is a time management method developed by
              Francesco Cirillo in the late 1980s. It uses a timer to break work
              into intervals, traditionally 25 minutes in length, separated by
              short breaks.
            </p>
            <p>
              After four work periods, a longer break is taken. This cycle helps
              maintain focus and prevent burnout.
            </p>
          </div>
        </div>

        <div className="card bg-base-100 shadow-md">
          <div className="card-body">
            <h2 className="card-title">ConcentrateOn</h2>
            <p>
              ConcentrateOn is a Pomodoro timer with animated characters that
              react to your work and rest states. The character works when you
              work and rests when you rest — a visual companion for your focus
              sessions.
            </p>
            <p>
              Originally built as a Windows UWP app, now available on the web.
            </p>
          </div>
        </div>

        <div className="card bg-base-100 shadow-md">
          <div className="card-body">
            <h2 className="card-title">Default Settings</h2>
            <ul className="list-disc list-inside space-y-1">
              <li>Work period: 25 minutes</li>
              <li>Short rest: 5 minutes</li>
              <li>Long rest: 15 minutes (every 4th break)</li>
            </ul>
          </div>
        </div>
      </div>
    </div>
  );
}
