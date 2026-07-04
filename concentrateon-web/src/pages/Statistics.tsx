import { useStatisticsStore } from "../stores/statisticsStore";

const activityLabels: Record<string, string> = {
  WorkingTime: "Work",
  ShortRest: "Short Rest",
  LongRest: "Long Rest",
};

export function Statistics() {
  const { entries, clearEntries } = useStatisticsStore();

  const formatDuration = (seconds: number) => {
    const m = Math.floor(seconds / 60);
    const s = seconds % 60;
    return m > 0 ? `${m}m ${s}s` : `${s}s`;
  };

  const formatDate = (ms: number) => {
    return new Date(ms).toLocaleString();
  };

  // Summary
  const totalWork = entries
    .filter((e) => e.activityType === "WorkingTime")
    .reduce((sum, e) => sum + e.durationSeconds, 0);
  const totalRest = entries
    .filter((e) => e.activityType !== "WorkingTime")
    .reduce((sum, e) => sum + e.durationSeconds, 0);

  return (
    <div className="container mx-auto max-w-4xl p-6">
      <div className="flex items-center justify-between mb-6">
        <h1 className="text-3xl font-bold">Statistics</h1>
        {entries.length > 0 && (
          <button className="btn btn-ghost btn-sm" onClick={clearEntries}>
            Clear All
          </button>
        )}
      </div>

      {/* Summary cards */}
      <div className="stats shadow mb-6">
        <div className="stat">
          <div className="stat-title">Total Work</div>
          <div className="stat-value text-error">{formatDuration(totalWork)}</div>
        </div>
        <div className="stat">
          <div className="stat-title">Total Rest</div>
          <div className="stat-value text-success">{formatDuration(totalRest)}</div>
        </div>
        <div className="stat">
          <div className="stat-title">Sessions</div>
          <div className="stat-value">{entries.length}</div>
        </div>
      </div>

      {/* Log table */}
      {entries.length === 0 ? (
        <div className="text-center text-base-content/50 py-12">
          No activity logged yet. Start a timer to begin tracking.
        </div>
      ) : (
        <div className="overflow-x-auto">
          <table className="table table-zebra">
            <thead>
              <tr>
                <th>Time</th>
                <th>Activity</th>
                <th>Duration</th>
              </tr>
            </thead>
            <tbody>
              {entries
                .slice()
                .reverse()
                .map((entry, i) => (
                  <tr key={i}>
                    <td>{formatDate(entry.startTime)}</td>
                    <td>{activityLabels[entry.activityType] ?? entry.activityType}</td>
                    <td>{formatDuration(entry.durationSeconds)}</td>
                  </tr>
                ))}
            </tbody>
          </table>
        </div>
      )}
    </div>
  );
}
