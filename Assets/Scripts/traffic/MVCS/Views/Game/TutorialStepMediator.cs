using Traffic.MVCS.Commands.Signals;
//using traffic.MVCS.model;
//using traffic.MVCS.model.level;
using Traffic.MVCS.Views.UI.HUD;
using Traffic.MVCS.Models;
using Traffic.Components;
using Traffic.Core;

using Commons.UI;
using Commons.Utils;
using UnityEngine;
using System;

using strange.extensions.mediation.impl;

namespace Traffic.MVCS.Views.UI
{
    public class TutorialStepMediator : Mediator
    {
        [Inject]
        public TutorialStepScreen view
        {
            get;
            set;
        }

        [Inject]
        public ResumeTutorial onResumeTutorial { get; set; }


        [Inject]
        public IUIManager UI {
            get;
            set;
        }

        [Inject]
        public ILocaleService localeService { get; set; }

        [Inject(EntryPoint.Container.Stage)]
        public GameObject stage { get; set; }

        GameObject target = null;


        void nextStepHandler()
        {
            Time.timeScale = 0.85f;
            
            UI.Hide(UIMap.Id.ScreenTutorial);
            target = null;

        }

        void Update()
        {
            UnityEngine.Debug.Log("Update");

            if (view.Step < 0)
                return;
            target = null;
            if (target == null)
            {

                GameObject[] vehicles = GameObject.FindGameObjectsWithTag("Vehicle");
                foreach (var v in vehicles)
                {
                    if (view.Step == 0 && v.GetComponent<Vehicle>().Number == 1)
                    {
                        target = v;
                        break;
                    }
                    if (view.Step == 4 && v.GetComponent<Vehicle>().Number == 3)
                    {
                        target = v;
                        break;
                    }
                    if (view.Step == 5 && v.GetComponent<Vehicle>().Number == 3)
                    {
                        target = v;
                        break;
                    }
                    if (view.Step == 7 && v.GetComponent<Vehicle>().Number == 5)
                    {
                        target = v;
                        break;
                    }
                }

                if (view.Step == 6)
                    target = GameObject.Find("StopHelper");

                foreach (TutorialTouchCamera camera in stage.GetComponentsInChildren<TutorialTouchCamera>())
                {
                    camera.SetTarget(target==null ? null : target.GetComponent<Vehicle>(), view.Step!=4);
                }

            }

            if (Camera.main == null)
            {
               // UnityEngine.Debug.Log("No camera");
                return;
            }

            bool vertical = Screen.height > Screen.width;

            float k = (Time.unscaledTime % 0.8f) / 0.8f;

            if (target != null)
            {
                Vector3 screenPos = Camera.main.WorldToScreenPoint(target.transform.position);

                screenPos.x /= (float)Camera.main.pixelWidth;
                screenPos.y /= (float)Camera.main.pixelHeight;

                UnityEngine.Debug.Log(screenPos);


                float w = view.gameObject.GetComponent<RectTransform>().rect.width;
                float h = view.gameObject.GetComponent<RectTransform>().rect.height;

              //  UnityEngine.Debug.Log(w + "," + h);

                w *= screenPos.x;
                h *= screenPos.y;

              //  UnityEngine.Debug.Log("k = " + k);

                if (view.Step == 4)
                {
                    view.SetHandPos(w + 90 * k, h + 50.0f * (float)Math.Sqrt(k));
                    view.SetHandAlpha(2 * (1 - k * k));
                }
                else if (view.Step == 6 || view.Step == 7)
                {
                    view.SetHandAlpha(0);
                }
                else
                {
                    float k2 = 1 - (float)Math.Sin(Math.PI * k);

                    view.SetHandPos(w - 10 * k2, h + 10.0f * k2);
                    view.SetHandAlpha(1);
                }
                view.SetShadePos(w, h);

                if (vertical)
                    view.SetBubblePos(w, h, view.Step==7 ? -170 : -120, true, true);
                else
                    view.SetBubblePos(w, h, 80, true,false);

            }
            else
            {
                float ratio = (float)Screen.height / (float)Screen.width;

                float scaledDimention;

                if (ratio < 1)
                    scaledDimention = 960 * ratio;            
                else
                    scaledDimention = 960 / ratio;

                if (vertical)
                {
                    view.SetShadePos(-160, 10);
                    if (view.Step == 1)
                        view.SetBubblePos(300, 940, 0, false, false);
                    if (view.Step == 2)
                        view.SetBubblePos(510, 930, -140, false, true);
                    if (view.Step == 3)
                        view.SetBubblePos(80, 930, 100, false, false);
                }
                else
                {
                    view.SetShadePos(10, -160);
                    if (view.Step == 1)
                        view.SetBubblePos(300, scaledDimention-20, 0, false, false);
                    if (view.Step == 2)
                        view.SetBubblePos(885, scaledDimention-30, -100, false, true);
                    if (view.Step == 3)
                        view.SetBubblePos(80, scaledDimention-30, 100, false, false);
                }
            }


            view.SetBubbleText(localeService.ProcessString("%TUTOR_STEP_"+view.Step.ToString()+"%"));
        }

        public override void OnRegister()
        {

            view.onButtonNextStep.AddListener(nextStepHandler);
            onResumeTutorial.AddListener(nextStepHandler);
            view.Layout(Screen.width,Screen.height);

            base.OnRegister();
        }




        public override void OnRemove()
        {
            view.onButtonNextStep.RemoveListener(nextStepHandler);
            onResumeTutorial.RemoveListener(nextStepHandler);            

            base.OnRemove();
        }
    }
}