using Traffic.MVCS.Commands.Signals;
using Traffic.MVCS.Models;
using Traffic.Core;
using System;
using Commons.UI;
using UnityEngine;

using strange.extensions.mediation.impl;
using Traffic.MVCS.Services;
using Traffic.Components;

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

        [Inject]
        public ILocaleService localeService { get; set; }

        [Inject(EntryPoint.Container.Stage)]
        public GameObject stage { get; set; }

        //   [Inject]
        //   public ShowAdsSignal showAds { private get; set; }

        [Inject]
        public AnalyticsCollector analitics { private get; set; }

        public void Update()
        {
            if (levels.TriesLeft <= 0)
            {
                TimeSpan span = levels.TriesRefreshTime - DateTime.Now;
                view.SetTimerText(
                    String.Format(localeService.ProcessString("%NO_TRIES_TIMER%"), ((int)span.TotalMinutes).ToString("D2"), span.Seconds.ToString("D2")));
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
            UI.Hide(UIMap.Id.NoTriesMessage);
            //showAds.Dispatch();
        }

        void infoOkHandler()
        {
            if (iapService.IsBought(IAPType.NoAdverts))
            {
                levels.TriesLeft = levels.TriesTotal;
                UI.Hide(UIMap.Id.NoTriesMessage);
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

            /*
            InfoMessageView view = UI.Show<InfoMessageView>(UIMap.Id.InfoMessage);
            view.SetMessageMode(false);

            iapService.PurchaseStart(IAPType.NoAdverts);*/

            WebDB webDB = stage.GetComponentInParent<WebDB>();
            if (webDB != null)
            {
                webDB.ShowOrderBox();
            }
        }

        public override void OnRegister()
        {
            analitics.NoTriesWindowShown();
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