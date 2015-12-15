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
    public class LevelListScreenMediator : Mediator
    {



        [Inject]
        public LevelListScreenView view
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
       
        [Inject]
        public IUIManager UI {
            get;
            set;
        }

        [Inject]
        public StartLevelSignal startLevel { get; set; }

        int page = 0;

        void switchPageHandler()
        {
            page = 1 - page;
            view.SetPage(page, levels);
        }

        void startLevelHandler(int index)
        {
            Logger.Log(index.ToString());
            startLevel.Dispatch(index);
        }
        
        public override void OnRegister()
        {
            view.onButtonNext.AddListener(switchPageHandler);
            view.onButtonPrev.AddListener(switchPageHandler);
            view.onButtonLevel.AddListener(startLevelHandler);

            view.Layout();
            view.SetPage(page, levels);


            base.OnRegister();
        }




        public override void OnRemove()
        {
            view.onButtonNext.RemoveListener(switchPageHandler);
            view.onButtonPrev.RemoveListener(switchPageHandler);
            view.onButtonLevel.RemoveListener(startLevelHandler);
            base.OnRemove();
        }
    }
}