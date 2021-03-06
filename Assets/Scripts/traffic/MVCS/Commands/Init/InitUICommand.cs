using Commons.Resources.Local;
using Commons.UI;
using Commons.Utils;

using UnityEngine.UI;

using strange.extensions.command.impl;
using strange.extensions.injector.api;
using Traffic.Components;
using Traffic.Core;
using UnityEngine;

namespace Traffic.MVCS.Commands.Init
{
    public class InitUICommand : Command
    {
        [Inject]
        public IInjectionBinder binder { private get; set; }

        [Inject(EntryPoint.Container.UI)]
        public GameObject uiContainer { private get; set; }

        public override void Execute()
        {
            Loggr.Log("init UI");

            var manager = new UIManager();
            injectionBinder.injector.Inject(manager);

            manager.Init(uiContainer, new LocalResourceManager());
            binder.Bind<IUIManager>().ToValue(manager);

            var watcher = uiContainer.gameObject.AddComponent<UIWatcher>();
            injectionBinder.injector.Inject(watcher);

            float ratio = (float)Screen.height / (float)Screen.width;

            if (ratio < 1)
                uiContainer.GetComponent<CanvasScaler>().referenceResolution = new Vector2(960, 960 * ratio);
            else
                uiContainer.GetComponent<CanvasScaler>().referenceResolution = new Vector2(960 / ratio, 960);

            manager.Show(UIMap.Id.ScreenLoading);
        }
    }
}