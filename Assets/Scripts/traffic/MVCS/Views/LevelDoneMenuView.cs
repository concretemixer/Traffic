using UnityEngine;
using UnityEngine.UI;
using strange.extensions.signal.impl;

namespace Traffic.MVCS.Views.UI
{
    public class LevelDoneMenuView : RotatableView
    {
        [SerializeField]
        Button homeButton;

        [SerializeField]
        Button nextButton;

        [SerializeField]
        Button shareButton;

        [SerializeField]
        Text score;

        [SerializeField]
        Text doneVertical;

        [SerializeField]
        Text doneHorizontal;

        [SerializeField]
        Image badge;

        public AudioClip successSound;

        public readonly Signal onButtonNextLevel = new Signal();
        public readonly Signal onButtonHome = new Signal();
        public readonly Signal onShareButton = new Signal();

        protected override void Awake()
        {
            homeButton.onClick.AddListener(onButtonHome.Dispatch);
            nextButton.onClick.AddListener(onButtonNextLevel.Dispatch);
            shareButton.onClick.AddListener(onShareButton.Dispatch);
            base.Awake();
        }

        public void ShowBadge(bool value)
        {
            badge.gameObject.SetActive(value);
        }

        public void SetScore(int score, int stars)
        {
            this.score.text = score.ToString("D6");

            if (stars >= 2 )
            {
                GameObject.Find("NoStar2Img").GetComponent<Image>().color = Color.clear;
                GameObject.Find("NoStar2Img_v").GetComponent<Image>().color = Color.clear;
            }

            if (stars >= 3)
            {
                GameObject.Find("NoStar3Img").GetComponent<Image>().color = Color.clear;
                GameObject.Find("NoStar3Img_v").GetComponent<Image>().color = Color.clear;
            }

        }

        public void SetMessage(string text)
        {
            this.doneVertical.text = text;
            this.doneHorizontal.text = text;
        }

        protected override void OnDestroy()
        {
            nextButton.onClick.RemoveListener(onButtonNextLevel.Dispatch);
            homeButton.onClick.RemoveListener(onButtonHome.Dispatch);
            shareButton.onClick.RemoveListener(onShareButton.Dispatch);
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

                nextButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-105, 10);
                shareButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-261, 10);
                homeButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-392, 10);
                score.GetComponent<RectTransform>().anchoredPosition = new Vector2(300, 180);
            }
            else
            {
                this.gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 960 / ratio);
                this.gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 960);

                nextButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-85, 10);
                shareButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-225, 10);
                homeButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-342, 10);

                score.GetComponent<RectTransform>().anchoredPosition = new Vector2(115, 180);

                // scaledDimention = 960 / ratio;
            }
            this.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            this.gameObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);

#if UNITY_WEBGL
            homeButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-240, 10);
            //homeButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-342, 10);
            shareButton.gameObject.SetActive(false);
#endif
        }
    }
}