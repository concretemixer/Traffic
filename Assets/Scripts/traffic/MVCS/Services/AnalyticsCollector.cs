using System;
using System.Collections.Generic;
using Collector = UnityEngine.Analytics.Analytics;
using LocalyticsUnity;

namespace Traffic.MVCS.Services
{
    public class AnalyticsCollector
    {
        static class Params
        {
            public static readonly Dictionary<string, object> NONE = new Dictionary<string, object>();

            public static Dictionary<string, object> Simple(string key, object val)
            {
                return new Dictionary<string, object>()
                {
                    {key, val}
                };
            }

            public static Dictionary<string, object> Level(int levelId, float score)
            {
                return new Dictionary<string, object>()
                {
                    {"level", levelId},
                    {"score", score}
                };
            }
        }

        public void LogTutorialStep(TutorialStep _step)
        {
          //  Collector.CustomEvent("tutorial", Params.Simple("step", (int)_step));
            Localytics.TagEvent("tutorial", new Dictionary<string, string>() { { "step", _step.ToString() } });
        }

        public void LevelStart(int levelId)
        {
            Localytics.TagEvent("level_start", new Dictionary<string, string>() { { "level", levelId.ToString() } });
            //Collector.CustomEvent("level_start", Params.Simple("level", levelId));
        }

        public void LevelFail(int levelId, float score)
        {
            Localytics.TagEvent("level_fail", new Dictionary<string, string>() { { "level", levelId.ToString() } });
            //Collector.CustomEvent("level_fail", Params.Level(levelId, score));
        }

        public void LevelComplete(int levelId, float score)
        {
            Localytics.TagEvent("level_complete",new Dictionary<string, string>() { { "level", levelId.ToString()} });
            //Collector.CustomEvent("level_complete", Params.Level(levelId, score));
        }

        public void FacebookConnected()
        {
            Localytics.TagEvent("fb_connected");
            //Collector.CustomEvent("fb_connected", Params.NONE);
        }

        public void FacebookShareStart()
        {
            Localytics.TagEvent("fb_share_start");
            //Collector.CustomEvent("fb_share_start", Params.NONE);
        }

        public void FacebookShareComplete()
        {
            Localytics.TagEvent("fb_share_complete");
            //Collector.CustomEvent("fb_share_complete", Params.NONE);
        }

        public void NoTriesWindowShown()
        {
            Localytics.TagEvent("no_tries_window_shown");
            //Collector.CustomEvent("no_tries_window_shown", Params.NONE);
        }

        public void AdsStart()
        {
            Localytics.TagEvent("ads_start");
            //Collector.CustomEvent("ads_start", Params.NONE);
        }

        public void AdsSkiped()
        {
            Localytics.TagEvent("ads_skiped");
           // Collector.CustomEvent("ads_skiped", Params.NONE);
        }

        public void AdsComplete()
        {
            Localytics.TagEvent("ads_complete");
            //Collector.CustomEvent("ads_complete", Params.NONE);
        }

        public void AdsFailed()
        {
            Localytics.TagEvent("ads_failed");
            //Collector.CustomEvent("ads_failed", Params.NONE);
            
        }
    }
}