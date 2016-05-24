using UnityEngine;
using UnityEngine.UI;

namespace Traffic.MVCS.Views.UI.Loading
{
    public class LoadingScreenView : RotatableView
    {
        [SerializeField]
        Text statusLabel;
        [SerializeField]
        GameObject splash;
        [SerializeField]
        GameObject preloader;

        public void SetStatus(string text)
        {
            statusLabel.text = text;
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

        public void ShowPreloader(bool show)
        {
            // splash.SetActive(!show);
            preloader.SetActive(show);
        }
    }
}