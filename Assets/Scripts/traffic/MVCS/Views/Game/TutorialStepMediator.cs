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

        void Update()
        {
            if (view.Step < 0)
                return;
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
                    if (view.Step == 1 && v.GetComponent<Vehicle>().Number == 3)
                    {
                        target = v;
                        break;
                    }
                    if (view.Step == 2 && v.GetComponent<Vehicle>().Number == 3)
                    {
                        target = v;
                        break;
                    }
                }

                if (target == null)
                    return;

            }

            if (Camera.current == null)
                return;

            float k = (Time.unscaledTime % 0.8f) / 0.8f;
            

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
                view.SetHandPos(w + 90 * k , h + 50.0f * (float)Math.Sqrt(k));
                view.SetHandAlpha(2*(1 - k * k));
            }
            else
            {
                float k2 = 1-(float)Math.Sin(Math.PI * k);
                
                view.SetHandPos(w - 10 * k2, h + 10.0f * k2);
                //view.SetHandAlpha(k > 0.5 ? 0 : 1);
            }
            view.SetShadePos(w, h);

            if (view.Step==0)
                view.SetBubblePos(w, h, true);
            else
                view.SetBubblePos(w, h, false);

        }

        public override void OnRegister()
        {

            view.onButtonNextStep.AddListener(nextStepHandler);
            
            view.Layout();

            base.OnRegister();
        }




        public override void OnRemove()
        {
            view.onButtonNextStep.RemoveListener(nextStepHandler);
            


            base.OnRemove();
        }
    }
}