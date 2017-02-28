using UnityEngine;
using UnityEngine.SceneManagement;
using Commons.UI;
using strange.extensions.command.impl;
using strange.extensions.context.api;
using Traffic.Components;
using Traffic.Core;
using Traffic.MVCS.Models;
using Commons.Utils;
using System.IO;
using System.Collections;
using Traffic.MVCS.Services;

namespace Traffic.MVCS.Commands
{

	public class StartLevelCommand : Command
	{
		[Inject]
		public IUIManager UI { private get; set; }

		[Inject(EntryPoint.Container.Stage)]
		public  GameObject stage { get  ; set ;}

        [Inject(EntryPoint.Container.StageMenu)]
        public GameObject stageMenu { get; set; }

        [Inject]
        public ILevelListModel levels { get; set; }

		[Inject]
		public int levelIndex{get;set;}

        [Inject]
        public AnalyticsCollector analitics { private get; set; }


        private void Setup()
        {
            var o = GameObject.Find("Level");

            LevelModel levelModel = injectionBinder.GetInstance<ILevelModel>() as LevelModel;

            MonoBehaviour[] scripts = o.GetComponents<MonoBehaviour>();
            foreach (var s in scripts)
                injectionBinder.injector.Inject(s);

            levelModel.Config = levels.LevelConfigs[levelIndex];
            levelModel.LevelIndex = levelIndex;

            foreach (var go in GameObject.FindGameObjectsWithTag("Respawn"))
            {
                Pitcher pitcher = go.GetComponent<Pitcher>();
                if (pitcher != null)
                {
                    injectionBinder.injector.Inject(go.GetComponent<Pitcher>());
                    pitcher.OnReady();

                    // if (levelModel.Config.pitchers.ContainsKey(go.name) && false)
                    // {
                    //     pitcher.Pause = levelModel.Config.pitchers[go.name].startDelay;
                    //     pitcher.IntervalMax = levelModel.Config.pitchers[go.name].intervalMax;
                    //     pitcher.IntervalMin = levelModel.Config.pitchers[go.name].intervalMin;
                    // }
                }
            }


            foreach (var go in GameObject.FindGameObjectsWithTag("Finish"))
            {
                MeshRenderer r = go.GetComponent<MeshRenderer>();
                if (r != null)
                {
                    r.enabled = false;
                }
            }

            AudioSource menuMusic = GameObject.Find("MenuMusic").GetComponent<AudioSource>();
            if (menuMusic.isPlaying)
            {
                menuMusic.Stop();
                AudioSource gameMusic = GameObject.Find("GameMusic").GetComponent<AudioSource>();
                gameMusic.Play();
            }


            AudioSource gameAmbient = GameObject.Find("GameAmbient").GetComponent<AudioSource>();
            AmbientTrackList gameAmbientTracks = GameObject.Find("GameAmbient").GetComponent<AmbientTrackList>();

            gameAmbient.clip = null;// gameAmbientTracks.Tracks[levelIndex];
            float musicVolume = PlayerPrefs.GetFloat("volume.music", 1);
            gameAmbient.mute = true;// musicVolume > 0;
            //gameAmbient.Play();

            float soundVolume = PlayerPrefs.GetFloat("volume.sound", 1);
            foreach (AudioSource src in stage.GetComponentsInChildren<AudioSource>())
            {
                src.volume = soundVolume;
            }

            UI.Show(UIMap.Id.ScreenHUD);

            analitics.LevelStart(levelIndex);
        }

        public override void Execute()
		{

            Time.timeScale = 0.85f;

			safeUnbind<ILevelModel>(GameState.Current);


            LevelModel levelModel = injectionBinder.GetInstance<ILevelModel>() as LevelModel;
            injectionBinder.Bind<ILevelModel>().To(levelModel).ToName(GameState.Current);

	        Debug.Log("LVL: "+levels.LevelNames[levelIndex]);

            levelModel.Config = levels.LevelConfigs[levelIndex];
            levelModel.LevelIndex = levelIndex;

                SceneManager.LoadScene(levels.LevelNames[levelIndex], LoadSceneMode.Additive);                
	}
        /*
        public override void Execute()
        {
            UI.Hide(UIMap.Id.ScreenHUD);
            UI.Hide(UIMap.Id.LevelFailedMenu);
            UI.Hide(UIMap.Id.LevelDoneMenu);
            UI.Hide(UIMap.Id.PauseMenu);
            UI.Hide(UIMap.Id.LevelListScreen);

            safeUnbind<ILevelModel>(GameState.Current);

            if (stage.transform.childCount > 0)
                GameObject.Destroy(stage.transform.GetChild(0).gameObject);


            GameplayConfig levelCfgs = new GameplayConfig();

            for (int a=0;a<levels.LevelNames.Length;a++) {
                GameObject instance = Object.Instantiate(Resources.Load("levels/" + levels.LevelNames[a], typeof(GameObject))) as GameObject;

                LevelConfig cfg = new LevelConfig();

                cfg.threeStarsScore = 10000;
                cfg.twoStarsScore = 5000;

                cfg.target = instance.GetComponentInChildren<Level>().targetScore;

                foreach (var pitcher in instance.transform.GetComponentsInChildren<Pitcher>())
                {                    
                    PitcherConfig p = new PitcherConfig();
                    p.startDelay = pitcher.Pause;
                    p.intervalMin = pitcher.IntervalMin;
                    p.intervalMax = pitcher.IntervalMax;

                    cfg.pitchers.Add(pitcher.gameObject.name, p);
                }

                GameObject.Destroy(instance);

                levelCfgs.levels[a+1] = cfg;
                
            }

            string data = JsonWriter.Serialize(levelCfgs);
            var streamWriter = new StreamWriter("out.json");
            streamWriter.Write(data);
            streamWriter.Close();
           
        }
        */

		void safeUnbind<T>()
		{
			var binding = injectionBinder.GetBinding<T>();
			if (binding != null)
				injectionBinder.Unbind<T>();
		}
		
		void safeUnbind<T>(object name)
		{
			var binding = injectionBinder.GetBinding<T>(name);
			if (binding != null)
				injectionBinder.Unbind<T>(name);
		}
	}
}

