using UnityEngine;
using UnityEngine.UI;
using strange.extensions.signal.impl;
using strange.extensions.mediation.impl;

namespace Traffic.MVCS.Views.UI
{
    public class SettingsMenuView : RotatableView
    {
        [SerializeField]
        Button backButton;

        [SerializeField]
        Button codeButton;

        [SerializeField]
        Image codeBg;

        [SerializeField]
        Button codeCloseButton;

        [SerializeField]
        Slider musicSlider;

        [SerializeField]
        Slider soundSlider;


        public readonly Signal onButtonCode = new Signal();
        public readonly Signal onButtonCodeClose = new Signal();
        public readonly Signal onButtonBack = new Signal();

        public readonly Signal<float> onMusicVolume= new Signal<float>();
        public readonly Signal<float> onSoundVolume = new Signal<float>();

        protected override void Awake()
        {
            backButton.onClick.AddListener(onButtonBack.Dispatch);
            codeButton.onClick.AddListener(onButtonCode.Dispatch);
            codeCloseButton.onClick.AddListener(onButtonCodeClose.Dispatch);

            musicSlider.onValueChanged.AddListener(onMusicVolume.Dispatch);
            soundSlider.onValueChanged.AddListener(onSoundVolume.Dispatch);

            musicSlider.value = PlayerPrefs.GetFloat("volume.music", 1);
            soundSlider.value = PlayerPrefs.GetFloat("volume.sound", 1);

            base.Awake();
        }

       

        protected override void OnDestroy()
        {
            backButton.onClick.RemoveListener(onButtonBack.Dispatch);
            codeButton.onClick.RemoveListener(onButtonCode.Dispatch);
            codeCloseButton.onClick.RemoveListener(onButtonCodeClose.Dispatch);

            musicSlider.onValueChanged.RemoveListener(onMusicVolume.Dispatch);
            soundSlider.onValueChanged.RemoveListener(onSoundVolume.Dispatch);

            base.OnDestroy();
        }

        public void ShowCode(bool show)
        {
            codeBg.gameObject.SetActive(show);
        }

        public override void Layout()
        {
            base.Layout();

            if (Time.timeScale == 0)
                codeButton.gameObject.SetActive(false);

            float ratio = (float)Screen.height / (float)Screen.width;

            // float scaledDimention;

            if (ratio < 1)
            {
                this.gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 960);
                this.gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 960 * ratio);
                // scaledDimention = 960 * ratio;

                musicSlider.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 540);
                musicSlider.GetComponent<RectTransform>().anchoredPosition = new Vector2(153, 156);
                soundSlider.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 540);
                soundSlider.GetComponent<RectTransform>().anchoredPosition = new Vector2(153, 87);
            }
            else
            {
                this.gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 960 / ratio);
                this.gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 960);


                musicSlider.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 304);
                musicSlider.GetComponent<RectTransform>().anchoredPosition = new Vector2(108, 145);
                soundSlider.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 304);
                soundSlider.GetComponent<RectTransform>().anchoredPosition = new Vector2(108, 75);

                // scaledDimention = 960 / ratio;
            }

            musicSlider.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            soundSlider.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);           

            this.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            this.gameObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);           
        }
    }
}