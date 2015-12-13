using Traffic.MVCS.Commands.Signals;
//using traffic.MVCS.model;
//using traffic.MVCS.model.level;
using Traffic.MVCS.Views.UI.HUD;
using Traffic.MVCS.Models;
using Traffic.Core;
using Commons.UI;
using Commons.Utils;

using strange.extensions.mediation.impl;

namespace Traffic.MVCS.Views.UI
{
    public class LevelDoneMenuMediator : Mediator
    {
        [Inject]
        public LevelDoneMenuView view
        {
            get;
            set;
        }

        [Inject]
        public ILevelListModel levels
        {
            get;
            set;
        }

        [Inject(GameState.Current)]
        public ILevelModel level
        {
            get;
            set;
        }

        [Inject]
        public LevelRetry onResume { get; set; }

        [Inject]
        public StartLevelSignal startLevel
        {
            get;
            set;
        }

        [Inject]
        public SwitchToMainScreenSignal toMainScreenSignal
        {
            get;
            set;
        }

        [Inject]
        public IUIManager UI {
            get;
            set;
        }

        void nextLevelHandler()
        {
            if (levels.GetLevelState(levels.CurrentLevelIndex + 1) == LevelState.NoLevel)
                toMainScreenSignal.Dispatch();
            else
            {
                levels.CurrentLevelIndex++;
                startLevel.Dispatch(levels.CurrentLevelIndex);
            }
        }

        void homeHandler()
        {
            toMainScreenSignal.Dispatch();
        }

        public override void OnRegister()
        {

            view.onButtonNextLevel.AddListener(nextLevelHandler);
            view.onButtonHome.AddListener(homeHandler);


            base.OnRegister();
        }




        public override void OnRemove()
        {
            view.onButtonNextLevel.RemoveListener(nextLevelHandler);
            view.onButtonHome.RemoveListener(homeHandler);


            base.OnRemove();
        }
    }
}