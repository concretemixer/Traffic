using UnityEngine;
using Commons.UI;
using strange.extensions.command.impl;
using strange.extensions.context.api;
using Traffic.Components;
using Traffic.Core;
using Traffic.MVCS.Models;
using Commons.Utils;
using Traffic.MVCS.Views.UI;
using Traffic.MVCS.Services;


namespace Traffic.MVCS.Commands
{
	public class TutorialPointCommand : Command
	{
		[Inject]
		public IUIManager UI { private get; set; }

		[Inject(EntryPoint.Container.Stage)]
		public  GameObject stage { get  ; set ;}

        [Inject]
        public int Point { get; set; }


        [Inject]
        public AnalyticsCollector analytics { private get; set; }

		public override void Execute()
		{
            analytics.LogTutorialStep((TutorialStep)Point);

            Time.timeScale = 0.0f;
            TutorialStepScreen screen = UI.Show<TutorialStepScreen>(UIMap.Id.ScreenTutorial);
            screen.SetStep(Point);

            foreach (var camera in stage.GetComponentsInChildren<TutorialTouchCamera>())
            {
                injectionBinder.injector.Inject(camera);
            }
		}


	}
}

