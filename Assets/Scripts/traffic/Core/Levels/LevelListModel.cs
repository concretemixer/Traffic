using Traffic.MVCS.Commands.Signals;
using UnityEngine;
using System;

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

        int _TriesLeft;
        public int TriesLeft
        {
            get {
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

            LevelState result = (LevelState)PlayerPrefs.GetInt("progress.2." + index.ToString(), 0);
            if (result == LevelState.Locked && index % 9 == 0)
                result = LevelState.Playable;

            return result;
        }

        public void SetLevelState(int index, LevelState state)
        {
            PlayerPrefs.SetInt("progress.2." + index.ToString(), (int)state);
        }


        public LevelListModel()
        {
            CurrentLevelIndex = 0;

            _TriesLeft = PlayerPrefs.GetInt("tries.left", 7);
            TriesTotal = 7;

            Int32 unixTimestamp = PlayerPrefs.GetInt("tries.refresh", 0);
            _TriesRefreshTime = new DateTime(1970, 1, 1).AddSeconds(unixTimestamp);

            if (TriesLeft <= 0)
            {
                if (DateTime.Now > TriesRefreshTime)
                    TriesLeft = TriesTotal;
            }

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

	        };

        }

    }
}