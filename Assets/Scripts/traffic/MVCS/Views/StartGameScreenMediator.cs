using Traffic.MVCS.Commands.Signals;
using Traffic.MVCS.Models;
using Commons.UI;
using UnityEngine;

using strange.extensions.mediation.impl;
using Commons.SN.Facebook;
using Traffic.MVCS.Services;

namespace Traffic.MVCS.Views.UI
{
    public class StartGameScreenMediator : Mediator
    {
        [Inject]
        public PurshaseOk onPurchaseOk { get; set; }

        [Inject]
        public PurchaseFailed onPurchaseFailed { get; set; }

        [Inject]
        public StartGameScreenView view { get; set; }

        [Inject]
        public IAPService iapService { get; set; }

        [Inject]
        public SwitchToMainScreenSignal toMainScreenSignal { get; set; }

        [Inject]
        public SwitchToSettingsScreenSignal toSettingsSignal { get; set; }

        [Inject]
        public FacebookSN facebook { private get; set; }
        
        public AnalyticsCollector analytics { private get; set; }

        [Inject]
        public IUIManager UI { get; set; }
        
        void homeHandler()
        {
            toMainScreenSignal.Dispatch();
        }

        void optionsHandler()
        {
            toSettingsSignal.Dispatch();
        }

        void shopHandler()
        {
            view.ShowShop(true, iapService);
        }

        void closeShopHandler()
        {
            view.ShowShop(false, iapService);
        }

        void connectHandler()
        {
        }

        void infoOkHandler()
        {
            view.ShowShop(true, iapService);

            InfoMessageView view2 = UI.Get<InfoMessageView>(UIMap.Id.InfoMessage);
            view2.onButtonOk.RemoveListener(infoOkHandler);
        }

        void purchaseOkHandler(IAPType what)
        {
            InfoMessageView view = UI.Get<InfoMessageView>(UIMap.Id.InfoMessage);

            float price;
            string currency="?";

            iapService.GetProductPrice(what, out price, out currency);

            view.SetCaption("PURCHASE OK");
            if (what == IAPType.AdditionalLevels) {
                view.SetText("You have purchased 12 additional levels for "+currency + (currency.Length > 1 ? " " : "") + price.ToString("F2"));
            }
            else if (what == IAPType.NoAdverts) {
                view.SetText("You have purchased the permanent advert removal for "+currency + (currency.Length > 1 ? " " : "") + price.ToString("F2"));
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

        void buyNoAdsHandler()
        {
            UI.Hide(UIMap.Id.InfoMessage);

            InfoMessageView view = UI.Show<InfoMessageView>(UIMap.Id.InfoMessage);
            view.SetMessageMode(false);

            iapService.PurchaseStart(IAPType.NoAdverts);
        }


        void quitHandler()
        {
            Application.Quit();
        }

        public override void OnRegister()
        {
            view.Layout(Screen.width, Screen.height);

            view.onButtonStart.AddListener(homeHandler);
            view.onButtonOptions.AddListener(optionsHandler);
            view.onButtonConnect.AddListener(connectHandler);
            view.onButtonShop.AddListener(shopHandler);
            view.onButtonShopClose.AddListener(closeShopHandler);

            view.onButtonBuyLevels.AddListener(buyLevelsHandler);
            view.onButtonBuyNoAds.AddListener(buyNoAdsHandler);
            view.onButtonQuit.AddListener(quitHandler);

            onPurchaseOk.AddListener(purchaseOkHandler);
            onPurchaseFailed.AddListener(purchaseFailHandler);

            view.onButtonConnect.AddListener(connectFBClickHandler);

            view.ShowShop(false, iapService);
        }

        void connectFBClickHandler()
        {
            if (facebook.IsLoggedIn)
            {
                var message = UI.Show<InfoMessageView>(UIMap.Id.InfoMessage);
                message.Show(true);
                message.SetText("You already connected to Facebook!\nShare best scores with your friends!");
                message.SetCaption("Awesome!");
            }
            else
            {
                facebook.Login().Done(
                    () => analytics.FacebookConnected()
                );
            }
        }


        public override void OnRemove()
        {
            view.onButtonStart.RemoveListener(homeHandler);
            view.onButtonOptions.RemoveListener(optionsHandler);
            view.onButtonConnect.RemoveListener(connectHandler);
            view.onButtonShop.RemoveListener(shopHandler);
            view.onButtonShopClose.RemoveListener(closeShopHandler);
            view.onButtonConnect.RemoveListener(connectFBClickHandler);
            view.onButtonQuit.RemoveListener(quitHandler);

            onPurchaseOk.RemoveListener(purchaseOkHandler);
            onPurchaseFailed.RemoveListener(purchaseFailHandler);

            base.OnRemove();
        }
    }
}