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
	public class NextLevelCommand : Command
	{
		[Inject]
		public UIManager UI { private get; set; }

		[Inject(EntryPoint.Container.Stage)]
		public  GameObject stage { get  ; set ;}

		[Inject]
		public string levelName{get;set;}

		public override void Execute()
		{
			safeUnbind<ILevelModel>(GameState.Current);

			GameObject instance = Object.Instantiate(Resources.Load("levels/"+levelName, typeof(GameObject))) as GameObject;
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

