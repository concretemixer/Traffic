using Traffic.MVCS.Commands.Signals;
//using traffic.MVCS.model;
//using traffic.MVCS.model.level;
using Traffic.MVCS.Views.UI.HUD;
using Traffic.MVCS.Models;
using Traffic.Core;
using Commons.UI;
using Commons.Utils;
using UnityEngine;
using Traffic.Components;

using strange.extensions.mediation.impl;

namespace Traffic.MVCS.Views.UI
{
    public class SettingsMenuMediator : Mediator
    {
        [Inject]
        public SettingsMenuView view
        {
            get;
            set;
        }

        [Inject]
        public ILocaleService localeService { get; set; }
                 
        [Inject]
        public SwitchToStartScreenSignal toStartScreenSignal
        {
            get;
            set;
        }

        [Inject]
        public IAPService iapService { get; set; }

        [Inject(EntryPoint.Container.Stage)]
        public GameObject stage { get; set; }

        [Inject]
        public LevelPause onPause { get; set; }
        /*
        [Inject]
        public RetyLevelSignal retyLevelSignal
        {
            get;
            set;
        }
            */
        [Inject]
        public IUIManager UI {
            get;
            set;
        }

        void codeHandler()
        {
            view.ShowCode(true);
        }

        void codeCloseHandler()
        {
            view.ShowCode(false);
        }

        void langCloseHandler()
        {
            view.ShowLangSelection(false);
        }

        void langChosenHandler(SystemLanguage lang)
        {
            view.ShowLangSelection(false);

            localeService.SetCurrentLanguage(lang);            

            UI.Hide(UIMap.Id.ScreenSettings);
            UI.Show(UIMap.Id.ScreenSettings);
        }

        void infoOkHandler()
        {
            InfoMessageView view2 = UI.Get<InfoMessageView>(UIMap.Id.InfoMessage);
            view2.onButtonOk.RemoveListener(infoOkHandler);
        }

        void codeOkHandler()
        {
            view.ShowCode(false);
            InfoMessageView view2 = UI.Show<InfoMessageView>(UIMap.Id.InfoMessage);
            if (iapService.ApplyCode(view.Code.Replace("_","")))
            {
                view2.SetCaption(localeService.ProcessString("%CODE_CAPTION%"));
                view2.SetText(localeService.ProcessString("%CODE_OK%"));
            }
            else
            {
                
                view2.SetCaption(localeService.ProcessString("%CODE_CAPTION%"));
                view2.SetText(localeService.ProcessString("%CODE_FAIL%"));
            }
            view2.SetMessageMode(true);
            view2.onButtonOk.AddListener(infoOkHandler);

        }

        void langHandler()
        {
            view.ShowLangSelection(true);
            return;
            if (localeService.GetCurrentLanguage() == SystemLanguage.Russian)
                localeService.SetCurrentLanguage(SystemLanguage.English);
            else
                localeService.SetCurrentLanguage(SystemLanguage.Russian);
            view.SetLanguage(localeService.GetCurrentLanguage());

            UI.Hide(UIMap.Id.ScreenSettings);
            UI.Show(UIMap.Id.ScreenSettings);
        }


        void homeHandler()
        {
            if (Time.timeScale == 0)
                onPause.Dispatch();
            else
                toStartScreenSignal.Dispatch();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                homeHandler();
            }
        }


        void musicVolumeHandler(float value)
        {
            PlayerPrefs.SetFloat("volume.music",value);

            
            AudioSource gameMusic = GameObject.Find("GameMusic").GetComponent<AudioSource>();
            AudioSource menuMusic = GameObject.Find("MenuMusic").GetComponent<AudioSource>();
            
            gameMusic.volume = value;
            menuMusic.volume = value;

            AudioSource gameAmbient = GameObject.Find("GameAmbient").GetComponent<AudioSource>();
            if (view.Ingame)
                gameAmbient.mute = true;// value > 0;
            else
                gameAmbient.mute = true;
        }

        void soundVolumeHandler(float value)
        {
            PlayerPrefs.SetFloat("volume.sound", value);

            AudioSource gameAmbient = GameObject.Find("GameAmbient").GetComponent<AudioSource>();
            gameAmbient.GetComponent<AudioSource>().volume = value;
            foreach (AudioSource src in stage.GetComponentsInChildren<AudioSource>())
            {
                src.volume = value;
            }
        }

        void shadowsToggleHandler(bool value)
        {
#if !UNITY_IOS
            PlayerPrefs.SetInt("gfx.shadows", value ? 1 : 0);

            Light[] lights = stage.transform.parent.GetComponentsInChildren<Light>(true);
            bool isNight = false;
            if (stage.GetComponentInChildren<Level>() != null)
                isNight = stage.GetComponentInChildren<Level>().isNight;


            if (lights.Length > 1)
            {
                foreach (var light in lights)
                {
                    if (light.gameObject.name.Contains("Realtime"))
                    {
                        light.shadows = value ? LightShadows.Soft : LightShadows.None;                        
                        if (isNight)
                            light.shadows = LightShadows.None;
                    }
                }
            }
#endif
        }

        public override void OnRegister()
        {
            view.ShowCode(false);
            view.ShowLangSelection(false);

            view.onButtonBack.AddListener(homeHandler);
            view.onButtonCode.AddListener(codeHandler);
            view.onButtonCodeClose.AddListener(codeCloseHandler);
            view.onButtonCodeOk.AddListener(codeOkHandler);

            view.onMusicVolume.AddListener(musicVolumeHandler);
            view.onSoundVolume.AddListener(soundVolumeHandler);

            view.onButtonLang.AddListener(langHandler);
            view.onButtonLangClose.AddListener(langCloseHandler);

            view.onLangChoosen.AddListener(langChosenHandler);
            view.onShadowsToggle.AddListener(shadowsToggleHandler);

            view.SetLanguage(localeService.GetCurrentLanguage());
            view.Layout(Screen.width, Screen.height);
            base.OnRegister();
        }




        public override void OnRemove()
        {
            view.onMusicVolume.RemoveListener(musicVolumeHandler);
            view.onSoundVolume.RemoveListener(soundVolumeHandler);

            view.onButtonBack.RemoveListener(homeHandler);
            view.onButtonCode.RemoveListener(codeHandler);
            view.onButtonCodeClose.RemoveListener(codeCloseHandler);
            view.onButtonCodeOk.RemoveListener(codeOkHandler);

            view.onButtonLang.RemoveListener(langHandler);
            view.onButtonLangClose.RemoveListener(langCloseHandler);
            view.onShadowsToggle.RemoveListener(shadowsToggleHandler);

            base.OnRemove();
        }
    }
}