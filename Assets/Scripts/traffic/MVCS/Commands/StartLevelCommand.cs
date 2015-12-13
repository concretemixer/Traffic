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
	public class StartLevelCommand : Command
	{
		[Inject]
		public IUIManager UI { private get; set; }

		[Inject(EntryPoint.Container.Stage)]
		public  GameObject stage { get  ; set ;}

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

            GameObject instance = Object.Instantiate(Resources.Load("levels/" + levels.LevelNames[levelIndex], typeof(GameObject))) as GameObject;
			instance.transform.SetParent(stage.transform);

			var o = GameObject.Find ("Level");
                   
			LevelModel levelModel = injectionBinder.GetInstance<ILevelModel>() as LevelModel;	
			injectionBinder.Bind<ILevelModel>().To(levelModel).ToName(GameState.Current);

            injectionBinder.injector.Inject(o.GetComponent<Level>());
            levelModel.Target = o.GetComponent<Level>().targetScore;

            foreach (var go in GameObject.FindGameObjectsWithTag("Respawn"))
            {
                if (go.GetComponent<Pitcher>()!=null)
                    injectionBinder.injector.Inject(go.GetComponent<Pitcher>());
            }

			UI.Show(UIMap.Id.ScreenHUD);				
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

