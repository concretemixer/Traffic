using Traffic.MVCS.Commands.Signals;
//using traffic.MVCS.model;
//using traffic.MVCS.model.level;
using Traffic.MVCS.Views.UI.HUD;
using Traffic.MVCS.Models;
using Traffic.Core;
using Commons.UI;
using Commons.Utils;
using UnityEngine;
using Traffic.MVCS.Services;
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

        [Inject]
        public SwitchToSettingsScreenSignal toSettingsSignal
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

        [Inject]
        public AnalyticsCollector analitycs { private get; set; }

        void resumeLevelHandler()
        {
            onResume.Dispatch();
        }

        void homeHandler()
        {
            analitycs.LevelResult(level.LevelIndex, "quit");
            toMainScreenSignal.Dispatch();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                resumeLevelHandler();
            }
        }


        void settingsHandler()
        {
            toSettingsSignal.Dispatch();
        }

        public override void OnRegister()
        {

            view.onButtonResumeLevel.AddListener(resumeLevelHandler);
            view.onButtonHome.AddListener(homeHandler);
            view.onButtonSettings.AddListener(settingsHandler);
            view.Layout(Screen.width, Screen.height);
            base.OnRegister();
        }




        public override void OnRemove()
        {
            view.onButtonSettings.RemoveListener(settingsHandler);
            view.onButtonResumeLevel.RemoveListener(resumeLevelHandler);
            view.onButtonHome.RemoveListener(homeHandler);

            base.OnRemove();
        }
    }
}