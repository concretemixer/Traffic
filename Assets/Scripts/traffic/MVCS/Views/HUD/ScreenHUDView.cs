using UnityEngine;
using UnityEngine.UI;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;

namespace Traffic.MVCS.Views.UI.HUD
{
    public class ScreenHUDView : View
    {

        [SerializeField]
        Button pauseButton;

        [SerializeField]
        Text score;


		[SerializeField]
		Text scoreShadow;

        [SerializeField]
        RawImage progressBar;

        [SerializeField]
        RawImage progressBg;


		public readonly Signal onButtonPauseLevel = new Signal();
		public readonly Signal onButtonRetyLevel = new Signal();
		public readonly Signal onButtonExitLevel = new Signal();

        protected override void Awake()
        {
			pauseButton.onClick.AddListener(onButtonPauseLevel.Dispatch);
  //          retyButton.onClick.AddListener(onRetyLevel.Dispatch);
    //        infoButton.onClick.AddListener(onInfoRequired.Dispatch);

            base.Awake();
        }

        public void SetScore(int score)
        {
             this.score.text = score.ToString("D6");
		     this.scoreShadow.text = this.score.text;
        }

        public void SetProgress(int current, int target)
        {
            float k = (float) current / (float) target;

            if (current >= target)
                k = 1;

            progressBar.GetComponent<RectTransform>().localScale = new Vector3(k,1,1);
            progressBar.GetComponent<RawImage>().uvRect = new Rect(0, 0, k, 1);
        }


        protected override void OnDestroy()
        {
			pauseButton.onClick.RemoveListener(onButtonPauseLevel.Dispatch);
//            endButton.onClick.RemoveListener(onExitLevel.Dispatch);
  //          retyButton.onClick.RemoveListener(onRetyLevel.Dispatch);
    //        infoButton.onClick.RemoveListener(onInfoRequired.Dispatch);

            base.OnDestroy();
        }

        public void Layout()
        {
            
            float ratio = (float)Screen.height / (float)Screen.width;

            this.gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 960);
            this.gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 960*ratio);

            this.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -480 * ratio);
            this.gameObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1,1);

            if (ratio < 1)
            {
                progressBg.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -30);
                pauseButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-45, -50);
                scoreShadow.GetComponent<RectTransform>().anchoredPosition = new Vector2(110, -50);
                scoreShadow.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                progressBg.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                pauseButton.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            }
            else
            {
                progressBg.GetComponent<RectTransform>().anchoredPosition = new Vector2(16, -45);
                pauseButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-60, -70);
                scoreShadow.GetComponent<RectTransform>().anchoredPosition = new Vector2(160, -150);

                scoreShadow.GetComponent<RectTransform>().localScale = new Vector3(1.5f, 1.5f, 1.5f);
                progressBg.GetComponent<RectTransform>().localScale = new Vector3(2.0f, 2.0f, 2.0f);
                pauseButton.GetComponent<RectTransform>().localScale = new Vector3(1.5f, 1.5f, 1.5f);

            }
        }

    }
}