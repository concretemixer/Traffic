using strange.extensions.command.api;
using strange.extensions.command.impl;
using strange.extensions.context.impl;
using Traffic.MVCS.Commands.Signals;
using Traffic.Components;
using UnityEngine;
using Traffic.Core;
using Traffic.MVCS.Commands.Init;
using Traffic.MVCS.Commands;
using Traffic.MVCS.Views.UI.HUD;
using Traffic.MVCS.Views.UI;
using Traffic.MVCS.Views.Game;
using Traffic.MVCS.Views.UI.Loading;

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
                .To<CreateServiceItemsCommand>()
                .To<LoadConfigCommand>()
                .To<InitUICommand>()
                .To<TryToLoginFBCommand>()
                .To<StartupCommand>();

            commandBinder.Bind<StartLevelSignal>().To<StartLevelCommand>();
            commandBinder.Bind<LevelPause>().To<PauseLevelCommand>();
            commandBinder.Bind<LevelResume>().To<ResumeLevelCommand>();
            commandBinder.Bind<LevelRetry>().To<RetryLevelCommand>();
            commandBinder.Bind<TutorialPoint>().To<TutorialPointCommand>();

            commandBinder.Bind<SwitchToMainScreenSignal>()
               .To<SwitchToMainScreenCommand>();
        }

        void mapSignals()
        {
            injectionBinder.Bind<OrientationChangedSignal>().ToSingleton();
            injectionBinder.Bind<VehicleReachedDestination>().ToSingleton();
            injectionBinder.Bind<VehicleCrashed>().ToSingleton();
            injectionBinder.Bind<LevelFailed>().ToSingleton();
            injectionBinder.Bind<LevelComplete>().ToSingleton();
            injectionBinder.Bind<ScoreGrow>().ToSingleton();
        }

        void mapModels()
        {
            injectionBinder.Bind<ILevelModel>().To<LevelModel>();
            injectionBinder.Bind<ILevelListModel>().To<LevelListModel>().ToSingleton();
        }

        void mapStageMediators()
        {
            mediationBinder.Bind<LevelView>().To<LevelMediator>();
        }

        void mapUIMediators()
        {
            mediationBinder.Bind<LevelListScreenView>().To<LevelListScreenMediator>();
            mediationBinder.Bind<LevelFailedMenuView>().To<LevelFailedMenuMediator>();
            mediationBinder.Bind<LevelDoneMenuView>().To<LevelDoneMenuMediator>();
            mediationBinder.Bind<PauseMenuView>().To<PauseMenuMediator>();
            mediationBinder.Bind<ScreenHUDView>().To<ScreenHUDMediator>();
            mediationBinder.Bind<LoadingScreenView>().To<LoadingScreenMediator>();
            mediationBinder.Bind<TutorialStepScreen>().To<TutorialStepMediator>();
        }

        void mapOthers()
        {
            injectionBinder.Bind<GameObject>().To(entryPoint.Stage).ToName(EntryPoint.Container.Stage);
            injectionBinder.Bind<GameObject>().To(entryPoint.UI).ToName(EntryPoint.Container.UI);
        }
    }
}
