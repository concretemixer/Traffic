using strange.extensions.signal.impl;

namespace Traffic.MVCS.Commands.Signals
{
    public class StartupSignal : Signal
    {
    }

    public class StartLevelSignal : Signal<int>
    {
    }

    public class SwitchToMainScreenSignal : Signal
    {
    }

    public class SwitchToStartScreenSignal : Signal
    {
    }

    public class SwitchToSettingsScreenSignal : Signal
    {
    }

    public class OrientationChangedSignal : Signal
    {
    }

    public class ShowAdsSignal : Signal
    {
    }

    public class AddLivesForAdsSignal : Signal
    {
    }
}