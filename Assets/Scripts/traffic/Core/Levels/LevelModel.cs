using Commons.Utils;

using Traffic.MVCS.Commands.Signals;

namespace Traffic.Core
{
    public class LevelModel : ILevelModel
    {

		public float Score {
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

        public bool Complete
        {
            get;
            set;
        }

    }
}