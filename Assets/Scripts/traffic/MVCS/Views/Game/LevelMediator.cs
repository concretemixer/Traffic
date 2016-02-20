using System.Collections.Generic;
using UnityEngine;

using Commons.UI;
using Traffic.MVCS.Models;
using Traffic.Core;
using Traffic.Components;
using Traffic.MVCS.Commands.Signals;

using strange.extensions.mediation.impl;

namespace Traffic.MVCS.Views.Game
{
    public class LevelMediator : Mediator
    {
        [Inject]
        public IUIManager UI { private get; set; }

        [Inject(EntryPoint.Container.Stage)]
        public GameObject stage { get; set; }

        [Inject]
        public LevelView view
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
		public VehicleReachedDestination onVehicleReachedDestination { get; set;}

		[Inject]
		public VehicleCrashed onVehicleCrashed { get; set;}

		[Inject]
		public LevelFailed onLevelFailed { get; set;}
		
		[Inject]
		public LevelComplete onLevelComplete { get; set;}

		void vehicleReachedHandler()
		{
			level.Progress++;
			if (level.Progress == level.Config.target) {
				onLevelComplete.Dispatch();
			}

           
		}



		void vehicleCrashedHandler()
		{
			if (!level.Failed) {
                //levelFailedDispatch();
                level.Failed = true;                
               // onLevelFailed.Dispatch();
                this.Invoke("levelFailedDispatch", 1);

                levels.TriesLeft--;

                foreach (var scenario in stage.GetComponentsInChildren<TutorialScenarioBase>())
                {
                    scenario.Stop();
                }                
			}
		}

        void levelFailedDispatch()
        {
            onLevelFailed.Dispatch();
        }

        void levelFailedHandler()
        {
            UI.Hide(UIMap.Id.ScreenHUD);	
            UI.Show(UIMap.Id.LevelFailedMenu);	
        }

        void levelCompleteHandler()
        {
            level.Complete = true;

            int stars = 1;

            if (level.Score >= levels.LevelConfigs[levels.CurrentLevelIndex].threeStarsScore)
                stars = 3;
            else if (level.Score >= levels.LevelConfigs[levels.CurrentLevelIndex].twoStarsScore)
                stars = 2;


            if (levels.GetLevelState(levels.CurrentLevelIndex) != LevelState.PassedThreeStars)
            {
                if (stars == 3)
                    levels.SetLevelState(levels.CurrentLevelIndex, LevelState.PassedThreeStars);
                else if (stars == 2)                
                    levels.SetLevelState(levels.CurrentLevelIndex, LevelState.PassedTwoStars);                
                else if (levels.GetLevelState(levels.CurrentLevelIndex) != LevelState.PassedTwoStars)
                        levels.SetLevelState(levels.CurrentLevelIndex, LevelState.PassedOneStar);
            }             

            if (levels.GetLevelState(levels.CurrentLevelIndex + 1) == LevelState.Locked)
                levels.SetLevelState(levels.CurrentLevelIndex + 1, LevelState.Playable);

            UI.Hide(UIMap.Id.ScreenHUD);
            UI.Show(UIMap.Id.LevelDoneMenu);
        }

        public override void OnRegister()
        {
			onVehicleReachedDestination.AddListener (vehicleReachedHandler);
			onVehicleCrashed.AddListener (vehicleCrashedHandler);
            onLevelFailed.AddListener(levelFailedHandler);
            onLevelComplete.AddListener(levelCompleteHandler);
			base.OnRegister();
        }

		public override void OnRemove()
        {
			onVehicleReachedDestination.RemoveListener (vehicleReachedHandler);
            onVehicleCrashed.RemoveListener(vehicleCrashedHandler);
            onLevelFailed.RemoveListener(levelFailedHandler);
            onLevelComplete.RemoveListener(levelCompleteHandler);
            base.OnRemove();
        }
    }
}