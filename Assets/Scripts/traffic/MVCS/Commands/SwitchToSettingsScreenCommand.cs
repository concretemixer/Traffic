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
    public class SwitchToSettingsScreenCommand : Command
	{
		[Inject]
		public IUIManager UI { private get; set; }

		[Inject(EntryPoint.Container.Stage)]
		public  GameObject stage { get  ; set ;}


		public override void Execute()
        {
            UI.Hide(UIMap.Id.PauseMenu);
            UI.Hide(UIMap.Id.ScreenMain);

            if (Time.timeScale==0)
                UI.Show(UIMap.Id.ScreenSettingsIngame);
            else
                UI.Show(UIMap.Id.ScreenSettings);
        }
	}
}

