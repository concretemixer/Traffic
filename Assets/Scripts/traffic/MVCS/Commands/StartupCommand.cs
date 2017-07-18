using UnityEngine;
using Commons.UI;
using strange.extensions.command.impl;
using strange.extensions.context.api;
using Traffic.Components;
using Traffic.Core;
using Traffic.MVCS.Models;
using Commons.Utils;
using System.IO;
using Traffic.MVCS.Services;

namespace Traffic.MVCS.Commands
{
    public class StartupCommand : Command
    {
        [Inject]
        public AnalyticsCollector analitycs { private get; set; }

        [Inject]
        public IUIManager UI { private get; set; }

        [Inject(EntryPoint.Container.StageMenu)]
        public GameObject stageMenu { get; set; }

        public override void Execute()
        {
            analitycs.SetDimentions();
            analitycs.SessionStart();            

            AudioSource gameMusic = GameObject.Find("GameMusic").GetComponent<AudioSource>();
            AudioSource menuMusic = GameObject.Find("MenuMusic").GetComponent<AudioSource>();

            gameMusic.volume = PlayerPrefs.GetFloat("volume.music", 1);
            menuMusic.volume = PlayerPrefs.GetFloat("volume.music", 1);


            AudioSource gameAmbient = GameObject.Find("GameAmbient").GetComponent<AudioSource>();
            gameAmbient.volume = PlayerPrefs.GetFloat("volume.sound", 1);
            gameAmbient.mute = true;

            GameObject.Find("UI Camera").SetActive(false);
            GameObject instance = Object.Instantiate(Resources.Load("levels/level0", typeof(GameObject))) as GameObject;
            instance.transform.SetParent(stageMenu.transform);

            foreach (var go in GameObject.FindGameObjectsWithTag("Respawn"))
            {
                Pitcher pitcher = go.GetComponent<Pitcher>();
                if (pitcher != null)
                {                    
                    pitcher.OnReady();
                }
            }

            /*
            var o = GameObject.Find("Level");

            LevelModel levelModel = injectionBinder.GetInstance<ILevelModel>() as LevelModel;
            injectionBinder.Bind<ILevelModel>().To(levelModel).ToName(GameState.Current);

            MonoBehaviour[] scripts = o.GetComponents<MonoBehaviour>();
            foreach (var s in scripts)
                injectionBinder.injector.Inject(s);

            levelModel.Config = new LevelConfig();

            foreach (var go in GameObject.FindGameObjectsWithTag("Respawn"))
            {
                Pitcher pitcher = go.GetComponent<Pitcher>();
                if (pitcher != null)
                {
                    injectionBinder.injector.Inject(go.GetComponent<Pitcher>());                 
                }
            }
            */

            injectionBinder.injector.Inject(stageMenu.GetComponentInParent<WebDB>());

            UI.Hide(UIMap.Id.ScreenLoading);
            // UI.Show(UIMap.Id.ScreenDebug);
            UI.Show(UIMap.Id.ScreenMain);
            //UI.Show(UIMap.Id.InfoMessage);
        }
    }
}