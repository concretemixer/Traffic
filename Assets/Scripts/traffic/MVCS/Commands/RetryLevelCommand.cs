using UnityEngine;
using Commons.UI;
using strange.extensions.command.impl;
using strange.extensions.context.api;
using Traffic.Components;
using Traffic.Core;
using Traffic.MVCS.Models;
using Commons.Utils;
using Traffic.MVCS.Services;

namespace Traffic.MVCS.Commands
{
	public class RetryLevelCommand : Command
	{
		[Inject]
		public IUIManager UI { private get; set; }

		[Inject(EntryPoint.Container.Stage)]
		public  GameObject stage { get  ; set ;}

        [Inject(GameState.Current)]
        public ILevelModel level
        {
            get;
            set;
        }

        [Inject]
        public ILevelListModel levels
        {
            get;
            set;
        }

        public override void Execute()
		{
            level.Score = 0;
            level.Progress = 0;
            level.Failed = false;

            foreach (var go in GameObject.FindGameObjectsWithTag("VehicleAI"))
            {
                GameObject.Destroy(go);
            }

            foreach (var go in GameObject.FindGameObjectsWithTag("Vehicle"))
            {
                GameObject.Destroy(go);
            }

            foreach (var go in GameObject.FindGameObjectsWithTag("Respawn"))
            {
                if (go.GetComponent<Pitcher>()!=null)
                    go.GetComponent<Pitcher>().Reset();
            }

            foreach(var scenario in stage.GetComponentsInChildren<TutorialScenarioBase>())
            {
                scenario.Reset();
            }

            Time.timeScale = 0.85f;
            //Logger.Log("Restart");
            UI.Show(UIMap.Id.ScreenHUD);	
            UI.Hide(UIMap.Id.LevelFailedMenu);
            UI.Hide(UIMap.Id.TutorialFailedMenu);

            
            WebDB webDB = stage.GetComponentInParent<WebDB>();
            if (webDB != null)
            {
                webDB.TryLevel(levels.CurrentLevelIndex);
            }
        }
	}
}

