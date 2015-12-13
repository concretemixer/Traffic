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
    public class LevelFailedMenuMediator : Mediator
    {


        [Inject]
        public LevelFailedMenuView view
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
        public LevelRetry onRetry { get; set; }


                  
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

        void retryLevelHandler()
        {            
            onRetry.Dispatch();
        }

        void homeHandler()
        {
            toMainScreenSignal.Dispatch();
        }

        public override void OnRegister()
        {

            view.onButtonRetryLevel.AddListener(retryLevelHandler);
            view.onButtonHome.AddListener(homeHandler);

            base.OnRegister();
        }




        public override void OnRemove()
        {
            view.onButtonRetryLevel.RemoveListener(retryLevelHandler);
            view.onButtonHome.RemoveListener(homeHandler);

            base.OnRemove();
        }
    }
}