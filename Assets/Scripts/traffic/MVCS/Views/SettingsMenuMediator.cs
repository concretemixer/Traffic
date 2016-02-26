using Traffic.MVCS.Commands.Signals;
//using traffic.MVCS.model;
//using traffic.MVCS.model.level;
using Traffic.MVCS.Views.UI.HUD;
using Traffic.MVCS.Models;
using Traffic.Core;
using Commons.UI;
using Commons.Utils;
using UnityEngine;

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
        public SwitchToStartScreenSignal toStartScreenSignal
        {
            get;
            set;
        }

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

        void homeHandler()
        {
            if (Time.timeScale == 0)
                onPause.Dispatch();
            else
                toStartScreenSignal.Dispatch();
        }

        void musicVolumeHandler(float value)
        {
            PlayerPrefs.SetFloat("volume.music",value);

            AudioSource gameMusic = GameObject.Find("GameMusic").GetComponent<AudioSource>();
            AudioSource menuMusic = GameObject.Find("MenuMusic").GetComponent<AudioSource>();

            gameMusic.volume = value;
            menuMusic.volume = value;
        }

        void soundVolumeHandler(float value)
        {
            PlayerPrefs.SetFloat("volume.sound", value);

            GameObject snd = GameObject.Find("AmbientSound");
            if (snd != null)
            {
                snd.GetComponent<AudioSource>().volume = value;
            }
        }

        public override void OnRegister()
        {
            view.ShowCode(false);
            view.onButtonBack.AddListener(homeHandler);
            view.onButtonCode.AddListener(codeHandler);
            view.onButtonCodeClose.AddListener(codeCloseHandler);

            view.onMusicVolume.AddListener(musicVolumeHandler);
            view.onSoundVolume.AddListener(soundVolumeHandler);

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
            base.OnRemove();
        }
    }
}