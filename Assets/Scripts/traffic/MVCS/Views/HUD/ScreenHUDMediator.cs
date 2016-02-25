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
        public ILevelListModel levels
        {
            get;
            set;
        }

        [Inject]
        public IAPService iapService { get; set; }

		[Inject]
		public VehicleReachedDestination onVehicleReachedDestination { get; set;}

		[Inject]
		public LevelPause onPause { get; set;}

		[Inject]
		public LevelResume onResume { get; set;}

        [Inject]
        public ScoreGrow onScoreGrow { get; set; }

        [Inject]
        public IUIManager ui {
            get;
            set;
        }
			
        public override void OnRegister()
		{
            onScoreGrow.AddListener(scoreGrowHandler);
			onVehicleReachedDestination.AddListener(vehicleReachHandler);
			view.onButtonPauseLevel.AddListener(pauseLevelHandler);
          //  view.onRetyLevel.AddListener(retyLevelHandler);

            view.Layout();
            updateLevelProgress();

            view.SetTutorial(levels.CurrentLevelIndex == 0);
                
            base.OnRegister();
        }

        void updateLevelProgress()
        {
             view.SetScore((int)level.Score);			
             view.SetProgress(level.Progress, level.Config.target);
             if (iapService.IsBought(IAPType.NoAdverts))
                view.SetTries(int.MaxValue,int.MaxValue);
            else
                view.SetTries(levels.TriesLeft, levels.TriesTotal);
        }

        void scoreGrowHandler(float diff)
        {
            if (!level.Failed && !level.Complete)
            {
                level.Score += diff;
                updateLevelProgress();
            }
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

        public override void OnRemove()
        {
			onVehicleReachedDestination.RemoveListener(vehicleReachHandler);
            onScoreGrow.RemoveListener(scoreGrowHandler);
			view.onButtonPauseLevel.RemoveListener(resumeLevelHandler);
			view.onButtonPauseLevel.RemoveListener(pauseLevelHandler);

            base.OnRemove();
        }

    }
}