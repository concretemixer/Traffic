using System.Collections.Generic;
using UnityEngine;
using System;

using Commons.UI;
using Traffic.MVCS.Models;
using Traffic.Core;
using Traffic.Components;
using Traffic.MVCS.Commands.Signals;

using strange.extensions.mediation.impl;
using Traffic.MVCS.Services;

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
        public IAPService iapService { get; set; }

        [Inject]
        public ILevelListModel levels { get; set; }

		[Inject]
		public VehicleReachedDestination onVehicleReachedDestination { get; set;}

		[Inject]
		public VehicleCrashed onVehicleCrashed { get; set;}

		[Inject]
		public LevelFailed onLevelFailed { get; set;}
		
		[Inject]
		public LevelComplete onLevelComplete { get; set;}

        [Inject]
        public AnalyticsCollector analitics { private get; set; }

        float shakeTimer = 0;
        const float shakeTime  = 0.5f;
        Vector3 cameraStartPos;
        float[] ampsX = { -3, 6, 2 };
        float[] periodsX = { 5f, 5f, 3f };
        float[] ampsY = { 3, -6, -1 };
        float[] periodsY = { 5f, 5f, 2f };

        void Update()
        {
//            Debug.Log("Shake timer = " + shakeTimer);
            if (shakeTimer>0)
            {
                float t = (shakeTime - shakeTimer) / shakeTime;


                Camera.main.transform.localPosition = cameraStartPos;

                for (int a = 0; a < ampsX.Length; a++)
                {
                    Camera.main.transform.localPosition += 
                        new Vector3((float)Math.Sin(periodsX[a] * Math.PI * t) * ampsX[a],
                                    (float)Math.Sin(periodsY[a] * Math.PI * t) * ampsY[a],
                                   0) *  (1 - t);
                }
                shakeTimer -= Time.deltaTime;
                if (shakeTimer <= 0)
                {
                    Camera.main.transform.localPosition = cameraStartPos;
                }
            }
        }


		void vehicleReachedHandler()
		{
            if (!level.Failed && !level.Complete)
            {
                level.Progress++;
                if (level.Progress == level.Config.target)
                {
                    analitics.LevelComplete(level.LevelIndex, level.Score);
                    onLevelComplete.Dispatch();
                }
            }
		}

		void vehicleCrashedHandler()
		{
            //Debug.Log("HERE!");
            if (!level.Failed && !level.Complete) {
                cameraStartPos = Camera.main.transform.localPosition;
                shakeTimer = shakeTime;
                level.Failed = true;
           //     Debug.Log("!!!! Shake timer = " + shakeTimer);

                for (int a = 0; a < ampsX.Length; a++)
                {
                    ampsX[a] = UnityEngine.Random.Range(-2f, 2f);
                    ampsY[a] = UnityEngine.Random.Range(-1f, 1f);
                    periodsX[a] = UnityEngine.Random.Range(3f, 6f);
                    periodsY[a] = UnityEngine.Random.Range(3f, 6f);                    
                }

              
               // onLevelFailed.Dispatch();
                this.Invoke("levelFailedDispatch", 1);

                if (levels.CurrentLevelIndex != 0)
                {
                    if (!iapService.IsBought(IAPType.NoAdverts))
                    {
                        levels.TriesLeft--;
                        if (levels.TriesLeft == 0)
                            levels.TriesRefreshTime = DateTime.Now.AddHours(1);
                    }
                }

                foreach (var scenario in stage.GetComponentsInChildren<TutorialScenarioBase>())
                {
                    scenario.Stop();
                }                
			}
		}

        void levelFailedDispatch()
        {
           // Debug.Log("HERE! 1");

            analitics.LevelFail(level.LevelIndex, level.Score);
            onLevelFailed.Dispatch();
        }

        void levelFailedHandler()
        {
           // Debug.Log("HERE!2");

            UI.Hide(UIMap.Id.ScreenHUD);
	        if (levels.CurrentLevelIndex==0)
                UI.Show(UIMap.Id.TutorialFailedMenu);	
            else
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
            if (levels.CurrentLevelIndex==0)
                UI.Show(UIMap.Id.TutorialDoneMenu);
            else
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