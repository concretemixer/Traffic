using Traffic.MVCS.Commands.Signals;
//using traffic.MVCS.model;
//using traffic.MVCS.model.level;
using Traffic.MVCS.Views.UI.HUD;
using Traffic.MVCS.Models;
using Traffic.Core;
using Traffic.Components;
using Commons.UI;
using Commons.Utils;
using UnityEngine;
using strange.extensions.mediation.impl;

namespace Traffic.MVCS.Views.UI
{
    public class LevelListScreenMediator : Mediator
    {



        [Inject]
        public LevelListScreenView view
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
       
        [Inject]
        public IUIManager UI {
            get;
            set;
        }

        [Inject]
        public IAPService iapService { get; set; }

        [Inject]
        public StartLevelSignal startLevel { get; set; }

        [Inject]
        public SwitchToStartScreenSignal toStartScreenSignal { get; set; }

        int page = 0;

        void switchPageHandler()
        {
            page = 1 - page;
            view.SetPage(page, levels);
            if (page==1)
                view.ShowLock(!iapService.IsBought(IAPType.AdditionalLevels));
        }

        void startLevelHandler(int index)
        {
            if (levels.TriesLeft <= 0)
                UI.Show(UIMap.Id.NoTriesMessage);
            else
            {
                levels.CurrentLevelIndex = index;
                startLevel.Dispatch(levels.CurrentLevelIndex);
            }
        }

        void homeHandler()
        {
            toStartScreenSignal.Dispatch();
        }

        void closeHandler()
        {
            page = 0;
            view.SetPage(page, levels);
            view.ShowLock(false);            
        }

        void infoOkHandler()
        {
            if (iapService.IsBought(IAPType.AdditionalLevels))
                view.ShowLock(false);
        }

        void buyLevelsHandler()
        {
            UI.Hide(UIMap.Id.InfoMessage);
            if (iapService.Buy(IAPType.AdditionalLevels))
            {
                InfoMessageView view = UI.Show<InfoMessageView>(UIMap.Id.InfoMessage);
                view.SetCaption("PURCHASE OK");
                view.SetText("You have purchased 12 additional levels for $1");
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
            view.onButtonHome.AddListener(homeHandler);
            view.onButtonNext.AddListener(switchPageHandler);
            view.onButtonPrev.AddListener(switchPageHandler);
            view.onButtonLevel.AddListener(startLevelHandler);
            view.onButtonClose.AddListener(closeHandler);
            view.onButtonBuy.AddListener(buyLevelsHandler);

            view.Layout(Screen.width, Screen.height);
            view.SetPage(page, levels);
            view.ShowLock(false);            
            view.SetDebugMessage(EntryPoint.DebugMessage);

            base.OnRegister();
        }




        public override void OnRemove()
        {
            view.onButtonBuy.RemoveListener(buyLevelsHandler);
            view.onButtonHome.RemoveListener(homeHandler);
            view.onButtonNext.RemoveListener(switchPageHandler);
            view.onButtonPrev.RemoveListener(switchPageHandler);
            view.onButtonLevel.RemoveListener(startLevelHandler);
            view.onButtonClose.RemoveListener(closeHandler);

            base.OnRemove();
        }
    }
}