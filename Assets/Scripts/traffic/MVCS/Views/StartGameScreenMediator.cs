using Traffic.MVCS.Commands.Signals;
using Traffic.MVCS.Models;
using Commons.UI;
using UnityEngine;

using strange.extensions.mediation.impl;
//using Commons.SN.Facebook;
using Traffic.MVCS.Services;
using Traffic.Components;

namespace Traffic.MVCS.Views.UI
{
    public class StartGameScreenMediator : Mediator
    {
        [Inject(EntryPoint.Container.Stage)]
        public GameObject stage { get; set; }

        [Inject]
        public PurshaseOk onPurchaseOk { get; set; }

        [Inject]
        public PurchaseFailed onPurchaseFailed { get; set; }

        [Inject]
        public RestorePurchasesFailed onRestorePurchaseFailed { get; set; }

        [Inject]
        public StartGameScreenView view { get; set; }

        [Inject]
        public IAPService iapService { get; set; }

        [Inject]
        public SwitchToMainScreenSignal toMainScreenSignal { get; set; }

        [Inject]
        public SwitchToSettingsScreenSignal toSettingsSignal { get; set; }

        //[Inject]
        //public FacebookSN facebook { private get; set; }
        
        public AnalyticsCollector analytics { private get; set; }

        [Inject]
        public IUIManager UI { get; set; }

        [Inject]
        public ILocaleService localeService { get; set; }


        private bool restoring = false;
        
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

        void topHandler()
        {
            WebDB webDB = stage.GetComponentInParent<WebDB>();
            if (webDB != null)
            {               
                view.ShowTop(true, webDB);
                localeService.SetAllTexts(view.gameObject);
            }
        }

        void closeTopHandler()
        {
            view.ShowTop(false, null);
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

            if (view==null)
                view = UI.Show<InfoMessageView>(UIMap.Id.InfoMessage);

            view.SetMessageMode(true);

            if (restoring)
            {
                view.SetCaption(localeService.ProcessString("%PURCHASE_RESTORED%"));
                view.SetText(localeService.ProcessString("%PURCHASE_RESTORED_TEXT%"));
            }
            else
            {
                view.SetCaption(localeService.ProcessString("%PURCHASE_OK%"));
                if (what == IAPType.AdditionalLevels)
                    view.SetText(localeService.ProcessString("%LEVELS_BOUGHT%"));
                else if (what == IAPType.NoAdverts)
                    view.SetText(localeService.ProcessString("%NO_ADS_BOUGHT%"));
            }
            

           
            view.onButtonOk.RemoveListener(infoOkHandler);
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

        void purchaseRestoreFailHandler()
        {
            restoring = false;
            InfoMessageView view = UI.Get<InfoMessageView>(UIMap.Id.InfoMessage);
            if (view == null)
                view = UI.Show<InfoMessageView>(UIMap.Id.InfoMessage);

            view.SetCaption(localeService.ProcessString("%PURCHASE_RESTORE_FAIL%"));
            view.SetText(localeService.ProcessString("%PURCHASE_RESTORE_FAIL_TEXT%"));
            view.SetMessageMode(true);
            view.onButtonOk.AddListener(infoOkHandler);
        }

        void buyLevelsHandler()
        {
            restoring = false;
            UI.Hide(UIMap.Id.InfoMessage);

            InfoMessageView view = UI.Show<InfoMessageView>(UIMap.Id.InfoMessage);
            view.SetMessageMode(false);
            iapService.PurchaseStart(IAPType.AdditionalLevels);
        }

        void buyNoAdsHandler()
        {
            restoring = false;
            UI.Hide(UIMap.Id.InfoMessage);

            InfoMessageView view = UI.Show<InfoMessageView>(UIMap.Id.InfoMessage);
            view.SetMessageMode(false);

            iapService.PurchaseStart(IAPType.NoAdverts);
        }


        void quitHandler()
        {
            Application.Quit();
        }

        void restorePurchasesHandler()
        {            
            UI.Hide(UIMap.Id.InfoMessage);
            restoring = true;
            iapService.RestorePurchases();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                quitHandler();
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
            onRestorePurchaseFailed.AddListener(purchaseRestoreFailHandler);

            view.onButtonConnect.AddListener(connectFBClickHandler);
            view.onButtonShopRestore.AddListener(restorePurchasesHandler);
            view.onButtonTop.AddListener(topHandler);
            view.onButtonTopClose.AddListener(closeTopHandler);

            view.ShowShop(false, iapService);
        }

        void connectFBClickHandler()
        {
            
            /*
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
            } */
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
            onRestorePurchaseFailed.RemoveListener(purchaseRestoreFailHandler);

            view.onButtonShopRestore.RemoveListener(restorePurchasesHandler);
            view.onButtonTop.RemoveListener(topHandler);
            view.onButtonTopClose.RemoveListener(closeTopHandler);

            base.OnRemove();
        }
    }
}