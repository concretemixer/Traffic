using Traffic.MVCS.Commands.Signals;
using Traffic.MVCS.Models;
using Commons.UI;
using UnityEngine;

using strange.extensions.mediation.impl;
using Commons.SN.Facebook;
using Commons.Utils;
using Traffic.MVCS.Services;

namespace Traffic.MVCS.Views.UI
{
    public class StartGameScreenMediator : Mediator
    {
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

        void buyNoAdsHandler()
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
            view.onButtonStart.AddListener(homeHandler);
            view.onButtonOptions.AddListener(optionsHandler);
            view.onButtonConnect.AddListener(connectHandler);
            view.onButtonShop.AddListener(shopHandler);
            view.onButtonShopClose.AddListener(closeShopHandler);

            view.onButtonBuyLevels.AddListener(buyLevelsHandler);
            view.onButtonBuyNoAds.AddListener(buyNoAdsHandler);

            view.ShowShop(false, iapService);
            view.Layout(Screen.width, Screen.height);

            view.onButtonConnect.AddListener(connectFBClickHandler);
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
                    analytics.FacebookConnected
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

            base.OnRemove();
        }
    }
}