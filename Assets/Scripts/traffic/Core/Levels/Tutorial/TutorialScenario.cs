using UnityEngine;
using System.Collections;

using Traffic.MVCS.Commands.Signals;

namespace Traffic.Core
{
    public class TutorialScenario : MonoBehaviour
    {
        [Inject]
        public TutorialPoint onTutorialPoint { get; set; }

        private int point = 0;
        private float lifetime = 0;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
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
