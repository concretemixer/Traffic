using UnityEngine;
using UnityEngine.SceneManagement;
using Commons.UI;
using strange.extensions.command.impl;
using Traffic.Components;
using Traffic.Core;
using Traffic.MVCS.Services;

namespace Traffic.MVCS.Commands
{

	public class InitLevelCommand : Command
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
        public AnalyticsCollector analitics { private get; set; }

        public override void Execute()
        {
            UI.HideAll();

            stageMenu.SetActive(false);

            if (stage.transform.childCount > 0)
                GameObject.Destroy(stage.transform.GetChild(0).gameObject);

            GameObject root = GameObject.FindGameObjectWithTag("Root");
//            SceneManager.MergeScenes(root.scene, SceneManager.GetActiveScene());
		SceneManager.MergeScenes(SceneManager.GetActiveScene(),root.scene);
            root.transform.SetParent(stage.transform);
            root.tag = "Untagged";

            if (root.GetComponentInChildren<TutorialTouchCamera>() != null)
                injectionBinder.injector.Inject(root.GetComponentInChildren<TutorialTouchCamera>());

            var o = GameObject.Find("Level");

            MonoBehaviour[] scripts = o.GetComponents<MonoBehaviour>();
            foreach (var s in scripts)
                injectionBinder.injector.Inject(s);

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

            //analitics.LevelStart(levelIndex);
        }
     

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

