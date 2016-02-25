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

        public int TriesLeft
        {
            get;
            set;
        }

        public int TriesTotal
        {
            get;
            set;
        }


        public DateTime TriesRefreshTime
        {
            get;
            set;
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

            TriesLeft = 1;
            TriesTotal = 10;

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
		        "Level18",
		        "Level19",
	        };

        }

    }
}