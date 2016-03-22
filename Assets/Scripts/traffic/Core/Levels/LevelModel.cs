using Commons.Utils;

using Traffic.MVCS.Commands.Signals;

namespace Traffic.Core
{
    public class LevelModel : ILevelModel
    {
        public LevelConfig Config
        {
            get;
            set;
        }

		public float Score {
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
        
        public int LevelIndex
        {
            get; 
            set;
        }

    }
}