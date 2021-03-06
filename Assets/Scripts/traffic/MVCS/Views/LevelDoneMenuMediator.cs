using Traffic.MVCS.Commands.Signals;
using Traffic.MVCS.Models;
using Traffic.Core;
using Commons.UI;
using Commons.Utils;
using UnityEngine;

using strange.extensions.mediation.impl;
using Commons.SN.Facebook;
using Commons.SN;
using Traffic.MVCS.Services;
using Traffic.Components;
using System.Collections;

namespace Traffic.MVCS.Views.UI
{
    public class LevelDoneMenuMediator : Mediator
    {
        [Inject]
        public LevelDoneMenuView view
        {
            get;
            set;
        }

        [Inject]
        public ILevelListModel levels
        {
            get;
            set;
        }

        [Inject(EntryPoint.Container.UI)]
        public GameObject UI { get; set; }

        [Inject(GameState.Current)]
        public ILevelModel level
        {
            get;
            set;
        }

        [Inject]
        public LevelRetry onResume { get; set; }

        [Inject]
        public StartLevelSignal startLevel
        {
            get;
            set;
        }

        [Inject]
        public InitLevelSignal initLevel { get; set; }

        [Inject]
        public SwitchToMainScreenSignal toMainScreenSignal
        {
            get;
            set;
        }

        [Inject]
        public IUIManager uiManager
        {
            get;
            set;
        }

        [Inject]
        public IAPService iapService { get; set; }

        [Inject]
        public FacebookSN facebook { private get; set; }

        [Inject]
        public AnalyticsCollector analytics { private get; set; }

        [Inject]
        public ILocaleService localeService { get; set; }

        IEnumerator waitForLoad()
        {
            while (true)
            {
                if (GameObject.FindGameObjectWithTag("Root") == null)
                    yield return null;
                break;
            }

            initLevel.Dispatch();
        }

        int acheivedStars;

        void nextLevelHandler()
        {
            if (levels.LevelsLeft==0) {
                 uiManager.Show(UIMap.Id.GameCompleteMessage);            
            }
            else if (levels.CurrentLevelIndex % 9 == 8)
            {
                uiManager.Show(UIMap.Id.LevelPackDoneMessage);
            }
            else
            {
                levels.CurrentLevelIndex = levels.CurrentLevelIndex + 1;
                startLevel.Dispatch(levels.CurrentLevelIndex);
                StartCoroutine("waitForLoad");
            }
        }

        void homeHandler()
        {
            toMainScreenSignal.Dispatch();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                homeHandler();
            }
        }

        public override void OnRegister()
        {
            acheivedStars = 1;

            if (level.Score >= levels.LevelConfigs[levels.CurrentLevelIndex].threeStarsScore)
                acheivedStars = 3;
            else if (level.Score >= levels.LevelConfigs[levels.CurrentLevelIndex].twoStarsScore)
                acheivedStars = 2;

            view.onButtonNextLevel.AddListener(nextLevelHandler);
            view.onButtonHome.AddListener(homeHandler);
            view.onShareButton.AddListener(shareHandler);

            view.SetMessage(acheivedStars == 1 ? localeService.ProcessString("%LEVEL_WON_1%") :
                (acheivedStars == 3 ? localeService.ProcessString("%LEVEL_WON_3%") : localeService.ProcessString("%LEVEL_WON_2%")));

            view.SetScore((int)level.Score, acheivedStars);

            if (levels.CurrentLevelIndex == 8)
                view.ShowBadge(true);
            if (levels.CurrentLevelIndex == 17)
                view.ShowBadge(true);
            if (levels.CurrentLevelIndex == 26)
                view.ShowBadge(true);

            view.Layout(Screen.width, Screen.height);

            this.Invoke("PlaySuccessSound", 0.5f);
           
        }

        void PlaySuccessSound()
        {
            float soundVolume = PlayerPrefs.GetFloat("volume.sound", 1);
            UI.GetComponent<AudioSource>().PlayOneShot(view.successSound, soundVolume);
        }

        void shareHandler()
        {
            Loggr.Log("Click to Facebook share.");
            analytics.FacebookShareStart();

            var level = levels.CurrentLevelIndex + 1;
            var postData = new PostData()
            {
                Link = "https://www.facebook.com/trafficstorm/",
                LinkName = "I did it!!!",
                LinkCaption = string.Format("I have completed level {0} and earned {1} stars!", level, acheivedStars),
                LinkDescription = "Try to beat me at hardcoriest game Traffic Storm.",
                Picture = "https://scontent.xx.fbcdn.net/hphotos-xpa1/t31.0-8/12778873_462805340596055_5575106335908842951_o.png"
            };

            facebook.Post(postData).Done(
                () => analytics.FacebookShareComplete()
            );
        }

        public override void OnRemove()
        {
            view.onButtonNextLevel.RemoveListener(nextLevelHandler);
            view.onButtonHome.RemoveListener(homeHandler);
            view.onShareButton.RemoveListener(shareHandler);

            base.OnRemove();
        }
    }
}