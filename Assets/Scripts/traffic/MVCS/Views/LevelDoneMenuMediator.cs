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
        public SwitchToMainScreenSignal toMainScreenSignal
        {
            get;
            set;
        }

        [Inject]
        public IUIManager UI
        {
            get;
            set;
        }

        [Inject]
        public FacebookSN facebook { private get; set; }

        [Inject]
        public AnalyticsCollector analytics { private get; set; }

        int acheivedStars;

        void nextLevelHandler()
        {
            if (levels.GetLevelState(levels.CurrentLevelIndex + 1) == LevelState.NoLevel)
                toMainScreenSignal.Dispatch();
            else
            {
                levels.CurrentLevelIndex = levels.CurrentLevelIndex + 1;
                startLevel.Dispatch(levels.CurrentLevelIndex);
            }
        }

        void homeHandler()
        {
            toMainScreenSignal.Dispatch();
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

            view.SetScore((int)level.Score, acheivedStars);
            view.Layout(Screen.width, Screen.height);
        }

        void shareHandler()
        {
            analytics.FacebookShareStart();

            var level = levels.CurrentLevelIndex + 1;
            var postData = new PostData()
            {
                Link = "https://www.facebook.com/trafficstorm/",
                LinkName = "I did it!!!",
                LinkCaption = string.Format("I achieved {0} level for {1} stars!", level, acheivedStars),
                LinkDescription = "Try to beat me at hardcoriest game Traffic Storm.",
                Picture = "https://scontent.xx.fbcdn.net/hphotos-xpa1/t31.0-8/12778873_462805340596055_5575106335908842951_o.png"
            };

            facebook.Post(postData).Done(
                analytics.FacebookShareComplete
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