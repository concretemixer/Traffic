using strange.extensions.command.api;
using strange.extensions.command.impl;
using strange.extensions.context.impl;
using Traffic.MVCS.Commands.Signals;
using Traffic.Components;
using UnityEngine;
using Traffic.MVCS.Commands.Init;

namespace Traffic.MVCS
{
    public class AppContext : MVCSContext
    {
        EntryPoint entryPoint;

        public AppContext(EntryPoint _view) : base(_view, true)
        {
            entryPoint = _view;
        }

        /// remap command binder to signals
        protected override void addCoreComponents()
        {
            // up signals
            base.addCoreComponents();
            injectionBinder.Unbind<ICommandBinder>();
            injectionBinder.Bind<ICommandBinder>().To<SignalCommandBinder>().ToSingleton();
        }

        public override void Launch()
        {
            injectionBinder.GetInstance<StartupSignal>().Dispatch();
        }

        protected override void mapBindings()
        {
            mapCommands();
            mapSignals();
            mapModels();
            mapStageMediators();
            mapUIMediators();
            mapOthers();
        }

        void mapCommands()
        {
            // init commands
            commandBinder.Bind<StartupSignal>().InSequence()
                .To<LoadConfigCommand>()
                .To<InitUICommand>();

            // commandBinder.Bind<StartupSignal>().To<StartupCommand>();
            // commandBinder.Bind<StartLevelSignal>().To<StartLevelCommand>();
            // commandBinder.Bind<SwitchToMainScreenSignal>().InSequence()
            // .To<CleanGameContainersCommand>()
            // .To<SwitchToMainScreenCommand>();

            // commandBinder.Bind<LevelFailed>().InSequence()
            // .To<LevelFailedCommand>()
            // .To<CleanGameContainersCommand>()
            // .To<SwitchToMainScreenCommand>();

            // commandBinder.Bind<LevelComplete>().InSequence()
            // .To<LevelCompleteCommand>()
            // .To<CleanGameContainersCommand>()
            // .To<SwitchToMainScreenCommand>();

            // commandBinder.Bind<RetyLevelSignal>().InSequence()
            // .To<CleanGameContainersCommand>()
            // .To<StartLevelCommand>();
        }

        void mapSignals()
        {
            // injectionBinder.Bind<PinElementSignal>().ToSingleton();
            // injectionBinder.Bind<UnpinElementSignal>().ToSingleton();
            // injectionBinder.Bind<EliminateElementsSignal>().ToSingleton();
            // injectionBinder.Bind<AddElementsSignal>().ToSingleton();
            // injectionBinder.Bind<MoveElementsSignal>().ToSingleton();
        }

        void mapModels()
        {
            // injectionBinder.Bind<RandomProxy>().To(new RandomProxy(123));
            // injectionBinder.Bind<IElementGenerator>().To<RandomElementGenerator>().ToSingleton();
            // injectionBinder.Bind<ILevelModel>().To<LevelModel>();
            // injectionBinder.Bind<IChainModel>().To<ChainModel>();

            // injectionBinder.Bind<IElementsDefModel>().To<ElementsDefModel>().ToSingleton();
            // injectionBinder.Bind<ILevelListModel>().To<LevelListModel>().ToSingleton();
        }

        void mapStageMediators()
        {
            // mediationBinder.Bind<LevelView>().To<LevelMediator>();
        }

        void mapUIMediators()
        {
            // mediationBinder.Bind<ScreenMainView>().To<ScreenMainMediator>();
            // mediationBinder.Bind<ScreenHUDView>().To<ScreenHUDMediator>();
        }

        void mapOthers()
        {
            injectionBinder.Bind<GameObject>().To(entryPoint.Stage).ToName(EntryPoint.Container.Stage);
            injectionBinder.Bind<GameObject>().To(entryPoint.UI).ToName(EntryPoint.Container.UI);

            // injectionBinder.Bind<UIMap>().ToSingleton();
            // injectionBinder.Bind<UIManager>().ToSingleton();
        }
    }
}
