using Commons.Utils;
using Commons.UI;
using strange.extensions.command.impl;
using UnityEngine;
using Traffic.Components;

namespace Traffic.MVCS.Commands.Init
{
	public class InitUICommand : Command
	{
		[Inject]
		public UIManager UI { private get; set; }

		[Inject(EntryPoint.Container.UI)]
		public  GameObject UIroot { get  ; set ;}

		public override void Execute()
		{
			Logger.Log ("init ui");
			UI.Init (UIroot);
		}
	}
}