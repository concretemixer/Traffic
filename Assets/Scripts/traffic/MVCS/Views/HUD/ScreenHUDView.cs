using UnityEngine;
using UnityEngine.UI;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;

namespace Traffic.MVCS.Views.UI.HUD
{
    public class ScreenHUDView : RotatableView
    {

        [SerializeField]
        Button pauseButton;

        [SerializeField]
        Text score;

        [SerializeField]
        Text tries;

        [SerializeField]
        RawImage progressBar;

        [SerializeField]
        RawImage progressBg;

        [SerializeField]
        Image bg;

		public readonly Signal onButtonPauseLevel = new Signal();
		public readonly Signal onButtonRetyLevel = new Signal();
		public readonly Signal onButtonExitLevel = new Signal();

        protected override void Awake()
        {
			pauseButton.onClick.AddListener(onButtonPauseLevel.Dispatch);
            base.Awake();
        }

        public void SetTutorial(bool value)
        {
            this.pauseButton.gameObject.SetActive(!value);
        }

        public void SetScore(int score)
        {
             this.score.text = score.ToString("D6");		    
        }

        public void SetTries(int left,int max)
        {
            if (max==int.MaxValue)
                this.tries.text = "∞";
            else
               this.tries.text = left.ToString() + "/" + max.ToString();
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

        public override void Layout(int width, int height)
        {
            base.Layout(width, height);

            float ratio = (float)height / (float)width;

            float scaledDimention; 

            if (ratio < 1)
            {
                this.gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 960);
                this.gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 960 * ratio);
                scaledDimention = 960 * ratio;
                //this.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -scaledDimention/2);
            }
            else
            {
                this.gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 960 / ratio);
                this.gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 960);
             //   this.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -480);
                scaledDimention = 960 / ratio;
            }

            this.gameObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);

            if (ratio < 1)
            {
                bg.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);

                progressBg.GetComponent<RectTransform>().anchoredPosition = new Vector2(-36,  - 20);
                progressBg.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);


                pauseButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(50, 45);
                score.GetComponent<RectTransform>().anchoredPosition = new Vector2(-85, -30);

            }
            else
            {
                bg.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
                progressBg.GetComponent<RectTransform>().anchoredPosition = new Vector2(-36, -20);
                if (scaledDimention<599)
                    progressBg.GetComponent<RectTransform>().localScale = new Vector3(0.4f, 1, 1);
                else
                    progressBg.GetComponent<RectTransform>().localScale = new Vector3(0.5f, 1, 1);

               pauseButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(50, 45);

               score.GetComponent<RectTransform>().anchoredPosition = new Vector2(-85, -30);

            }
        }

    }
}