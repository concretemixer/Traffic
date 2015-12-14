using System.Collections.Generic;
using UnityEngine;

using Commons.UI;
using Traffic.MVCS.Models;
using Traffic.Core;
using Traffic.MVCS.Commands.Signals;

using strange.extensions.mediation.impl;

namespace Traffic.MVCS.Views.Game
{
    public class LevelMediator : Mediator
    {
        [Inject]
        public IUIManager UI { private get; set; }

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
			if (level.Progress == level.Target) {
				onLevelComplete.Dispatch();
			}

           
		}



		void vehicleCrashedHandler()
		{
			if (!level.Failed) {
                Invoke("levelFailedDispatch", 1);				
				level.Failed = true;
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
			onVehicleCrashed.RemoveListener (vehicleReachedHandler);
            onLevelFailed.RemoveListener(levelFailedHandler);
            onLevelComplete.RemoveListener(levelCompleteHandler);
            base.OnRemove();
        }
    }
}