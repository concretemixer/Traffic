using System;
using System.Collections.Generic;
using Collector = UnityEngine.Analytics.Analytics;
using LocalyticsUnity;

namespace Traffic.MVCS.Services
{
    public class AnalyticsCollector
    {
        /*
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
          */
        public void LogTutorialStep(TutorialStep _step)
        {
            //  Collector.CustomEvent("tutorial", Params.Simple("step", (int)_step));
#if (UNITY_ANDROID || UNITY_IOS)
            Localytics.TagEvent("tutorial", new Dictionary<string, string>() { { "step", _step.ToString() } });
#endif
        }

        public void LevelStart(int levelId)
        {
#if (UNITY_ANDROID || UNITY_IOS)
            Localytics.TagEvent("level_start", new Dictionary<string, string>() { { "level", levelId.ToString() } });
            //Collector.CustomEvent("level_start", Params.Simple("level", levelId));
#endif
        }

        public void LevelFail(int levelId, float score)
        {
#if (UNITY_ANDROID || UNITY_IOS)
            Localytics.TagEvent("level_fail", new Dictionary<string, string>() { { "level", levelId.ToString() } });
            //Collector.CustomEvent("level_fail", Params.Level(levelId, score));
#endif
        }

        public void LevelComplete(int levelId, float score)
        {
#if (UNITY_ANDROID || UNITY_IOS)
            Localytics.TagEvent("level_complete",new Dictionary<string, string>() { { "level", levelId.ToString()} });
            //Collector.CustomEvent("level_complete", Params.Level(levelId, score));
#endif
        }

        public void FacebookConnected()
        {
#if (UNITY_ANDROID || UNITY_IOS)
            Localytics.TagEvent("fb_connected");
            //Collector.CustomEvent("fb_connected", Params.NONE);
#endif
        }

        public void FacebookShareStart()
        {
#if (UNITY_ANDROID || UNITY_IOS)
            Localytics.TagEvent("fb_share_start");
            //Collector.CustomEvent("fb_share_start", Params.NONE);
#endif
        }

        public void FacebookShareComplete()
        {
#if (UNITY_ANDROID || UNITY_IOS)
            Localytics.TagEvent("fb_share_complete");
            //Collector.CustomEvent("fb_share_complete", Params.NONE);
#endif
        }

        public void NoTriesWindowShown()
        {
#if (UNITY_ANDROID || UNITY_IOS)
            Localytics.TagEvent("no_tries_window_shown");
            //Collector.CustomEvent("no_tries_window_shown", Params.NONE);
#endif
        }

        public void AdsStart()
        {
#if (UNITY_ANDROID || UNITY_IOS)
            Localytics.TagEvent("ads_start");
            //Collector.CustomEvent("ads_start", Params.NONE);
#endif
        }

        public void AdsSkiped()
        {
#if (UNITY_ANDROID || UNITY_IOS)
            Localytics.TagEvent("ads_skiped");
            // Collector.CustomEvent("ads_skiped", Params.NONE);
#endif
        }

        public void AdsComplete()
        {
#if (UNITY_ANDROID || UNITY_IOS)
            Localytics.TagEvent("ads_complete");
            //Collector.CustomEvent("ads_complete", Params.NONE);
#endif
        }

        public void AdsFailed()
        {
#if (UNITY_ANDROID || UNITY_IOS)
            Localytics.TagEvent("ads_failed");
#endif
            //Collector.CustomEvent("ads_failed", Params.NONE);

        }
    }
}