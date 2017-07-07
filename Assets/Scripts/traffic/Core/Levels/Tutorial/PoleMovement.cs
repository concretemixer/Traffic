using UnityEngine;
using System.Collections;

namespace Traffic.Core
{

    public class PoleMovement : TutorialScenarioBase
    {

        // Use this for initialization
        float lifetime = 0;
        bool down = false;
        private bool stopped = false;

        GameObject flash;

        void Start()
        {
            flash = GameObject.Find("Flash1");
            Reset();
        }

        public override void Reset()
        {
            transform.localRotation = Quaternion.Euler(0, 0, -66.0f);
            transform.localPosition = Vector3.zero;
            lifetime = 0;
            down = false;
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

            lifetime += Time.deltaTime;
            if (down)
            {
                if (lifetime > 14.5)
                {
                    transform.localRotation = Quaternion.Euler(0, 0, -(lifetime - 14.5f)*66.0f);
                }

                if (lifetime > 15.5)
                {
                    transform.localRotation = Quaternion.Euler(0, 0, -70.0f);

                    if (flash != null && flash.GetComponent<ParticleSystem>() != null)
                        if (flash.GetComponent<ParticleSystem>().isPlaying)
                            flash.GetComponent<ParticleSystem>().Stop();

                }
            }
            else
            {

                if (lifetime > 7.5f)
                {
                    transform.localRotation = Quaternion.Euler(0, 0, -(8.5f - lifetime)*66.0f);
                    if (flash!=null && flash.GetComponent<ParticleSystem>()!=null)
                        if (!flash.GetComponent<ParticleSystem>().isPlaying)
                            flash.GetComponent<ParticleSystem>().Play();
                }

                if (lifetime > 8.5f)
                {
                    transform.localRotation = Quaternion.Euler(0, 0, 0);
                    down = true;
                }
            }
        }
    }

}