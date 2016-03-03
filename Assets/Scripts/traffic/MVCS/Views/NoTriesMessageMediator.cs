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
    public class NoTriesMessageMediator : Mediator
    {
        [Inject]
        public PurshaseOk onPurchaseOk { get; set; }

        [Inject]
        public PurchaseFailed onPurchaseFailed { get; set; }

        [Inject]
        public NoTriesMessageView view { get; set; }
       
        [Inject]
        public ILevelListModel levels { get; set; }

        [Inject]
        public IAPService iapService { get; set; }

        [Inject]
        public IUIManager UI { get; set; }

        public void Update()
        {
            if (levels.TriesLeft <= 0)
            {
                TimeSpan span = levels.TriesRefreshTime - DateTime.Now;
                view.SetTimerText(String.Format("TRIES WILL REFRESH AUTOMATICALY IN {0}:{1}", ((int)span.TotalMinutes).ToString("D2"), span.Seconds.ToString("D2")));
                if (DateTime.Now > levels.TriesRefreshTime)
                {
                    levels.TriesLeft = levels.TriesTotal;
                    UI.Hide(UIMap.Id.NoTriesMessage);
                }
            }
        }

        void closeHandler()
        {
            UI.Hide(UIMap.Id.NoTriesMessage);            
        }

        void advertHandler()
        {
            levels.TriesLeft = levels.TriesTotal;
            UI.Hide(UIMap.Id.NoTriesMessage);
        }

        void infoOkHandler()
        {
            if (iapService.IsBought(IAPType.NoAdverts))
            {
                levels.TriesLeft = levels.TriesTotal;
                UI.Hide(UIMap.Id.NoTriesMessage);
            }
        }


        void purchaseOkHandler(IAPType what)
        {
            InfoMessageView view = UI.Get<InfoMessageView>(UIMap.Id.InfoMessage);

            view.SetCaption("PURCHASE OK");
            if (what == IAPType.AdditionalLevels)
                view.SetText("You have purchased 12 additional levels for $1");
            else if (what == IAPType.NoAdverts)
                view.SetText("You have purchased the permanent advert removal for $2");
            else
                view.SetText("You have purchased something...");

            view.SetMessageMode(true);
            view.onButtonOk.AddListener(infoOkHandler);
        }

        void purchaseFailHandler(IAPType what)
        {
            InfoMessageView view = UI.Get<InfoMessageView>(UIMap.Id.InfoMessage);
            view.SetCaption("PURCHASE FAILED");
            view.SetText("For some reason your purchase is failed");
            view.SetMessageMode(true);
            view.onButtonOk.AddListener(infoOkHandler);
        }

        void buyHandler()
        {
            UI.Hide(UIMap.Id.InfoMessage);

            InfoMessageView view = UI.Show<InfoMessageView>(UIMap.Id.InfoMessage);
            view.SetMessageMode(false);

            iapService.PurchaseStart(IAPType.NoAdverts);
        }

        public override void OnRegister()
        {

            view.onButtonClose.AddListener(closeHandler);
            view.onButtonAdvert.AddListener(advertHandler);
            view.onButtonBuy.AddListener(buyHandler);

            onPurchaseOk.AddListener(purchaseOkHandler);
            onPurchaseFailed.AddListener(purchaseFailHandler);


            view.Layout(Screen.width, Screen.height);

            base.OnRegister();
        }




        public override void OnRemove()
        {
            view.onButtonClose.RemoveListener(closeHandler);
            view.onButtonAdvert.RemoveListener(advertHandler);
            view.onButtonBuy.RemoveListener(buyHandler);

            onPurchaseOk.RemoveListener(purchaseOkHandler);
            onPurchaseFailed.RemoveListener(purchaseFailHandler);

            base.OnRemove();
        }
    }
}