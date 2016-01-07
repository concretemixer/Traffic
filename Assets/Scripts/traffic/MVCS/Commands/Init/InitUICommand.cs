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

            injectionBinder.injector.Inject(uiContainer.GetComponent<UIWatcher>());

            float ratio = (float)Screen.height / (float)Screen.width;

            uiContainer.GetComponent<CanvasScaler>().referenceResolution = new Vector2(960, 960 * ratio);
        }
    }
}