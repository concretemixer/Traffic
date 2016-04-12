using UnityEngine;
using UnityEngine.UI;
using strange.extensions.signal.impl;
using Traffic.MVCS.Models;

namespace Traffic.MVCS.Views.UI
{
    public class StartGameScreenView : RotatableView
    {

        [SerializeField]
        Button startButton;

        [SerializeField]
        Button connectButton;

        [SerializeField]
        Button shopButton;

        [SerializeField]
        Button optionsButton;

        [SerializeField]
        Button quitButton;

        [SerializeField]
        Image shopBg;

        [SerializeField]
        Button shopCloseButton;

        [SerializeField]
        Button shopBuyLevels;

        [SerializeField]
        Button shopBuyNoAdverts;


        [SerializeField]
        Image shopLevelsBought;

        [SerializeField]
        Image shopNoAdvertsBought;


        public readonly Signal onButtonStart = new Signal();
        public readonly Signal onButtonConnect = new Signal();
        public readonly Signal onButtonOptions = new Signal();
        public readonly Signal onButtonQuit = new Signal();
        public readonly Signal onButtonShop = new Signal();
        public readonly Signal onButtonShopClose = new Signal();

        public readonly Signal onButtonBuyLevels= new Signal();
        public readonly Signal onButtonBuyNoAds= new Signal();


        protected override void Awake()
        {
            shopCloseButton.onClick.AddListener(onButtonShopClose.Dispatch);
            shopButton.onClick.AddListener(onButtonShop.Dispatch);
            optionsButton.onClick.AddListener(onButtonOptions.Dispatch);
            connectButton.onClick.AddListener(onButtonConnect.Dispatch);
            startButton.onClick.AddListener(onButtonStart.Dispatch);
            quitButton.onClick.AddListener(onButtonQuit.Dispatch);

            shopBuyLevels.onClick.AddListener(onButtonBuyLevels.Dispatch);
            shopBuyNoAdverts.onClick.AddListener(onButtonBuyNoAds.Dispatch);
            base.Awake();
        }
    

        protected override void OnDestroy()
        {
            quitButton.onClick.RemoveListener(onButtonQuit.Dispatch);
            shopBuyLevels.onClick.RemoveListener(onButtonBuyLevels.Dispatch);
            shopBuyNoAdverts.onClick.RemoveListener(onButtonBuyNoAds.Dispatch);
            shopCloseButton.onClick.RemoveListener(onButtonShopClose.Dispatch);
            shopButton.onClick.RemoveListener(onButtonShop.Dispatch);
            optionsButton.onClick.RemoveListener(onButtonOptions.Dispatch);
            connectButton.onClick.RemoveListener(onButtonConnect.Dispatch);
            startButton.onClick.RemoveListener(onButtonStart.Dispatch);


            base.OnDestroy();
        }

        public void ShowShop(bool show, IAPService iapService)
        {
            if (show)
            {
                if (iapService.IsBought(IAPType.AdditionalLevels))
                {
                    shopBuyLevels.gameObject.SetActive(false);
                    shopLevelsBought.gameObject.SetActive(true);
                }
                if (iapService.IsBought(IAPType.NoAdverts))
                {
                    shopBuyNoAdverts.gameObject.SetActive(false);
                    shopNoAdvertsBought.gameObject.SetActive(true);
                }
            }
            shopBg.gameObject.SetActive(show);
        }

        public override void Layout(int width, int height)
        {
            base.Layout(width, height);

            float ratio = (float)height / (float)width;

            // float scaledDimention;

            if (ratio < 1)
            {
                this.gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 960);
                this.gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 960 * ratio);

                startButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-105, 10);
                connectButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-300, 10);
                shopButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-470, 10);
                // scaledDimention = 960 * ratio;
            }
            else
            {
                this.gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 960 / ratio);
                this.gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 960);

                startButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-85, 10);
                connectButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-265, 10);
                shopButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-425, 10);

                // scaledDimention = 960 / ratio;
            }
            this.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            this.gameObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        }
    }
}