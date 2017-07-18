using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Commons.UI;
using strange.extensions.command.impl;
using Traffic.Components;
using Traffic.Core;
using Traffic.MVCS.Services;
using System.Collections;
using System.Collections.Generic;
using System;


namespace Traffic
{

 

    public class LevelScore
    {
        public LevelScore() { }

        public int state;
        public int score;
        public int attempts;
    }

    public class LevelScoreData
    {
        public LevelScoreData() { }
        public Dictionary<string, LevelScore> levels = new Dictionary<string, LevelScore>();

        public int tries;
        public int diff;        
    }

    public class TopEntry
    {
        public TopEntry() { }

        public string name;
        public int score;
        public int levels;
    }

    public class TopScoresData
    {
        public TopScoresData() { }
        public Dictionary<int, TopEntry> top = new Dictionary<int, TopEntry>();
    }

    public class WebDB : MonoBehaviour
    {
        string URL = "";
        string userId = "";
        Dictionary<string, string> urlParams = new Dictionary<string, string>();

        Dictionary<int, TopEntry> top;
        public Dictionary<int, TopEntry> Top
        {
            get
            {
                return top;
            }
        }

        public void Start()
        {
#if UNITY_WEBGL
            PlayerPrefs.SetInt("progress_ok", 0);

            urlParams = ParseUrlParams();
            if (urlParams.ContainsKey("viewer_id"))
            {
                userId = urlParams["viewer_id"];

                PlayerPrefs.SetString("user_id", userId);

                RequestProgress();
            }
            else
                PlayerPrefs.SetString("user_id", "0");

            //  string json = Resources.Load<TextAsset>("config/progress").text;

            //LoadFromJson(json);

#else
            PlayerPrefs.SetString("user_id", "0");
#endif
        }

        private IEnumerator WaitForRequest(WWW www)
        {
            yield return www;

            // check for errors
            if (www.error != null)
            {
                Debug.Log("WWW Error: " + www.error);
                if (www.url.Contains("progress"))
                    PlayerPrefs.SetInt("progress_ok", 1);
            }
            else
            {
                if (www.url.Contains("progress"))
                {
                    //Debug.Log("WWW Text: " + www.text);
                    LoadProgressFromJson(userId, www.text);
                    PlayerPrefs.SetInt("progress_ok", 1);
                }
                else if (www.url.Contains("top"))
                {
                  //  Debug.Log("WWW Text: " + www.text);
                    LoadTopFromJson(userId, www.text);
                    PlayerPrefs.SetInt("top_ok", 1);
                }
            }

            
        }

        void RequestProgress()
        {
            //http://trafficstorm.concretemixergames.com/webgl/progress.php

            
            WWW req = new WWW(URL + "/progress.php?user_id="+userId);
            StartCoroutine(WaitForRequest(req));          
        }

        public void RequestTop()
        {
            PlayerPrefs.SetInt("top_ok", 0);            
            WWW req = new WWW(URL + "/top.php?user_id=" + userId);
            StartCoroutine(WaitForRequest(req));
        }

        void LoadProgressFromJson(string userId, string json)
        {
            var progress = Newtonsoft.Json.JsonConvert.DeserializeObject<LevelScoreData>(json);

            for (int a = 0; a < 27; a++)
            {
                PlayerPrefs.SetInt("progress.2." + userId + "." + a.ToString(), 0);
                PlayerPrefs.SetInt("score.2." + userId + "." + a.ToString(), 0);
            }

            int crashes = 0;
            foreach (string key in progress.levels.Keys)
            {
                PlayerPrefs.SetInt("progress.2." + userId + "." + key, progress.levels[key].state);
                PlayerPrefs.SetInt("score.2." + userId + "." + key, progress.levels[key].score);
                crashes += progress.levels[key].attempts;
            }
            PlayerPrefs.SetInt("progress.attempts",crashes);
            PlayerPrefs.SetInt("tries.left", progress.tries);

            DateTime lastTry = DateTime.Now.AddSeconds(-progress.tries);
            Int32 unixTimestamp = (Int32)(lastTry.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            PlayerPrefs.SetInt("tries.last", unixTimestamp);
        }

        void LoadTopFromJson(string userId, string json)
        {
            var _top = Newtonsoft.Json.JsonConvert.DeserializeObject<TopScoresData>(json);
            top = _top.top;
        }

        IEnumerator GetText(string url)
        {
            Debug.Log("WWW: " + url);
            UnityWebRequest www = UnityWebRequest.Get(url);
            yield return www.Send();

            if (www.isError)
            {
                Debug.Log(www.error);
            }
            else
            {
               // Debug.Log(www.downloadHandler.text);
            }
        }

        public void UpdateState(int index, int state, int score)
        {
            StartCoroutine(GetText(URL + "/update.php?user_id=" + userId + "&level=" + index + "&score=" + score + "&state=" + state));
            
        }

        public void TryLevel(int index)
        {
            StartCoroutine(GetText(URL + "/try.php?user_id=" + userId + "&level=" + index));
            int crashes = PlayerPrefs.GetInt("progress.attempts", 0);
            PlayerPrefs.SetInt("progress.attempts", crashes+1);
            //Debug.Log("progress.attempts = " + crashes);
            if (crashes == 100)
                LogCrashAchievement();

            if (index != 0)
            {
                DateTime lastTry = DateTime.Now;
                Int32 unixTimestamp = (Int32)(lastTry.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                PlayerPrefs.SetInt("tries.last", unixTimestamp);
            }
        }

        private const string serviceKey = "55a4faec55a4faec55a4faec2f55f9d903555a455a4faec0cfaaf6f4d842328c9d6aefe";


        public void LogTutorialAchievement()
        {
            StartCoroutine(GetText(URL + "/achievement.php?user_id=" + userId + "&activity_id=3&value=0&access_token="+serviceKey));
        }

        public void LogCrashAchievement()
        {
            StartCoroutine(GetText(URL + "/achievement.php?user_id=" + userId + "&activity_id=4&value=0&access_token=" + serviceKey));
        }

        public void LogLevelAchievement(int count)
        {
            StartCoroutine(GetText(URL + "/achievement.php?user_id=" + userId + "&activity_id=1&value="+count+"&access_token=" + serviceKey));
        }

        public void ShowOrderBox()
        {
            //  StartCoroutine(GetText("https://api.vk.com/method/showOrderBox?type=item&votes=10&item=item1" + "&access_token=" + urlParams["access_token"]));
            Application.ExternalCall("order");
        }

        public void onOrderCancel()
        {
            Debug.Log("Order canceled");
        }

        Dictionary<string, string> ParseUrlParams()
        {
            string Application_absoluteURL = "http://trafficstorm.concretemixergames.com/webgl2?api_url=https://api.vk.com/api.php&api_id=6104047&api_settings=257&viewer_id=1515540&viewer_type=2&sid=f412b52e22946204d01eb9938a87f6f3d2c250dcb281938d51d59b0e1608f7e6deb6ac407408b1911eebf&secret=a6fe816344&access_token=35711d15cf4ccecf5d39b50fda2db717f56498b3bd48d2369cff877dbdb574a21472bd9264b059457f408&user_id=1515540&group_id=0&is_app_user=1&auth_key=68fdd07734e4843281f64caaeb88a98c&language=0&parent_language=0&is_secure=1&ads_app_id=6104047_f1aa53f3f187160325&referrer=menu&lc_name=1425d72e&hash=";
            if (Application.absoluteURL.Contains("concretemixergames"))
                Application_absoluteURL = Application.absoluteURL;

            Dictionary<string, string> res = new Dictionary<string, string>();
            if (!Application_absoluteURL.Contains("?"))
            {
                URL = Application_absoluteURL;
                return res;
            }
            string p = Application_absoluteURL.Split(new char[] { '?' }, StringSplitOptions.None)[1];

            URL = Application_absoluteURL.Split(new char[] { '?' }, StringSplitOptions.None)[0];

            foreach (var item in p.Split(new char[] { '&' }, StringSplitOptions.None))
            {
                string[] keyval = item.Split(new char[] { '=' }, StringSplitOptions.None);
                if (keyval.Length == 2)
                    res[keyval[0]] = keyval[1];
            }

            return res;
        }

    }
}



