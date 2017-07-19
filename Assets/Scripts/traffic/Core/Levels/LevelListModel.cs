using Traffic.MVCS.Commands.Signals;
using UnityEngine;
using System;
using Traffic.Components;


namespace Traffic.Core
{
    public class LevelListModel : ILevelListModel
    {
        public int CurrentLevelIndex {
             get;
             set;
        }

        public string[] LevelNames
        {
             get;
             set;
		}

        public LevelConfig[] LevelConfigs
        {
            get;
            set;
        }

        [Inject(EntryPoint.Container.Stage)]
        public GameObject stage { get; set; }

        int _TriesLeft;
        public int TriesLeft
        {
            get {
#if UNITY_WEBGL
                _TriesLeft = PlayerPrefs.GetInt("tries.left", 30);
#endif
                return _TriesLeft;
            }
            set {
                _TriesLeft = value;
                PlayerPrefs.SetInt("tries.left", _TriesLeft);
            }
        }

        public int TriesTotal
        {
            get;
            set;
        }

        DateTime _TriesRefreshTime;
        public DateTime TriesRefreshTime
        {
            get { return _TriesRefreshTime; }
            set
            {
                _TriesRefreshTime = value;
                Int32 unixTimestamp = (Int32)(value.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                PlayerPrefs.SetInt("tries.refresh", unixTimestamp);
            }
        }

        public int LevelsLeft
        {
            get {
                int left = LevelNames.Length;
                for (int a = 0; a < LevelNames.Length; a++)
                {
                    LevelState s = GetLevelState(a);
                    if (s == LevelState.PassedOneStar || s == LevelState.PassedTwoStars|| s == LevelState.PassedThreeStars)
                        left--;
                }
                return left;
            }            
        }

        public LevelState GetLevelState(int index)
        {
            if (index < 0)
                return LevelState.NoLevel;
            if (index >= LevelNames.Length)
                return LevelState.NoLevel;

            string user_id = PlayerPrefs.GetString("user_id","0");
            LevelState result = (LevelState)PlayerPrefs.GetInt("progress.2." + user_id + "." + index.ToString(), 0);
            if (result == LevelState.Locked) {
                if (index % 9 == 0)
                    return LevelState.Playable;
                LevelState prev = (LevelState)PlayerPrefs.GetInt("progress.2." + user_id + "." + (index-1).ToString(), 0);
                if (prev==LevelState.PassedOneStar || prev == LevelState.PassedTwoStars|| prev == LevelState.PassedThreeStars)
                    return LevelState.Playable;
            }

            return result;
        }

        public void SetLevelState(int index, LevelState state, int score)
        {
            string user_id = PlayerPrefs.GetString("user_id","0");
            PlayerPrefs.SetInt("progress.2." + user_id +"."+ index.ToString(), (int)state);
            PlayerPrefs.SetInt("score.2." + user_id + "." + index.ToString(), score);
            WebDB webDB = stage.GetComponentInParent<WebDB>();
            if (webDB != null)
            {
                webDB.UpdateState(index, (int)state, score);
                if (index == 0 && (state == LevelState.PassedOneStar || state == LevelState.PassedThreeStars || state == LevelState.PassedTwoStars))
                    webDB.LogTutorialAchievement();
                int levels = GetPassedLevelCount();
                if (levels>1)
                    webDB.LogLevelAchievement(levels);
            }
        }

        public int GetLevelScore(int index)
        {
            string user_id = PlayerPrefs.GetString("user_id","0");
            return PlayerPrefs.GetInt("score.2." + user_id + "." + index.ToString(), 0);
        }

        private int GetPassedLevelCount()
        {
            string user_id = PlayerPrefs.GetString("user_id", "0");
            int result = 0;
            for (int a = 0; a < 27; a++)
            {
                LevelState state = (LevelState)PlayerPrefs.GetInt("progress.2." + user_id + "." + a, 0);
                if (state == LevelState.PassedOneStar || state == LevelState.PassedThreeStars || state == LevelState.PassedTwoStars)
                    result++;
            }
            return result;
        }

        /*
        public void SetLevelScore(int index, int score)
        {
            string user_id = PlayerPrefs.GetString("user_id","0");
            PlayerPrefs.SetInt("score.2." + user_id + "." + index.ToString(), score);
        }
          */
        public LevelListModel()
        {
            CurrentLevelIndex = 0;

            _TriesLeft = PlayerPrefs.GetInt("tries.left", 0);
            TriesTotal = 7;

            Int32 unixTimestamp = PlayerPrefs.GetInt("tries.refresh", 0);
            _TriesRefreshTime = new DateTime(1970, 1, 1).AddSeconds(unixTimestamp);

            /*
            if (TriesLeft <= 0)
            {
                if (DateTime.Now > TriesRefreshTime)
                    TriesLeft = TriesTotal;
            }
              */

            LevelNames = new string[] 
	        {
				              "LevelTutorial",
					        "Level1",
					        "Level2",
					        "Level3_1", 		        
							"Level4_1",
					        "Level13_1",
							"Level5",
					        "Level10",
					        "Level7",

				"Level4",
				"Level6",
				"Level22",
				"Level11",
				"Level18_1",
				"Level23",

				"Level16",
				"Level17",
				"Level19",


				 "Level3",
				"Level9",
				"Level14",
				"Level21",
				"Level18",
				"Level12",
				"Level13",
				"Level15",
				"Level8",

                "Level10_Night",

            };

        }

    }
}