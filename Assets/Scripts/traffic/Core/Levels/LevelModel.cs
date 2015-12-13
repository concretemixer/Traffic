using Commons.Utils;

using Traffic.MVCS.Commands.Signals;

namespace Traffic.Core
{
    public class LevelModel : ILevelModel
    {

		public int Score {
			get;
			set;
		}

        public int Target {
            get;
            set;
        }

		public int Progress {
			get;
			set;
		}

        public bool Failed
        {
            get;
            set;
        }

    }
}