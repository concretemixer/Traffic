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

        void SetDimentionsSessions()
        {
#if (UNITY_ANDROID || UNITY_IOS)
            Int32 _sessions = PlayerPrefs.GetInt("stats.sessions_count", 0);
            _sessions++;
            if (_sessions <= 5)
                Localytics.SetCustomDimension(3, _sessions.ToString());
            else if (_sessions <= 10)
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
#endif
        }

        public void SetDimentions()
        {
#if (UNITY_ANDROID || UNITY_IOS)
            SetDimentionsSessions();

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

            PlayerPrefs.SetInt("stats.level_ch_1", packs[0]);
            PlayerPrefs.SetInt("stats.level_ch_2", packs[1]);
            PlayerPrefs.SetInt("stats.level_ch_3", packs[2]);
#endif
        }

        public void LogTutorialStep(TutorialStep _step)
        {
            //  Collector.CustomEvent("tutorial", Params.Simple("step", (int)_step));
#if (UNITY_ANDROID || UNITY_IOS)
            Localytics.TagEvent("Tutorial", new Dictionary<string, string>() { { "step", _step.ToString() } });
#endif
        }

        public void LevelStart(int levelId)
        {
            //Int32 _tries = PlayerPrefs.GetInt("stats.tries."+levelId.ToString(), 0);
           // _tries++;
           // PlayerPrefs.SetInt("stats.tries." + levelId.ToString(), _tries);
            return;
#if (UNITY_ANDROID || UNITY_IOS)


            Localytics.TagEvent("level_start", new Dictionary<string, string>() { { "level", levelId.ToString() } });
            //Collector.CustomEvent("level_start", Params.Simple("level", levelId));
#endif
        }

        public void LevelFail(int levelId, float score)
        {
            return;
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
            return;
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
            Int32 _tries = PlayerPrefs.GetInt("stats.tries." + levelId.ToString(), 0);
            _tries++;
            PlayerPrefs.SetInt("stats.tries." + levelId.ToString(), _tries);
#if (UNITY_ANDROID || UNITY_IOS)
            Localytics.TagEvent("Level Result", 
                new Dictionary<string, string>() {                    
                    { "Chapter", (((int)(levelId / 9))+1).ToString() },
                    { "Level", ((levelId % 9)+1).ToString() },
                    { "Result", result },
                    { "Try Num", _tries.ToString() },

            { "Session Number",(PlayerPrefs.GetInt("stats.sessions_count", 0)+1).ToString() },
            { "Level Ch 1",PlayerPrefs.GetInt("stats.level_ch_1", 0).ToString() },
            { "Level Ch 2",PlayerPrefs.GetInt("stats.level_ch_2", 0).ToString() },
            { "Level Ch 3",PlayerPrefs.GetInt("stats.level_ch_3", 0).ToString() },

        });            
#endif
        }


        public void SessionStart()
        {            
#if (UNITY_ANDROID || UNITY_IOS)
            Int32 unixTimestamp = PlayerPrefs.GetInt("stats.last_session_start", 0);
            DateTime LastSessionStart = new DateTime(1970, 1, 1).AddSeconds(unixTimestamp);

            TimeSpan span = DateTime.Now - LastSessionStart;

            if (span.TotalSeconds > 300)
            {
                Int32 _sessions = PlayerPrefs.GetInt("stats.sessions_count", 0);
                _sessions++;
                PlayerPrefs.SetInt("stats.sessions_count", _sessions);

                SetDimentionsSessions();

                Localytics.TagEvent("Session Start",
                    new Dictionary<string, string>() {
                    { "Session Number", _sessions.ToString() },            
                    { "Level Ch 1",PlayerPrefs.GetInt("stats.level_ch_1", 0).ToString() },
                    { "Level Ch 2",PlayerPrefs.GetInt("stats.level_ch_2", 0).ToString() },
                    { "Level Ch 3",PlayerPrefs.GetInt("stats.level_ch_3", 0).ToString() },
                    });

                if (_sessions <= 1)
                {
                    Localytics.TagEvent("First Launch");
                }

                unixTimestamp = (Int32)(DateTime.Now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                PlayerPrefs.SetInt("stats.last_session_start", unixTimestamp);

            }
#endif
        }


        public void FacebookConnected()
        {
#if (UNITY_ANDROID || UNITY_IOS)
            Localytics.TagEvent("FB Connected");
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
            Localytics.TagEvent("No Tries Window");
            //Collector.CustomEvent("no_tries_window_shown", Params.NONE);
#endif
        }

        public void AdsStart()
        {
#if (UNITY_ANDROID || UNITY_IOS)
            Localytics.TagEvent("Ads Start");
            //Collector.CustomEvent("ads_start", Params.NONE);
#endif
        }

        public void AdsSkiped()
        {
#if (UNITY_ANDROID || UNITY_IOS)
            Localytics.TagEvent("Ads Skiped");
            // Collector.CustomEvent("ads_skiped", Params.NONE);
#endif
        }

        public void AdsComplete()
        {
#if (UNITY_ANDROID || UNITY_IOS)
            Localytics.TagEvent("Ads_Complete");
            //Collector.CustomEvent("ads_complete", Params.NONE);
#endif
        }

        public void AdsFailed()
        {
#if (UNITY_ANDROID || UNITY_IOS)
            Localytics.TagEvent("Ads Failed");
#endif
            //Collector.CustomEvent("ads_failed", Params.NONE);

        }
    }
}