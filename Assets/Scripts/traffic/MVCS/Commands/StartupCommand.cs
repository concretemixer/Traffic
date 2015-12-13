using Commons.UI;
using UnityEngine.UI;
using strange.extensions.command.impl;
using UnityEngine;

using Traffic.Components;
using Traffic.Core;

namespace Traffic.MVCS.Commands
{
    public class StartupCommand : Command
    {
       // [Inject]
      //  public IUIManager UI { private get; set; }

        [Inject(EntryPoint.Container.UI)]
        public GameObject uiRoot { get; set; }

        public override void Execute()
        {

            //UI.Show(UIMap.Id.ScreenHUD);
        }
    }
}