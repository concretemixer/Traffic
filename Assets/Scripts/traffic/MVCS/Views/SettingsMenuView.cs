using UnityEngine;
using UnityEngine.UI;
using strange.extensions.signal.impl;
using strange.extensions.mediation.impl;
using System.Collections.Generic;

namespace Traffic.MVCS.Views.UI
{
    public class SettingsMenuView : RotatableView
    {
        [SerializeField]
        Button backButton;

        [SerializeField]
        Button codeButton;


        [SerializeField]
        Button langButton;

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
        Toggle shadowsToggle;

        [SerializeField]
        Button[] pinButtons;

        [SerializeField]
        Button pinDelete;

        [SerializeField]
        Button pinOk;

        [SerializeField]
        Image langBg;

        [SerializeField]
        Button langCloseButton;

        [SerializeField]
        Button[] languages;

        public string Code {
            get { return codeText.text; }
        }

        public readonly Signal onButtonCode = new Signal();
        public readonly Signal onButtonCodeClose = new Signal();
        public readonly Signal onButtonCodeOk = new Signal();
        public readonly Signal onButtonBack = new Signal();
        public readonly Signal onButtonLang = new Signal();
        public readonly Signal onButtonLangClose = new Signal();
        

        public readonly Signal<float> onMusicVolume= new Signal<float>();
        public readonly Signal<float> onSoundVolume = new Signal<float>();
        public readonly Signal<bool> onShadowsToggle = new Signal<bool>();

        public readonly Signal<SystemLanguage> onLangChoosen = new Signal<SystemLanguage>();

        protected override void Awake()
        {
            backButton.onClick.AddListener(onButtonBack.Dispatch);
            codeButton.onClick.AddListener(onButtonCode.Dispatch);
            codeCloseButton.onClick.AddListener(onButtonCodeClose.Dispatch);

            musicSlider.onValueChanged.AddListener(onMusicVolume.Dispatch);
            soundSlider.onValueChanged.AddListener(onSoundVolume.Dispatch);
            shadowsToggle.onValueChanged.AddListener(onShadowsToggle.Dispatch);

            langButton.onClick.AddListener(onButtonLang.Dispatch);
            langCloseButton.onClick.AddListener(onButtonLangClose.Dispatch);

            pinOk.onClick.AddListener(onButtonCodeOk.Dispatch);

            musicSlider.value = PlayerPrefs.GetFloat("volume.music", 1);
            soundSlider.value = PlayerPrefs.GetFloat("volume.sound", 1);
            shadowsToggle.isOn = PlayerPrefs.GetInt("gfx.shadows", 1) > 0;

#if UNITY_IOS
            shadowsToggle.gameObject.SetActive(false);
#endif



            foreach (Button b in pinButtons)
            {
                string text = b.GetComponentInChildren<Text>().text;
                b.onClick.AddListener(delegate { OnPinClick(text); });
            }

            pinDelete.onClick.AddListener(OnPinDelete);

            languages[0].onClick.AddListener(delegate { onLangChoosen.Dispatch(SystemLanguage.English); });
            languages[1].onClick.AddListener(delegate { onLangChoosen.Dispatch(SystemLanguage.Russian); });
            languages[2].onClick.AddListener(delegate { onLangChoosen.Dispatch(SystemLanguage.French); });

            base.Awake();
        }

        protected override void OnDestroy()
        {
            backButton.onClick.RemoveListener(onButtonBack.Dispatch);
            codeButton.onClick.RemoveListener(onButtonCode.Dispatch);
            codeCloseButton.onClick.RemoveListener(onButtonCodeClose.Dispatch);

            musicSlider.onValueChanged.RemoveListener(onMusicVolume.Dispatch);
            soundSlider.onValueChanged.RemoveListener(onSoundVolume.Dispatch);
            shadowsToggle.onValueChanged.RemoveListener(onShadowsToggle.Dispatch);

            langButton.onClick.RemoveListener(onButtonLang.Dispatch);
            langCloseButton.onClick.RemoveListener(onButtonLangClose.Dispatch);

            pinOk.onClick.RemoveListener(onButtonCodeOk.Dispatch);

            foreach (Button b in pinButtons)
            {                
                b.onClick.RemoveAllListeners();
            }

            pinDelete.onClick.RemoveListener(OnPinDelete);

            foreach (Button b in languages)
            {
                b.onClick.RemoveAllListeners();
            }


            base.OnDestroy();
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



        private bool _ingame = false;

        public bool Ingame
        {
            get { return _ingame; }
        }


        public void SetIngame(bool value)
        {
            _ingame = value;
            codeButton.gameObject.SetActive(!value);
        }

        public void ShowCode(bool show)
        {
            codeBg.gameObject.SetActive(show);
        }

        public void ShowLangSelection(bool show)
        {
            langBg.gameObject.SetActive(show);
        }

        private static Dictionary<SystemLanguage, string> map = new Dictionary<SystemLanguage, string>
        {
            {SystemLanguage.English, "locale/icons/eng"},
            {SystemLanguage.Russian, "locale/icons/ru"},
            {SystemLanguage.French, "locale/icons/fr"}
        };

        public void SetLanguage(SystemLanguage lang)
        {
            Sprite img = UnityEngine.Resources.Load<Sprite>(map[lang]);
            langButton.image.sprite = img;

            if (lang == SystemLanguage.English)
                languages[0].image.color = new Color32(255, 255, 255, 150);
            if (lang == SystemLanguage.Russian)
                languages[1].image.color = new Color32(255, 255, 255, 150);
            if (lang == SystemLanguage.French)
                languages[2].image.color = new Color32(255, 255, 255, 150);
        }

        public override void Layout(int width, int height)
        {
            base.Layout(width, height);

            if (Time.timeScale == 0)
            {
                codeButton.gameObject.SetActive(false);
                backButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(5, -50); 
            }
            else
                backButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(5, -5); 

            float ratio = (float)height / (float)width;

            // float scaledDimention;

            if (ratio < 1)
            {
                this.gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 960);
                this.gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 960 * ratio);
                // scaledDimention = 960 * ratio;

                musicSlider.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 540);
                musicSlider.GetComponent<RectTransform>().anchoredPosition = new Vector2(153, 127);
                soundSlider.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 540);
                soundSlider.GetComponent<RectTransform>().anchoredPosition = new Vector2(153, 68);

                langButton.GetComponent<RectTransform>().localScale = new Vector2(1,1);
                langButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(440, 195);

               shadowsToggle.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                shadowsToggle.GetComponent<RectTransform>().anchoredPosition = new Vector2(-30, 185);

            }
            else
            {
                this.gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 960 / ratio);
                this.gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 960);


                musicSlider.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 304);
                musicSlider.GetComponent<RectTransform>().anchoredPosition = new Vector2(108, 112);
                soundSlider.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 304);
                soundSlider.GetComponent<RectTransform>().anchoredPosition = new Vector2(108, 52);


                langButton.GetComponent<RectTransform>().localScale = new Vector2(0.6f, 0.6f);
                langButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(272, 178);

               shadowsToggle.GetComponent<RectTransform>().localScale = new Vector3(.8f, .8f, .8f);
                shadowsToggle.GetComponent<RectTransform>().anchoredPosition = new Vector2(25, 160);

                // scaledDimention = 960 / ratio;
            }

#if UNITY_WEBGL
            codeButton.gameObject.SetActive(false);
#endif

            musicSlider.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            soundSlider.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);           

            this.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            this.gameObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);           
        }
    }
}