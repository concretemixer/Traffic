using Traffic.MVCS.Commands.Signals;
//using traffic.MVCS.model;
//using traffic.MVCS.model.level;
using Traffic.MVCS.Views.UI.HUD;
using Traffic.MVCS.Models;
using Traffic.Core;
using System;
using Commons.UI;
using Commons.Utils;
using UnityEngine;

using strange.extensions.mediation.impl;

namespace Traffic.MVCS.Views.UI
{
    public class LevelFailedMenuMediator : Mediator
    {


        [Inject]
        public LevelFailedMenuView view
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
        public ILevelListModel levels { get; set; }

        [Inject]
        public IAPService iapService { get; set; }

        [Inject]
        public ILocaleService localeService { get; set; }

        [Inject]
        public LevelRetry onRetry { get; set; }


                  
        [Inject]
        public SwitchToMainScreenSignal toMainScreenSignal
        {
            get;
            set;
        }

        [Inject]
        public IUIManager UI {
            get;
            set;
        }

      
        void retryLevelHandler()
        {
            if (levels.TriesLeft <= 0)
                UI.Show(UIMap.Id.NoTriesMessage);
            else
                onRetry.Dispatch();
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

            view.onButtonRetryLevel.AddListener(retryLevelHandler);
            view.onButtonHome.AddListener(homeHandler);

            if (levels.TriesLeft <= 0)
                UI.Show(UIMap.Id.NoTriesMessage);

            view.SetScore((int)level.Score);

            float k = (float)level.Progress / (float)levels.LevelConfigs[levels.CurrentLevelIndex].target;

            view.SetMessage(k < 0.3 ? localeService.ProcessString("%LEVEL_LOST_1%") :
                (k > 0.8 ? localeService.ProcessString("%LEVEL_LOST_3%") : localeService.ProcessString("%LEVEL_LOST_2%")));

            view.Layout(Screen.width, Screen.height);

            base.OnRegister();
        }




        public override void OnRemove()
        {
            view.onButtonRetryLevel.RemoveListener(retryLevelHandler);
            view.onButtonHome.RemoveListener(homeHandler);
            base.OnRemove();
        }
    }
}