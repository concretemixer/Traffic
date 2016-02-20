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


		public override void Execute()
        {
            UI.Hide(UIMap.Id.ScreenHUD);
            UI.Hide(UIMap.Id.LevelFailedMenu);
            UI.Hide(UIMap.Id.LevelDoneMenu);
            UI.Hide(UIMap.Id.PauseMenu);
            UI.Hide(UIMap.Id.LevelListScreen);
            UI.Hide(UIMap.Id.ScreenMain);
            UI.Hide(UIMap.Id.ScreenTutorial);

            safeUnbind<ILevelModel>(GameState.Current);

            if (stage.transform.childCount > 0)
                GameObject.Destroy(stage.transform.GetChild(0).gameObject);
            Time.timeScale = 1;

            AudioSource gameMusic = GameObject.Find("GameMusic").GetComponent<AudioSource>();
            AudioSource menuMusic = GameObject.Find("MenuMusic").GetComponent<AudioSource>();


            {
                GameObject cameraMain = GameObject.Find("UI Camera");
                cameraMain.GetComponent<Camera>().enabled = true;
                cameraMain.GetComponent<AudioListener>().enabled = true;
            }

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

