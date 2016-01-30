using UnityEngine;
using UnityEngine.UI;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;

namespace Traffic.MVCS.Views.UI
{
    public class StartGameScreenView : RotatableView
    {

        [SerializeField]
        Button startButton;

        [SerializeField]
        Button connectButton;



        public readonly Signal onButtonStart = new Signal();
        public readonly Signal onButtonConnect = new Signal();

        protected override void Awake()
        {
            connectButton.onClick.AddListener(onButtonConnect.Dispatch);
            startButton.onClick.AddListener(onButtonStart.Dispatch);
            base.Awake();
        }
    

        protected override void OnDestroy()
        {
            connectButton.onClick.RemoveListener(onButtonConnect.Dispatch);
            startButton.onClick.RemoveListener(onButtonStart.Dispatch);
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

                startButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-105, 10);
                connectButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-300, 10);
                // scaledDimention = 960 * ratio;
            }
            else
            {
                this.gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 960 / ratio);
                this.gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 960);

                startButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-85, 10);
                connectButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-265, 10);

                // scaledDimention = 960 / ratio;
            }
            this.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            this.gameObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        }
    }
}