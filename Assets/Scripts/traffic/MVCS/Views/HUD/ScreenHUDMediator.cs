using Traffic.MVCS.Commands.Signals;
//using traffic.MVCS.model;
//using traffic.MVCS.model.level;
using Traffic.MVCS.Views.UI.HUD;
using Traffic.MVCS.Models;
using Traffic.Core;
using Commons.UI;
using Commons.Utils;

using strange.extensions.mediation.impl;

namespace Traffic.MVCS.Views.UI.HUD
{
    public class ScreenHUDMediator : Mediator
    {
        [Inject]
        public ScreenHUDView view
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
        public OrientationChangedSignal onOrientationChanged { get; set; }

		[Inject]
		public VehicleReachedDestination onVehicleReachedDestination { get; set;}

		[Inject]
		public LevelPause onPause { get; set;}

		[Inject]
		public LevelResume onResume { get; set;}

		/*
        [Inject]
        public SwitchToMainScreenSignal toMainScreenSignal
        {
            get;
            set;
        }

        [Inject]
        public RetyLevelSignal retyLevelSignal
        {
            get;
            set;
        }
*/
        [Inject]
        public IUIManager ui {
            get;
            set;
        }
			


        public override void OnRegister()
		{
			onVehicleReachedDestination.AddListener(vehicleReachHandler);
			view.onButtonPauseLevel.AddListener(pauseLevelHandler);
          //  view.onRetyLevel.AddListener(retyLevelHandler);
            onOrientationChanged.AddListener(orientationChangedHandler);

            view.Layout();
            updateLevelProgress();

            base.OnRegister();
        }

        void updateLevelProgress()
        {
             view.SetScore(level.Score);			
             view.SetProgress(level.Progress, level.Target);
        }

        void exitLevelHandler()
        {
        }

		void pauseLevelHandler()
		{
			view.onButtonPauseLevel.RemoveListener(pauseLevelHandler);
			onPause.Dispatch ();
			view.onButtonPauseLevel.AddListener(resumeLevelHandler);
		}

		void resumeLevelHandler()
		{
			view.onButtonPauseLevel.RemoveListener(resumeLevelHandler);
			onResume.Dispatch ();
			view.onButtonPauseLevel.AddListener(pauseLevelHandler);
		}
		
		void retyLevelHandler()
        {
        }

		void vehicleReachHandler()
		{
            updateLevelProgress();
        }

        void orientationChangedHandler()
        {
            view.Layout();
        }

        public override void OnRemove()
        {
			onVehicleReachedDestination.RemoveListener(vehicleReachHandler);
			view.onButtonPauseLevel.RemoveListener(resumeLevelHandler);
			view.onButtonPauseLevel.RemoveListener(pauseLevelHandler);
            onOrientationChanged.RemoveListener(orientationChangedHandler);
			//view.onExitLevel.RemoveListener(exitLevelHandler);
            //view.onRetyLevel.RemoveListener(retyLevelHandler);

            base.OnRemove();
        }

    }
}