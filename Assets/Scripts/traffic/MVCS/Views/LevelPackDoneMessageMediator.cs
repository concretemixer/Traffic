using Traffic.MVCS.Commands.Signals;
using Traffic.MVCS.Models;
using Traffic.Core;
using System;
using Commons.UI;
using UnityEngine;

using strange.extensions.mediation.impl;

namespace Traffic.MVCS.Views.UI
{
    public class LevelPackDoneMessageMediator : Mediator
    {
        [Inject]
        public PurshaseOk onPurchaseOk { get; set; }

        [Inject]
        public PurchaseFailed onPurchaseFailed { get; set; }

        [Inject]
        public LevelPackDoneMessageView view { get; set; }

        [Inject]
        public ILevelListModel levels { get; set; }

        [Inject]
        public IAPService iapService { get; set; }

        [Inject]
        public IUIManager UI { get; set; }

        [Inject]
        public ILocaleService localeService { get; set; }
        
        [Inject]
        public SwitchToMainScreenSignal toMainScreen { private get; set; }


        void closeHandler()
        {
            UI.Hide(UIMap.Id.LevelPackDoneMessage);
        }

        void homeHandler()
        {
            toMainScreen.Dispatch(); 
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                homeHandler();
            }
        }

        void infoOkHandler()
        {
            if (iapService.IsBought(IAPType.AdditionalLevels))
            {
                levels.TriesLeft = levels.TriesTotal;
                UI.Hide(UIMap.Id.LevelPackDoneMessage);
            }

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

        void buyHandler()
        {
            UI.Hide(UIMap.Id.InfoMessage);

            InfoMessageView view = UI.Show<InfoMessageView>(UIMap.Id.InfoMessage);
            view.SetMessageMode(false);

            iapService.PurchaseStart(IAPType.AdditionalLevels);
        }

        public override void OnRegister()
        {
            view.onButtonClose.AddListener(closeHandler);
            view.onButtonHome.AddListener(homeHandler);
            view.onButtonBuy.AddListener(buyHandler);

            onPurchaseOk.AddListener(purchaseOkHandler);
            onPurchaseFailed.AddListener(purchaseFailHandler);

            view.Layout(Screen.width, Screen.height);

            base.OnRegister();
        }

        public override void OnRemove()
        {
            view.onButtonClose.RemoveListener(closeHandler);
            view.onButtonHome.RemoveListener(homeHandler);
            view.onButtonBuy.RemoveListener(buyHandler);

            onPurchaseOk.RemoveListener(purchaseOkHandler);
            onPurchaseFailed.RemoveListener(purchaseFailHandler);

            base.OnRemove();
        }
    }
}