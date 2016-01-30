using UnityEngine;
using System.Collections;

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
            lifetime += Time.deltaTime * Time.timeScale;
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
                if (lifetime > 11)
                {
                    onTutorialPoint.Dispatch(point);
                    point++;
                }
            }
            else if (point == 2)
            {
                if (lifetime > 17)
                {
                    onTutorialPoint.Dispatch(point);
                    point++;
                }
            }
        }
    }
}
