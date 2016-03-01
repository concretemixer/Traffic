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

        [SerializeField]
        Image handImage;

        [SerializeField]
        Image frameImage;

        [SerializeField]
        Image shadeImage;

        [SerializeField]
        Image bubbleTopImage;

        [SerializeField]
        Image bubbleBottomImage;


        int step = -1;
        public int Step
        {
            get { return step; }            
        }


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

        public void SetStep(int step)
        {
            this.step = step;
        }

        public void SetHandPos(float x, float y)
        {
            handImage.gameObject.SetActive(true);
            handImage.GetComponent<RectTransform>().anchoredPosition = new Vector3(x, y, 0);            
        }

        public void SetShadePos(float x, float y)
        {
            shadeImage.gameObject.SetActive(true);
            shadeImage.GetComponent<RectTransform>().anchoredPosition = new Vector3(x, y, 0);            
        }

        public void SetBubblePos(float x, float y, int shift, bool top, bool right)
        {
            bubbleBottomImage.GetComponent<RectTransform>().anchoredPosition = new Vector3(x, y, 0);
            bubbleTopImage.GetComponent<RectTransform>().anchoredPosition = new Vector3(x, y, 0);
            bubbleBottomImage.gameObject.SetActive(!top);
            bubbleTopImage.gameObject.SetActive(top);

            Image bubble = top ? bubbleTopImage : bubbleBottomImage;

            foreach (Image img in bubble.gameObject.GetComponentsInChildren<Image>())
            {
                if (img.gameObject.name == "Bubble")
                {
                    Vector3 pos = img.GetComponent<RectTransform>().anchoredPosition;
                    img.GetComponent<RectTransform>().anchoredPosition = new Vector3(shift, pos.y, 0);
                }
                if (img.gameObject.name == "BubbleTipLeft")
                {
                    img.GetComponent<Image>().enabled = !right;
                }
                if (img.gameObject.name == "BubbleTipRight")
                {
                    img.GetComponent<Image>().enabled = right;
                }

            }
        }

        public void SetBubbleText(string text)
        {
            foreach (var t in bubbleBottomImage.gameObject.GetComponentsInChildren<Text>())
                t.text = text;
            foreach (var t in bubbleTopImage.gameObject.GetComponentsInChildren<Text>())
                t.text = text;
        }


        public void SetHandAlpha(float a)
        {
            handImage.GetComponent<Image>().color = new Color(1, 1, 1, a);
        }

        public override void Layout(int width, int height)
        {

            base.Layout(width, height);

            float ratio = (float)height / (float)width;

            float scaledDimention;

            if (ratio < 1)
            {
                scaledDimention = 960 * ratio;


                this.gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 960);
                this.gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 960 * ratio);
                frameImage.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 960);
                frameImage.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, scaledDimention - 60);
            }
            else
            {
                scaledDimention = 960 / ratio;

                this.gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 960 / ratio);
                this.gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 960);
                frameImage.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, scaledDimention);
                frameImage.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 900);

            }
            this.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            this.gameObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        }
    }
}