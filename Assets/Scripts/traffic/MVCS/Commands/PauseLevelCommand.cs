using UnityEngine;
using Commons.UI;
using strange.extensions.command.impl;
using Traffic.Components;

namespace Traffic.MVCS.Commands
{
	public class PauseLevelCommand : Command
	{
		[Inject]
		public IUIManager UI { private get; set; }

		[Inject(EntryPoint.Container.Stage)]
		public  GameObject stage { get  ; set ;}


		public override void Execute()
		{
			Time.timeScale=0;
            UI.Hide(UIMap.Id.ScreenSettings);            
            UI.Show(UIMap.Id.PauseMenu);	
		}
	}
}

