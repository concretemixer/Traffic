using UnityEngine;
using UnityEngine.UI;
using strange.extensions.mediation.impl;
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

        protected override void Awake()
        {
            goButton.onClick.AddListener(onButtonResumeLevel.Dispatch);
            homeButton.onClick.AddListener(onButtonHome.Dispatch);
            base.Awake();
        }

       

        protected override void OnDestroy()
        {
            goButton.onClick.RemoveListener(onButtonResumeLevel.Dispatch);
            homeButton.onClick.RemoveListener(onButtonHome.Dispatch);
            base.OnDestroy();
        }

        public override void Layout()
        {

            float ratio = (float)Screen.height / (float)Screen.width;

            float scaledDimention;

            if (ratio < 1)
            {
                this.gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 960);
                this.gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 960 * ratio);
                scaledDimention = 960 * ratio;
            }
            else
            {
                this.gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 960 / ratio);
                this.gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 960);

                scaledDimention = 960 / ratio;
            }
        }
    }
}