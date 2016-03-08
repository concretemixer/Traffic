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

#if UNITY_STANDALONE
        public override void Execute()
        {
            addLives.Dispatch();
        }
#else
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
                    Loggr.Log("Video completed. User rewarded credits.");
                    addLives.Dispatch();
                    analitycs.AdsComplete();
                    break;
                case ShowResult.Skipped:
                    Loggr.Log("Video was skipped.");
                    addLives.Dispatch();
                    analitycs.AdsSkiped();
                    break;
                case ShowResult.Failed:
                    Loggr.Log("Video failed to show.");
                    addLives.Dispatch();
                    analitycs.AdsFailed();
                    break;
            }
            
            Release();
        }
#endif
    }
}