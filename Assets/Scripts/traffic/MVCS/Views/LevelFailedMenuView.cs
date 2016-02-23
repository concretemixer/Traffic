using UnityEngine;
using UnityEngine.UI;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;

namespace Traffic.MVCS.Views.UI
{
    public class LevelFailedMenuView : RotatableView
    {
        [SerializeField]
        Button homeButton;

        [SerializeField]
        Button retryButton;

        [SerializeField]
        Button closeButton;

        [SerializeField]
        Button advertButton;

        [SerializeField]
        Image lockedImage;

        [SerializeField]
        Text score;

        public readonly Signal onButtonAdvert = new Signal();
        public readonly Signal onButtonRetryLevel = new Signal();
        public readonly Signal onButtonHome = new Signal();
        public readonly Signal onButtonClose = new Signal();

        public void SetScore(int score)
        {
            this.score.text = score.ToString("D6");
        }

        public void SetLocked(bool locked)
        {
            this.lockedImage.gameObject.SetActive(locked);
        }    

        protected override void Awake()
        {
            homeButton.onClick.AddListener(onButtonHome.Dispatch);
            retryButton.onClick.AddListener(onButtonRetryLevel.Dispatch);
            closeButton.onClick.AddListener(onButtonClose.Dispatch);
            advertButton.onClick.AddListener(onButtonAdvert.Dispatch);
            base.Awake();
        }

       

        protected override void OnDestroy()
        {
            homeButton.onClick.RemoveListener(onButtonHome.Dispatch);
            retryButton.onClick.RemoveListener(onButtonRetryLevel.Dispatch);
            closeButton.onClick.RemoveListener(onButtonClose.Dispatch);
            advertButton.onClick.RemoveListener(onButtonAdvert.Dispatch);
            base.OnDestroy();
        }

        public override void Layout()
        {
            base.Layout();

            float ratio = (float)Screen.height / (float)Screen.width;

            // float scaledDimention;

            if (ratio < 1)
            {
                this.gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 960);
                this.gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 960 * ratio);
                // scaledDimention = 960 * ratio;
                retryButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-105, 10);
                homeButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-250, 10);
                score.GetComponent<RectTransform>().anchoredPosition = new Vector2(300, 172);
            }
            else
            {
                this.gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 960 / ratio);
                this.gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 960);

                retryButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-85, 10);
                homeButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-215, 10);


                score.GetComponent<RectTransform>().anchoredPosition = new Vector2(100, 172);
                // scaledDimention = 960 / ratio;
            }
            this.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            this.gameObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        }
    }
}