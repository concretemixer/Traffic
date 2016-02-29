using System.Collections.Generic;
using Collector = UnityEngine.Analytics.Analytics;

namespace Traffic.MVCS.Services
{
    public class AnalyticsCollector
    {
        class Params
        {
            public static readonly Dictionary<string, object> NONE = new Dictionary<string, object>();

            public static Dictionary<string, object> Simple(string key, object val)
            {
                return new Dictionary<string, object>() {
                    {key, val}
                };
            }

            public static Dictionary<string, object> Level(int levelId, int score)
            {
                return new Dictionary<string, object>() {
                    {"level", levelId},
                    {"score", score}
                };
            }
        }

        public void LogTutorialStep(TutoroalStep _step)
        {
            Collector.CustomEvent("tutorial", Params.Simple("step", (int)_step));
        }

        public void LevelStart(int levelId)
        {
            Collector.CustomEvent("level_start", Params.Simple("level", levelId));
        }

        public void LevelFail(int levelId, int score)
        {
            Collector.CustomEvent("level_fail", Params.Level(levelId, score));
        }

        public void LevelComplete(int levelId, int score)
        {
            Collector.CustomEvent("level_complete", Params.Level(levelId, score));
        }

        public void FacebookConnected()
        {
            Collector.CustomEvent("fb_connected", Params.NONE);
        }
        
        public void FacebookShareStart()
        {
            Collector.CustomEvent("fb_share_start", Params.NONE);
        }
        
        public void FacebookShareComplete()
        {
            Collector.CustomEvent("fb_share_complete", Params.NONE);
        }
        
        public void AdsWindowShown()
        {
            Collector.CustomEvent("ads_window_shown", Params.NONE);
        }
        
        public void AdsStart()
        {
            Collector.CustomEvent("ads_start", Params.NONE);
        }
        
        public void AdsSkiped()
        {
            Collector.CustomEvent("ads_skiped", Params.NONE);
        }
        
        public void AdsComplete()
        {
            Collector.CustomEvent("ads_complete", Params.NONE);
        }

        void logOnePropEvent(string eventName, string paramName, object param)
        {
            Collector.CustomEvent(eventName, new Dictionary<string, object>() {
                {paramName, param}
            });
        }
    }
}