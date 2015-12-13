using Traffic.MVCS.Commands.Signals;

namespace Traffic.Core
{
    public interface ILevelModel
    {
        int Score {
            get;
            set;
        }
		
		int Progress {
			get;
			set;
		}

		int Target {
			get;
			set;
		}

        bool Failed
        {
            get;
            set;
        }
    }
}