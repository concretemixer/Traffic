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
    public class StartGameScreenMediator : Mediator
    {
        [Inject]
        public StartGameScreenView view
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

        void homeHandler()
        {
            toMainScreenSignal.Dispatch();
        }

        void optionsHandler()
        {            
        }

        void shopsHandler()
        {
        }

        void connectHandler()
        {
        }


        public override void OnRegister()
        {          
            view.onButtonStart.AddListener(homeHandler);
            view.onButtonOptions.AddListener(optionsHandler);
            view.onButtonConnect.AddListener(connectHandler);
            view.onButtonShop.AddListener(shopsHandler);
            view.Layout();

            base.OnRegister();
        }




        public override void OnRemove()
        {
            view.onButtonStart.RemoveListener(homeHandler);
            view.onButtonOptions.RemoveListener(optionsHandler);
            view.onButtonConnect.RemoveListener(connectHandler);
            view.onButtonShop.RemoveListener(shopsHandler);
          
            base.OnRemove();
        }
    }
}