using Traffic.MVCS.Commands.Signals;

namespace Traffic.Core
{
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

        LevelState GetLevelState(int index);
        void SetLevelState(int index, LevelState state);
    }
}