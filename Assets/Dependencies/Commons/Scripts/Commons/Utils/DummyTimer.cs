using System;
using strange.extensions.signal.impl;
using UnityEngine;

namespace Commons.Utils
{
    public class DummyTimer : MonoBehaviour, IDisposable
    {
        // TODO: requre to optimize for using a one timer for all
        public static Signal WaitFor(float duration, string name = "DummyTimer")
        {
            var timer = InstantiateUtil.AddToPath<DummyTimer>("system.timers");
            timer.name = name;
            timer.Run(duration);
            return timer.onTime;
        }

        public static DummyTimer Create(string name = "DummyTimer")
        {
            var timer = InstantiateUtil.AddToPath<DummyTimer>("system.timers");
            timer.name = name;
            return timer;
        }

        public readonly Signal onTime = new Signal();

        float duration = -1;
        float lasts;
        bool isRepeated;

        public void Dispose()
        {
            Destroy(this.gameObject);
        }

        public void Run(float duration, bool isRepeated = false)
        {
            this.duration = duration;
            this.lasts = duration;
            this.isRepeated = isRepeated;
        }

        void FixedUpdate()
        {
            if (duration != -1)
            {
                lasts -= (Time.deltaTime * 1000);
                if (lasts <= 0)
                {
                    if (isRepeated)
                        lasts = duration;
                    else
                    {
                        duration = -1;
                        Dispose();
                    }

                    onTime.Dispatch();
                }
            }
        }
    }
}