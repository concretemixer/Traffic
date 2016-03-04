using Traffic.MVCS.Commands.Signals;
//using traffic.MVCS.model;
//using traffic.MVCS.model.level;
using Traffic.MVCS.Views.UI.HUD;
using Traffic.MVCS.Models;
using Traffic.Core;
using Commons.UI;
using Commons.Utils;
using UnityEngine;

using strange.extensions.mediation.impl;

namespace Traffic.MVCS.Views.UI
{
    public class InfoMesageMediator : Mediator
    {
        [Inject]
        public InfoMessageView view
        {
            get;
            set;
        }

                 
        [Inject]
        public SwitchToStartScreenSignal toStartScreenSignal
        {
            get;
            set;
        }

        [Inject]
        public LevelPause onPause { get; set; }
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


        void closeHandler()
        {
            Loggr.Log("here");
            view.Show(false);
        }


        public override void OnRegister()
        {
            view.onButtonOk.AddListener(closeHandler);
            view.onButtonClose.AddListener(closeHandler);

            view.Layout(Screen.width, Screen.height);
            
            base.OnRegister();
        }




        public override void OnRemove()
        {
            view.onButtonOk.RemoveListener(closeHandler);
            view.onButtonClose.RemoveListener(closeHandler);                        

            base.OnRemove();
        }
    }
}