using UnityEngine;
using System.Collections;
using Traffic.MVCS.Services;
using Traffic.MVCS.Commands.Signals;

namespace Traffic.Core
{
    public class TutorialScenario : TutorialScenarioBase
    {
        [Inject]
        public TutorialPoint onTutorialPoint { get; set; }

        private int point = 0;
        private float lifetime = 0;
        private bool stopped = false;

        // Use this for initialization
        void Start()
        {

        }

        public override void Reset()
        {
            onTutorialPoint.Dispatch((int)TutorialStep.START);
            lifetime = 0;
            point = 0;
            stopped = false;
        }

        public override void Stop()
        {
            stopped = true;
        }

        // Update is called once per frame
        void Update()
        {
            if (stopped)
                return;
            if (Time.timeScale>0)
                lifetime += Time.deltaTime;
            if (point == 0)
            {
                if (lifetime > 3)
                {
                    onTutorialPoint.Dispatch(point);
                    point++;
                }
            }
            else if (point == 1)
            {
                if (lifetime > 8)
                {
                    onTutorialPoint.Dispatch(point);
                    point++;
                }
            }
            else if (point == 2)
            {
                if (lifetime > 9)
                {
                    onTutorialPoint.Dispatch(point);
                    point++;
                }
            }
            else if (point == 3)
            {
                if (lifetime > 10)
                {
                    onTutorialPoint.Dispatch(point);
                    point++;
                }
            }
            else if (point == 4)
            {
                if (lifetime > 11.5)
                {
                    onTutorialPoint.Dispatch(point);
                    point++;
                }
            }
            else if (point == 5)
            {
                if (lifetime > 17)
                {
                    GameObject[] vehicles = GameObject.FindGameObjectsWithTag("Vehicle");
                    foreach (var v in vehicles)
                    {
                        if (v.GetComponent<Vehicle>().Number == 3)
                        {
                            if (v.GetComponent<Rigidbody>().velocity.magnitude<0.01)
                                onTutorialPoint.Dispatch(point);
                            break;
                        }
                    }
                    
                    point++;
                }
            }
            else if (point == 6)
            {
                if (lifetime > 22)
                {
                    onTutorialPoint.Dispatch(point);
                    point++;
                }
            }
            else if (point == 7)
            {
                if (lifetime > 24.5)
                {
                    onTutorialPoint.Dispatch(point);
                    point++;
                }
            }

        }
    }
}
