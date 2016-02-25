using Traffic.MVCS.Commands.Signals;
//using traffic.MVCS.model;
//using traffic.MVCS.model.level;
using Traffic.MVCS.Views.UI.HUD;
using Traffic.MVCS.Models;
using Traffic.Core;
using System;
using Commons.UI;
using Commons.Utils;

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

        public void Update()
        {
            if (levels.TriesLeft <= 0)
            {
                TimeSpan span = levels.TriesRefreshTime - DateTime.Now;
                view.SetTimerText(String.Format("TRIES WILL REFRESH AUTOMATICALY IN {0}:{1}", ((int)span.TotalMinutes).ToString("D2"), span.Seconds.ToString("D2")));
                if (DateTime.Now > levels.TriesRefreshTime)
                {
                    levels.TriesLeft = levels.TriesTotal;
                    view.SetLocked(false);
                }
            }
        }

        void closeHandler()
        {
            view.SetLocked(false);
        }

        void advertHandler()
        {
            levels.TriesLeft = levels.TriesTotal;
            view.SetLocked(false);
        }

        void retryLevelHandler()
        {
            if (levels.TriesLeft <= 0)
                view.SetLocked(true);
            else
                onRetry.Dispatch();
        }

        void homeHandler()
        {
            toMainScreenSignal.Dispatch();
        }

        void infoOkHandler()
        {
            if (iapService.IsBought(IAPType.NoAdverts))
            {
                levels.TriesLeft = levels.TriesTotal;
                view.SetLocked(false);
            }
        }


        void buyHandler()
        {
            UI.Hide(UIMap.Id.InfoMessage);

            if (iapService.Buy(IAPType.NoAdverts))
            {
                InfoMessageView view = UI.Show<InfoMessageView>(UIMap.Id.InfoMessage);
                view.SetCaption("PURCHASE OK");
                view.SetText("You have purchased the permanent advert removal for $2");
                view.onButtonOk.AddListener(infoOkHandler);
            }
            else
            {
                InfoMessageView view = UI.Show<InfoMessageView>(UIMap.Id.InfoMessage);
                view.SetCaption("PURCHASE FAILED");
                view.SetText("For some reason your purchase is failed");
                view.onButtonOk.AddListener(infoOkHandler);
            }
        }

        public override void OnRegister()
        {

            view.onButtonRetryLevel.AddListener(retryLevelHandler);
            view.onButtonHome.AddListener(homeHandler);
            view.onButtonClose.AddListener(closeHandler);
            view.onButtonAdvert.AddListener(advertHandler);
            view.onButtonBuy.AddListener(buyHandler);

            view.SetLocked(levels.TriesLeft <= 0);

            view.Layout();

            base.OnRegister();
        }




        public override void OnRemove()
        {
            view.onButtonRetryLevel.RemoveListener(retryLevelHandler);
            view.onButtonHome.RemoveListener(homeHandler);
            view.onButtonClose.RemoveListener(closeHandler);
            view.onButtonAdvert.RemoveListener(advertHandler);
            view.onButtonBuy.RemoveListener(buyHandler);
            base.OnRemove();
        }
    }
}