using Traffic.MVCS.Commands.Signals;
//using traffic.MVCS.model;
//using traffic.MVCS.model.level;
using Traffic.MVCS.Views.UI.HUD;
using Traffic.MVCS.Models;
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
        public IUIManager UI {
            get;
            set;
        }

        GameObject target = null;

        void nextStepHandler()
        {
            Time.timeScale = 1;
            UI.Hide(UIMap.Id.ScreenTutorial);
            target = null;

        }

        string[] texts = {
            "The cars are about to crash! TAP the car to accelerate it.",
            "Progress bar. It grows as a vehicle reaches the edge of the screen.",
            "Score. Accelerate vehicles to gain more point!",
            "Attempts count. It decreases with every crash. Once it reaches zero, you have to refill it in various ways.",
            "SWIPE the car to stop it or to decelerate.",
            "TAP the car to make it move again."};


        void Update()
        {
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
                }



            }

            if (Camera.current == null)
                return;

            float k = (Time.unscaledTime % 0.8f) / 0.8f;

            if (target != null)
            {
                Vector3 screenPos = Camera.current.WorldToScreenPoint(target.transform.position);

                screenPos.x /= (float)Camera.current.pixelWidth;
                screenPos.y /= (float)Camera.current.pixelHeight;

                // Debug.Log(screenPos);


                float w = view.gameObject.GetComponent<RectTransform>().rect.width;
                float h = view.gameObject.GetComponent<RectTransform>().rect.height;

                //   Debug.Log(w + "," + h);

                w *= screenPos.x;
                h *= screenPos.y;

                //Debug.Log("k = " + k);

                if (view.Step == 1)
                {
                    view.SetHandPos(w + 90 * k, h + 50.0f * (float)Math.Sqrt(k));
                    view.SetHandAlpha(2 * (1 - k * k));
                }
                else
                {
                    float k2 = 1 - (float)Math.Sin(Math.PI * k);

                    view.SetHandPos(w - 10 * k2, h + 10.0f * k2);
                    //view.SetHandAlpha(k > 0.5 ? 0 : 1);
                }
                view.SetShadePos(w, h);

                //if (view.Step==0)
                view.SetBubblePos(w, h, 50, true);
                //else
                //  view.SetBubblePos(w, h, false);
            }
            else
            {
                view.SetShadePos(0, -160);
                if (view.Step==1)
                    view.SetBubblePos(300, 580, 0 ,false);
                if (view.Step == 2)
                    view.SetBubblePos(845, 570, -70, false);
                if (view.Step == 3)
                    view.SetBubblePos(80, 570, 100, false);
            }

            view.SetBubbleText(texts[view.Step]);
        }

        public override void OnRegister()
        {

            view.onButtonNextStep.AddListener(nextStepHandler);
            
            view.Layout(Screen.width,Screen.height);

            base.OnRegister();
        }




        public override void OnRemove()
        {
            view.onButtonNextStep.RemoveListener(nextStepHandler);
            


            base.OnRemove();
        }
    }
}