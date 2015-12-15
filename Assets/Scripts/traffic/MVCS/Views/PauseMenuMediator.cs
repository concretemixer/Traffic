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
    public class PauseMenuMediator : Mediator
    {
        [Inject]
        public PauseMenuView view
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
        public LevelResume onResume { get; set; }
                 
        [Inject]
        public SwitchToMainScreenSignal toMainScreenSignal
        {
            get;
            set;
        }

        /*
        [Inject]
        public RetyLevelSignal retyLevelSignal
        {
            get;
            set;
        }
            */
        [Inject]
        public IUIManager UI {
            get;
            set;
        }

        void resumeLevelHandler()
        {
            onResume.Dispatch();
        }

        void homeHandler()
        {
            toMainScreenSignal.Dispatch();
        }

        public override void OnRegister()
        {

            view.onButtonResumeLevel.AddListener(resumeLevelHandler);
            view.onButtonHome.AddListener(homeHandler);
            view.Layout();
            base.OnRegister();
        }




        public override void OnRemove()
        {
            view.onButtonResumeLevel.RemoveListener(resumeLevelHandler);
            view.onButtonHome.RemoveListener(homeHandler);

            base.OnRemove();
        }
    }
}