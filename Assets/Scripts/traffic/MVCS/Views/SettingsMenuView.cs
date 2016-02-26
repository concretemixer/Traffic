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

        [SerializeField]
        Text codeText;

        [SerializeField]
        Button[] pinButtons;

        [SerializeField]
        Button pinDelete;

        [SerializeField]
        Button pinOk;

        public readonly Signal onButtonCode = new Signal();
        public readonly Signal onButtonCodeClose = new Signal();
        public readonly Signal onButtonCodeOk = new Signal();
        public readonly Signal onButtonBack = new Signal();
        

        public readonly Signal<float> onMusicVolume= new Signal<float>();
        public readonly Signal<float> onSoundVolume = new Signal<float>();

        protected override void Awake()
        {
            backButton.onClick.AddListener(onButtonBack.Dispatch);
            codeButton.onClick.AddListener(onButtonCode.Dispatch);
            codeCloseButton.onClick.AddListener(onButtonCodeClose.Dispatch);
            pinOk.onClick.AddListener(onButtonCodeOk.Dispatch);

            musicSlider.onValueChanged.AddListener(onMusicVolume.Dispatch);
            soundSlider.onValueChanged.AddListener(onSoundVolume.Dispatch);

            musicSlider.value = PlayerPrefs.GetFloat("volume.music", 1);
            soundSlider.value = PlayerPrefs.GetFloat("volume.sound", 1);

            foreach (Button b in pinButtons)
            {
                string text = b.GetComponentInChildren<Text>().text;
                b.onClick.AddListener(delegate { OnPinClick(text); });
            }

            pinDelete.onClick.AddListener(OnPinDelete);
            

            base.Awake();
        }

        private void OnPinClick(string text)
        {
            bool cursor = false;
            int len = codeText.text.Length;
            if (codeText.text.EndsWith("_"))
            {
                cursor = true;
                codeText.text = codeText.text.Substring(0, len - 1);
            }

            codeText.text += text;
            if (codeText.text.Length > 16)
                codeText.text = codeText.text.Substring(1);
            if (cursor)
                codeText.text += "_";
        }

        private void OnPinDelete()
        {
            int len = codeText.text.Length;
            if (len == 0)
                return;

            if (codeText.text.EndsWith("_"))
            {
                codeText.text = codeText.text.Substring(0, len - 1);
                len--;
                if (len == 0)
                    return;
            }

            codeText.text = codeText.text.Substring(0,len-1);
        }



        private float cursorCooldown = 0;
        private void Update()
        {
            cursorCooldown -= Time.deltaTime;
            if (cursorCooldown < 0)
            {
                int len = codeText.text.Length;
                if (codeText.text.EndsWith("_"))
                    codeText.text = codeText.text.Substring(0, len - 1);
                else
                    codeText.text += "_";
                cursorCooldown = 0.5f;
            }
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

        public override void Layout(int width, int height)
        {
            base.Layout(width, height);

            if (Time.timeScale == 0)
                codeButton.gameObject.SetActive(false);

            float ratio = (float)height / (float)width;

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