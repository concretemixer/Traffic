using UnityEngine;
using Commons.UI;
using strange.extensions.command.impl;
using strange.extensions.context.api;
using Traffic.Components;
using Traffic.Core;
using Traffic.MVCS.Models;
using Commons.Utils;
using System.IO;

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


        
		public override void Execute()
		{
            UI.Hide(UIMap.Id.ScreenHUD);
            UI.Hide(UIMap.Id.LevelFailedMenu);
            UI.Hide(UIMap.Id.LevelDoneMenu);
            UI.Hide(UIMap.Id.PauseMenu);
            UI.Hide(UIMap.Id.LevelListScreen);

			safeUnbind<ILevelModel>(GameState.Current);

            if (stage.transform.childCount>0)
                GameObject.Destroy(stage.transform.GetChild(0).gameObject);

            stageMenu.SetActive(false);

            GameObject instance = Object.Instantiate(Resources.Load("levels/" + levels.LevelNames[levelIndex], typeof(GameObject))) as GameObject;
			instance.transform.SetParent(stage.transform);

			var o = GameObject.Find ("Level");
                   
			LevelModel levelModel = injectionBinder.GetInstance<ILevelModel>() as LevelModel;	
			injectionBinder.Bind<ILevelModel>().To(levelModel).ToName(GameState.Current);

            MonoBehaviour[] scripts = o.GetComponents<MonoBehaviour>(); 
            foreach(var s in scripts)
                injectionBinder.injector.Inject(s);

            levelModel.Config = levels.LevelConfigs[levelIndex];            

            foreach (var go in GameObject.FindGameObjectsWithTag("Respawn"))
            {
                Pitcher pitcher = go.GetComponent<Pitcher>();
                if (pitcher != null)
                {
                    injectionBinder.injector.Inject(go.GetComponent<Pitcher>());

                    // if (levelModel.Config.pitchers.ContainsKey(go.name) && false)
                    // {
                    //     pitcher.Pause = levelModel.Config.pitchers[go.name].startDelay;
                    //     pitcher.IntervalMax = levelModel.Config.pitchers[go.name].intervalMax;
                    //     pitcher.IntervalMin = levelModel.Config.pitchers[go.name].intervalMin;
                    // }
                }
            }

            AudioSource menuMusic = GameObject.Find("MenuMusic").GetComponent<AudioSource>();
            if (menuMusic.isPlaying) {
                menuMusic.Stop();
                AudioSource gameMusic = GameObject.Find("GameMusic").GetComponent<AudioSource>();
                gameMusic.Play();
            }

			UI.Show(UIMap.Id.ScreenHUD);				
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

