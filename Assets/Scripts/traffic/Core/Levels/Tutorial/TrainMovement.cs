using UnityEngine;
using System.Collections;
using Traffic.MVCS.Commands.Signals;

namespace Traffic.Core
{

    public class TrainMovement : TutorialScenarioBase
    {


        bool stop = false;
        float lifetime = 0;
        private float vmax = 30;
        private Vector3 startPos;

        // Use this for initialization
        void Start()
        {
            startPos = transform.localPosition;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }

        public override void Stop()
        {
            stop = true;
        }

        public override void Reset()
        {
            stop = false;
            lifetime = 0;
            transform.localPosition = startPos;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }

        // Update is called once per frame
        void Update()
        {
            if (stop)
                return;

            lifetime += Time.deltaTime;

            if (lifetime < 7.5)
                return;

            if (Time.timeScale>0)
                if (GetComponent<Rigidbody>().velocity.magnitude < vmax)
                    GetComponent<Rigidbody>().AddForce(-10, 0, 0, ForceMode.Force);
        }
    }

}