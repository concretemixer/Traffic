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
using System;
using System.Collections;


namespace Traffic.MVCS.Views.UI
{
    public class LevelListScreenMediator : Mediator
    {
        [Inject]
        public PurshaseOk onPurchaseOk { get; set; }

        [Inject]
        public PurchaseFailed onPurchaseFailed { get; set; }

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
        public ILocaleService localeService { get; set; }

        [Inject]
        public IAPService iapService { get; set; }

        [Inject]
        public StartLevelSignal startLevel { get; set; }

        [Inject]
        public InitLevelSignal initLevel { get; set; }

        [Inject]
        public SwitchToStartScreenSignal toStartScreenSignal { get; set; }

        static int page = 0;

        void switchPageHandlerNext()
        {
            page = page < 2 ? page+1 : page;
            view.SetPage(page, levels);
        }

		void switchPageHandlerPrev()
		{
			page = page > 0 ? page-1 : page;
			view.SetPage(page, levels);
		}


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

        void startLevelHandler(int index)
        {
            bool block = false;


            if (levels.TriesLeft <= 0)
            {
                if (DateTime.Now > levels.TriesRefreshTime)
                    levels.TriesLeft = levels.TriesTotal;
                else
                    block = true;
            }

            if (block)
                UI.Show(UIMap.Id.NoTriesMessage);
            else
            {
                levels.CurrentLevelIndex = index;
                startLevel.Dispatch(levels.CurrentLevelIndex);
                StartCoroutine("waitForLoad");                
            }
        }

        void homeHandler()
        {
            toStartScreenSignal.Dispatch();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                homeHandler();
            }
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

            InfoMessageView view2 = UI.Get<InfoMessageView>(UIMap.Id.InfoMessage);
            view2.onButtonOk.RemoveListener(infoOkHandler);
        }

        void purchaseOkHandler(IAPType what)
        {            
            InfoMessageView view = UI.Get<InfoMessageView>(UIMap.Id.InfoMessage);

            view.SetCaption(localeService.ProcessString("%PURCHASE_OK%"));
            if (what == IAPType.AdditionalLevels)
                view.SetText(localeService.ProcessString("%LEVELS_BOUGHT%"));
            else if (what == IAPType.NoAdverts)
                view.SetText(localeService.ProcessString("%NO_ADS_BOUGHT%"));        


            view.SetMessageMode(true);
            view.onButtonOk.AddListener(infoOkHandler);
        }

        void purchaseFailHandler(IAPType what, string error)
        {            
            InfoMessageView view = UI.Get<InfoMessageView>(UIMap.Id.InfoMessage);
            view.SetCaption(localeService.ProcessString("%PURCHASE_FAILED%"));
            view.SetText(error);
            view.SetMessageMode(true);
            view.onButtonOk.AddListener(infoOkHandler);
        }


        void buyLevelsHandler()
        {
            UI.Hide(UIMap.Id.InfoMessage);
            InfoMessageView view = UI.Show<InfoMessageView>(UIMap.Id.InfoMessage);
            view.SetMessageMode(false);

            iapService.PurchaseStart(IAPType.AdditionalLevels);
        }

        public override void OnRegister()
        {
            view.onButtonHome.AddListener(homeHandler);
            view.onButtonNext.AddListener(switchPageHandlerNext);
            view.onButtonPrev.AddListener(switchPageHandlerPrev);
            view.onButtonLevel.AddListener(startLevelHandler);
            view.onButtonClose.AddListener(closeHandler);
            view.onButtonBuy.AddListener(buyLevelsHandler);

            onPurchaseOk.AddListener(purchaseOkHandler);
            onPurchaseFailed.AddListener(purchaseFailHandler);

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
            view.onButtonNext.RemoveListener(switchPageHandlerNext);
            view.onButtonPrev.RemoveListener(switchPageHandlerPrev);
            view.onButtonLevel.RemoveListener(startLevelHandler);
            view.onButtonClose.RemoveListener(closeHandler);

            onPurchaseOk.RemoveListener(purchaseOkHandler);
            onPurchaseFailed.RemoveListener(purchaseFailHandler);
            base.OnRemove();
        }
    }
}