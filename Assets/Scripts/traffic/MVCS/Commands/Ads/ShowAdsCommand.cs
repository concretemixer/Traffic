using Commons.Utils;
using strange.extensions.command.impl;
using Traffic.MVCS.Commands.Signals;
using Traffic.MVCS.Services;
using UnityEngine.Advertisements;

namespace Traffic.MVCS.Commands.Ads
{
    public class ShowAdsCommand : Command
    {
        [Inject]
        public AddLivesForAdsSignal addLives { private get; set; }

        [Inject]
        public AnalyticsCollector analitycs { private get; set; }

        public override void Execute()
        {
            Retain();

            ShowOptions options = new ShowOptions();
            options.resultCallback = showResultHandler;

            Advertisement.Show(null, options);
            analitycs.AdsStart();
        }

        void showResultHandler(ShowResult result)
        {
            switch (result)
            {
                case ShowResult.Finished:
                    analitycs.AdsComplete();
                    Loggr.Log("Video completed. User rewarded credits.");
                    break;
                case ShowResult.Skipped:
                    Loggr.Log("Video was skipped.");
                    analitycs.AdsSkiped();
                    break;
                case ShowResult.Failed:
                    addLives.Dispatch();
                    analitycs.AdsFailed();
                    Loggr.Log("Video failed to show.");
                    break;
            }
            addLives.Dispatch();
            Release();
        }
    }
}