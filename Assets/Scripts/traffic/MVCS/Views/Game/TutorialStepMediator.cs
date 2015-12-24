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
                    if (view.Step==0 && v.GetComponent<Vehicle>().Number == 1)
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

                Vector3 screenPos = Camera.current.WorldToScreenPoint(target.transform.position);

                screenPos.x /= (float)Camera.current.pixelWidth;
                screenPos.y /= (float)Camera.current.pixelHeight;

                // Debug.Log(screenPos);


                float w = view.gameObject.GetComponent<RectTransform>().rect.width;
                float h = view.gameObject.GetComponent<RectTransform>().rect.height;

                //   Debug.Log(w + "," + h);

                w *= screenPos.x;
                h *= screenPos.y;

                //  Debug.Log(w + "," + h);

                view.SetHandPos(w, h);
            }
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