using System;
using System.Collections.Generic;
using Commons.SN.Facebook;
using Collector = UnityEngine.Analytics.Analytics;
using LocalyticsUnity;
using UnityEngine;
using Traffic.Core;

namespace Traffic.MVCS.Services
{
    public class AnalyticsCollector
    {
        [Inject]
        public FacebookSN facebook { private get; set; }

        [Inject]
        public ILevelListModel levels { get; set; }

        public void SetDimentions()
        {
            Int32 _sessions = PlayerPrefs.GetInt("stats.sessions_count", 0);
            _sessions++;
            if (_sessions<=5)
                Localytics.SetCustomDimension(3, _sessions.ToString());
            else if (_sessions<=10)
                Localytics.SetCustomDimension(3, "6-10");
            else if (_sessions <= 25)
                Localytics.SetCustomDimension(3, "11-25");
            else if (_sessions <= 50)
                Localytics.SetCustomDimension(3, "26-50");
            else if (_sessions <= 50)
                Localytics.SetCustomDimension(3, "26-50");
            else if (_sessions <= 100)
                Localytics.SetCustomDimension(3, "51-100");
            else if (_sessions <= 200)
                Localytics.SetCustomDimension(3, "101-200");
            else
                Localytics.SetCustomDimension(3, "200+");

            if (facebook!=null && facebook.IsLoggedIn)
                Localytics.SetCustomDimension(4, "Facebook");

            int[] packs = { 0, 0, 0 };

            for (int a = 0; a < 3; a++)
            {
                for (int b = 0; b < 9; b++)
                {
                    switch (levels.GetLevelState(a * 9 + b))
                    {
                        case LevelState.PassedOneStar:
                        case LevelState.PassedTwoStars:
                        case LevelState.PassedThreeStars:
                            packs[a] = b + 1;
                            break;
                    }
                }
            }

            Localytics.SetCustomDimension(0, packs[0].ToString());
            Localytics.SetCustomDimension(1, packs[1].ToString());
            Localytics.SetCustomDimension(2, packs[2].ToString());
        }

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
            Localytics.TagEvent("level_fail", new Dictionary<string, string>() {
                { "level", levelId.ToString() },
                    { "pack", (levelId / 9).ToString() },
                    { "level_in_pack", (levelId % 9).ToString() },

            });
            //Collector.CustomEvent("level_fail", Params.Level(levelId, score));
#endif
        }

        public void LevelComplete(int levelId, float score)
        {
#if (UNITY_ANDROID || UNITY_IOS)
            Localytics.TagEvent("level_complete",new Dictionary<string, string>() {
                { "level", levelId.ToString()}  ,
                { "pack", (levelId / 9).ToString() },
                { "level_in_pack", (levelId % 9).ToString() },
            });
            //Collector.CustomEvent("level_complete", Params.Level(levelId, score));
#endif
        }

        public void LevelResult(int levelId, string result)
        {
#if (UNITY_ANDROID || UNITY_IOS)
            Localytics.TagEvent("level_result", 
                new Dictionary<string, string>() {
                    { "level", levelId.ToString() },
                    { "pack", (levelId / 9).ToString() },
                    { "level_in_pack", (levelId % 9).ToString() },
                    { "result", result },
                });            
#endif
        }


        public void SessionStart()
        {            
#if (UNITY_ANDROID || UNITY_IOS)
            Int32 unixTimestamp = PlayerPrefs.GetInt("stats.last_session_start", 0);
            DateTime LastSessionStart = new DateTime(1970, 1, 1).AddSeconds(unixTimestamp);

            TimeSpan span = LastSessionStart - DateTime.Now;

            if (span.TotalSeconds > 300)
            {
                Int32 _sessions = PlayerPrefs.GetInt("stats.sessions_count", 0);
                _sessions++;
                PlayerPrefs.SetInt("stats.sessions_count", _sessions);

                Localytics.TagEvent("session_start",
                    new Dictionary<string, string>() {
                    { "number", _sessions.ToString() },                    
                    });

                if (_sessions <= 1)
                {
                    Localytics.TagEvent("first_launch");
                }

                unixTimestamp = (Int32)(DateTime.Now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                PlayerPrefs.SetInt("tries.last_session_start", unixTimestamp);

            }
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