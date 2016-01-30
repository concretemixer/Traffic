using UnityEngine;
using System.Collections;

namespace Traffic.Core
{

    public class TrainMovement : TutorialScenarioBase
    {

        float lifetime = 0;
        private float v = 0;
        private float vmax = 30;
        private Vector3 startPos;

        // Use this for initialization
        void Start()
        {
            startPos = transform.localPosition;
        }


        public override void Reset()
        {
            lifetime = 0;
            v = 0;
            transform.localPosition = startPos;
        }

        // Update is called once per frame
        void Update()
        {

            lifetime += Time.deltaTime;

            if (lifetime < 7.5)
                return;

            v += Time.deltaTime * 11;
            if (v > vmax)
                v = vmax;

            Vector3 shift = new Vector3(-v * Time.deltaTime,0,0);

            transform.localPosition += shift;

        }
    }

}