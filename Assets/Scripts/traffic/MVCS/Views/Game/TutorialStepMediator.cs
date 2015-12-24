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
    public class TutorialStepMediator : Mediator
    {
        [Inject]
        public TutorialStepScreen view
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

        void nextStepHandler()
        {
        }

        void homeHandler()
        {
            toMainScreenSignal.Dispatch();
        }

        public override void OnRegister()
        {

            view.onButtonNextStep.AddListener(nextStepHandler);
            
            view.Layout();

            base.OnRegister();
        }




        public override void OnRemove()
        {
            view.onButtonNextStep.RemoveListener(nextStepHandler);
            


            base.OnRemove();
        }
    }
}