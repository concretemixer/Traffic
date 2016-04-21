using Commons.Utils;
using strange.extensions.command.impl;
using Traffic.MVCS.Commands.Signals;
using Traffic.MVCS.Services;
using AppodealAds.Unity.Api;
using AppodealAds.Unity.Common;
using UnityEngine;

namespace Traffic.MVCS.Commands.Ads
{
    public class ShowAppodealAdsCommand : Command, INonSkippableVideoAdListener
    {
        [Inject]
        public AddLivesForAdsSignal addLives { private get; set; }

        [Inject]
        public AnalyticsCollector analitycs { private get; set; }


        public override void Execute()
        {
            Debug.Log("HERE");

            Appodeal.setNonSkippableVideoCallbacks(this);

            analitycs.AdsStart();
            Appodeal.show(Appodeal.NON_SKIPPABLE_VIDEO);
            addLives.Dispatch();

          // 
        }

        #region NonSkippable Video callback handlers
        public void onNonSkippableVideoLoaded() 
        { 
            Debug.Log("Video loaded"); 
        }

        public void onNonSkippableVideoFailedToLoad() 
        {
            analitycs.AdsFailed();
            Debug.Log("Video failed"); 
        }

        public void onNonSkippableVideoShown() 
        { 
            Debug.Log("Video shown"); 
        }

        public void onNonSkippableVideoClosed() 
        {

            Debug.Log("Video closed"); 
        }

        public void onNonSkippableVideoFinished() 
        {
            analitycs.AdsComplete();
            Debug.Log("onNonSkippableVideoFinished"); 
        }
        #endregion
    }
}