using strange.extensions.injector.api;
using Traffic.Components;
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
            Logger.Log("init UI");

            var manager = new UIManager();
            manager.Init(uiContainer);
            binder.Bind<IUIManager>().ToValue(manager);
        }
    }
}