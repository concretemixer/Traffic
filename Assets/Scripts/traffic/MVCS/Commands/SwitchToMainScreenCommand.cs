using UnityEngine;
using Commons.UI;
using strange.extensions.command.impl;
using strange.extensions.context.api;
using Traffic.Components;
using Traffic.Core;
using Traffic.MVCS.Models;
using Commons.Utils;

namespace Traffic.MVCS.Commands
{
    public class SwitchToMainScreenCommand : Command
	{
		[Inject]
		public IUIManager UI { private get; set; }

		[Inject(EntryPoint.Container.Stage)]
		public  GameObject stage { get  ; set ;}

        [Inject(EntryPoint.Container.StageMenu)]
        public GameObject stageMenu { get; set; }


		public override void Execute()
        {
            UI.HideAll();

            safeUnbind<ILevelModel>(GameState.Current);

            if (stage.transform.childCount > 0)
                GameObject.Destroy(stage.transform.GetChild(0).gameObject);
            Time.timeScale = 1;

            AudioSource gameMusic = GameObject.Find("GameMusic").GetComponent<AudioSource>();
            AudioSource menuMusic = GameObject.Find("MenuMusic").GetComponent<AudioSource>();
            AudioSource gameAmbient = GameObject.Find("GameAmbient").GetComponent<AudioSource>();

            gameAmbient.mute = true;

            stageMenu.SetActive(true);

            if (!menuMusic.isPlaying)
            {
                menuMusic.Play();
                gameMusic.Stop();
            }

            UI.Show(UIMap.Id.LevelListScreen);
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

