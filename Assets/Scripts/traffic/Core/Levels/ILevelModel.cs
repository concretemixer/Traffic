using Traffic.MVCS.Commands.Signals;
using System.Collections.Generic;

namespace Traffic.Core
{



    public interface ILevelModel
    {
        float Score {
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


        bool Complete
        {
            get;
            set;
        }
    }
}