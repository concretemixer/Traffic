using Traffic.MVCS.Commands.Signals;
using System.Collections.Generic;

namespace Traffic.Core
{

    public class PitcherConfig
    {
        public float startDelay;
        public float intervalMin;
        public float intervalMax;
    }

    public class LevelConfig
    {        
        public int threeStarsScore;
        public int twoStarsScore;
        public int target;
        public Dictionary<string, PitcherConfig> pitchers = new Dictionary<string, PitcherConfig>();
    }

    public class GameplayConfig
    {
        public int version;
        public Dictionary<string, LevelConfig> levels = new Dictionary<string, LevelConfig>();
    }
   
    public enum LevelState
    {
        Locked = 0,
        PayLocked = 1,
        Playable = 2,
        PassedOneStar = 3,
        PassedTwoStars = 4,
        PassedThreeStars = 5,
        NoLevel = 100
    }

    public interface ILevelListModel
    {
        int CurrentLevelIndex {
            get;
            set;
        }
		
		string[] LevelNames {
			get;
			set;
		}

        LevelConfig[] LevelConfigs
        {
            get;
            set;
        }

        LevelState GetLevelState(int index);
        void SetLevelState(int index, LevelState state);
    }
}