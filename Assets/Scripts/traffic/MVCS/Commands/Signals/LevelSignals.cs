//using Traffic.MVCS.model;
//using Traffic.MVCS.model.level;
using strange.extensions.signal.impl;
using UnityEngine;

namespace Traffic.MVCS.Commands.Signals
{
    public class VehicleReachedDestination : Signal
    {
    }

	public class VehicleCrashed : Signal
	{
	}

    public class LevelFailed : Signal
    {
    }

    public class LevelComplete : Signal
    {
    }

	public class LevelPause : Signal
	{
	}

	public class LevelResume : Signal
	{
	}

    public class LevelRetry : Signal
    {
    }

    public class ScoreGrow : Signal<float>
    {
    }

    public class TutorialPoint : Signal<int>
    {
    }

    public class ResumeTutorial : Signal
    {
    }

}