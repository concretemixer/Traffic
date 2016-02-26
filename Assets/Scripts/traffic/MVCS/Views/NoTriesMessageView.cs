using UnityEngine;
using UnityEngine.UI;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;

namespace Traffic.MVCS.Views.UI
{
    public class NoTriesMessageView : RotatableView
    {
        [SerializeField]
        Button closeButton;

        [SerializeField]
        Button advertButton;

        [SerializeField]
        Button buyButton;

        [SerializeField]
        Text timerText;

        public readonly Signal onButtonBuy = new Signal();
        public readonly Signal onButtonAdvert = new Signal();
        public readonly Signal onButtonClose = new Signal();

        public void SetTimerText(string text)
        {
            this.timerText.text = text;
        }
  

        protected override void Awake()
        {
            closeButton.onClick.AddListener(onButtonClose.Dispatch);
            advertButton.onClick.AddListener(onButtonAdvert.Dispatch);
            buyButton.onClick.AddListener(onButtonBuy.Dispatch);
            base.Awake();
        }

       

        protected override void OnDestroy()
        {
            closeButton.onClick.RemoveListener(onButtonClose.Dispatch);
            advertButton.onClick.RemoveListener(onButtonAdvert.Dispatch);
            buyButton.onClick.RemoveListener(onButtonBuy.Dispatch);
            base.OnDestroy();
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
                // scaledDimention = 960 * ratio;
            }
            else
            {
                this.gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 960 / ratio);
                this.gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 960);

                // scaledDimention = 960 / ratio;
            }
            this.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            this.gameObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        }
    }
}