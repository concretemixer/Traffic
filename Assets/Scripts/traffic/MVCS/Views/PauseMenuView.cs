using UnityEngine;
using UnityEngine.UI;
using strange.extensions.signal.impl;
using strange.extensions.mediation.impl;

namespace Traffic.MVCS.Views.UI
{
    public class PauseMenuView : RotatableView
    {
        [SerializeField]
        Button homeButton;

        [SerializeField]
        Button goButton;

        [SerializeField]
        Button optionsButton;


        public readonly Signal onButtonResumeLevel = new Signal();
        public readonly Signal onButtonHome = new Signal();
        public readonly Signal onButtonSettings= new Signal();

        protected override void Awake()
        {
            goButton.onClick.AddListener(onButtonResumeLevel.Dispatch);
            homeButton.onClick.AddListener(onButtonHome.Dispatch);
            optionsButton.onClick.AddListener(onButtonSettings.Dispatch);
            base.Awake();
        }

       

        protected override void OnDestroy()
        {
            optionsButton.onClick.RemoveListener(onButtonSettings.Dispatch);
            goButton.onClick.RemoveListener(onButtonResumeLevel.Dispatch);
            homeButton.onClick.RemoveListener(onButtonHome.Dispatch);
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

                goButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-105, 10);
                optionsButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-250, 10);
                homeButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-370, 10);
            }
            else
            {
                this.gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 960 / ratio);
                this.gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 960);

                goButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-85, 10);
                optionsButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-215, 10);
                homeButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-325, 10);

                // scaledDimention = 960 / ratio;
            }

            this.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            this.gameObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        }
    }
}