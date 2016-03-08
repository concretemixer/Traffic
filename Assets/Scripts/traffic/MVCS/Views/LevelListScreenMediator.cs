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
            if (page == 1)
            {
                float price;
                string currency;
                if (iapService.GetProductPrice(IAPType.AdditionalLevels, out price, out currency))
                {
                    view.shopLevelsDesc.text = view.shopLevelsDesc.text.Replace("%PRICE%",
                        currency + (currency.Length > 1 ? " " : "") + price.ToString("F2"));
                }
                view.ShowLock(!iapService.IsBought(IAPType.AdditionalLevels));
            }
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

            InfoMessageView view2 = UI.Get<InfoMessageView>(UIMap.Id.InfoMessage);
            view2.onButtonOk.RemoveListener(infoOkHandler);
        }

        void purchaseOkHandler(IAPType what)
        {            
            InfoMessageView view = UI.Get<InfoMessageView>(UIMap.Id.InfoMessage);
            view.SetCaption("PURCHASE OK");

            float price;
            string currency = "?";

            iapService.GetProductPrice(what, out price, out currency);

            view.SetCaption("PURCHASE OK");
            if (what == IAPType.AdditionalLevels)
            {
                view.SetText("You have purchased 12 additional levels for " + currency + (currency.Length > 1 ? " " : "") + price.ToString("F2"));
            }
            else if (what == IAPType.NoAdverts)
            {
                view.SetText("You have purchased the permanent advert removal for " + currency + (currency.Length > 1 ? " " : "") + price.ToString("F2"));
            }
            else
                view.SetText("You have purchased something...");            


            view.SetMessageMode(true);
            view.onButtonOk.AddListener(infoOkHandler);
        }

        void purchaseFailHandler(IAPType what, string error)
        {            
            InfoMessageView view = UI.Get<InfoMessageView>(UIMap.Id.InfoMessage);
            view.SetCaption("PURCHASE FAILED");
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
            view.onButtonNext.AddListener(switchPageHandler);
            view.onButtonPrev.AddListener(switchPageHandler);
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
            view.onButtonNext.RemoveListener(switchPageHandler);
            view.onButtonPrev.RemoveListener(switchPageHandler);
            view.onButtonLevel.RemoveListener(startLevelHandler);
            view.onButtonClose.RemoveListener(closeHandler);

            base.OnRemove();
        }
    }
}