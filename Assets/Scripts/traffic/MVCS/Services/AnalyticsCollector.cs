using System.Collections.Generic;
using Collector = UnityEngine.Analytics.Analytics;

namespace Traffic.MVCS.Services
{
    public class AnalyticsCollector
    {
        public void LogTutorialStep(TutoroalStep _step)
        {
            logOnePropEvent("tutorial", "step", (int)_step);
        }

        public void LevelStart(int levelId)
        {
            logOnePropEvent("level_start", "level_id", levelId);
        }

        public void LevelFail(int levelId)
        {
            logOnePropEvent("level_fail", "level_id", levelId);
        }

        public void LevelComplete(int levelId)
        {
            logOnePropEvent("level_complete", "level_id", levelId);
        }

        void logOnePropEvent(string eventName, string paramName, object param)
        {
            Collector.CustomEvent(eventName, new Dictionary<string, object>() {
                {paramName, param}
            });
        }
    }
}