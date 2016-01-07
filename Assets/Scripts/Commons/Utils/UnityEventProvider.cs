using strange.extensions.signal.impl;
using UnityEngine;

namespace Commons.Utils
{
    public class UnityEventProvider : MonoBehaviour
    {
        public readonly Signal onUpdate = new Signal();
        public readonly Signal onGui = new Signal();

        void Update()
        {
            onUpdate.Dispatch();
        }

        void OnGUI()
        {
            onGui.Dispatch();
        }
    }
}