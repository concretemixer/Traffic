using Traffic.MVCS.Commands.Signals;
using System.Collections.Generic;

namespace Traffic.Core
{



    public interface ILevelModel
    {
        LevelConfig Config
        {
            get;
            set;
        }

        float Score {
            get;
            set;
        }
		
		int Progress {
			get;
			set;
		}

        bool Failed
        {
            get;
            set;
        }


        bool Complete
        {
            get;
            set;
        }
    }
}