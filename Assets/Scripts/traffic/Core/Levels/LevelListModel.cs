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

        public LevelState GetLevelState(int index)
        {
            if (index < 0)
                return LevelState.NoLevel;
            if (index >= LevelNames.Length)
                return LevelState.NoLevel;

            LevelState result = (LevelState)PlayerPrefs.GetInt("progress.1." + index.ToString(), 0);
            if (result == LevelState.Locked && index == 0)
                result = LevelState.Playable;

            return result;
        }

        public void SetLevelState(int index, LevelState state)
        {
            PlayerPrefs.SetInt("progress.1." + index.ToString(), (int)state);
        }


        public LevelListModel()
        {
            CurrentLevelIndex = 0;

            _TriesLeft = PlayerPrefs.GetInt("tries.left", 10);
            TriesTotal = 10;

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
		        "Level4_1",

		        "Level3_1",
		        "Level4",
		        "Level10",
		        "Level6",

		        "Level5",
		        "Level3",
		        "Level18_1",
		        "Level8",

		        "Level7",
		        "Level21",
		        "Level13_1",
		        "Level9",

		        "Level11",
		        "Level12",
		        "Level14",
		        "Level15",

		        "Level13",	
		        "Level16",
		        "Level17",
		        "Level18"

		        //"Level19",
	        };

        }

    }
}