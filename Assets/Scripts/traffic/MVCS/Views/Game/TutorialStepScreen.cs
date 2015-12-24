using UnityEngine;
using UnityEngine.UI;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;

namespace Traffic.MVCS.Views.UI
{
    public class TutorialStepScreen : RotatableView
    {
        [SerializeField]
        Button nextButton;



        public readonly Signal onButtonNextStep = new Signal();
        

        protected override void Awake()
        {
            nextButton.onClick.AddListener(onButtonNextStep.Dispatch);
            base.Awake();
        }

        protected override void OnDestroy()
        {
            nextButton.onClick.RemoveListener(onButtonNextStep.Dispatch);
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
            this.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            this.gameObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        }
    }
}