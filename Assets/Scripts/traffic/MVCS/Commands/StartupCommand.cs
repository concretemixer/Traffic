using Commons.UI;
using strange.extensions.command.impl;
using UnityEngine;
using UnityEngine.UI;
using Traffic.Components;
using Traffic.Core;

namespace Traffic.MVCS.Commands
{
    public class StartupCommand : Command
    {
        [Inject]
        public UIManager UI { private get; set; }

        [Inject(EntryPoint.Container.UI)]
        public GameObject uiRoot { get; set; }

        public override void Execute()
        {
            injectionBinder.injector.Inject(uiRoot.GetComponent<UIWatcher>());

            float ratio = (float)Screen.height / (float)Screen.width;

            uiRoot.GetComponent<CanvasScaler>().referenceResolution = new Vector2(960, 960 * ratio);

            UI.Show(UIMap.Id.LevelListScreen);
            //UI.Show(UIMap.Id.ScreenHUD);
        }
    }
}