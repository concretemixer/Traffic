using Commons.Utils;
using strange.extensions.command.impl;
using Traffic.MVCS.Commands.Signals;
using Traffic.MVCS.Services;
using AppodealAds.Unity.Api;
using AppodealAds.Unity.Common;
using UnityEngine;


namespace Traffic.MVCS.Commands.Ads
{
        public class ShowAppodealAdsCommand : Command, ISkippableVideoAdListener
        {
            [Inject]
            public AddLivesForAdsSignal addLives { private get; set; }

            [Inject]
            public AnalyticsCollector analitycs { private get; set; }


            public override void Execute()
            {
                Debug.Log("HERE");
                                          
#if (UNITY_IOS || UNITY_ANDROID)
                Appodeal.setSkippableVideoCallbacks(this);

                analitycs.AdsStart();
                Appodeal.show(Appodeal.NON_SKIPPABLE_VIDEO);
#endif
                addLives.Dispatch();

              // 
            }

#region Video callback handlers
            public void onSkippableVideoLoaded() {  }
            public void onSkippableVideoFailedToLoad() { analitycs.AdsFailed(); }
            public void onSkippableVideoShown() {  }
            public void onSkippableVideoFinished() { analitycs.AdsComplete(); }
            public void onSkippableVideoClosed() { analitycs.AdsSkiped(); }
#endregion

        }
    }