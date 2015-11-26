using strange.extensions.context.impl;
using Traffic.MVCS;
using UnityEngine;

namespace Traffic.Components
{
    public class EntryPoint : ContextView
    {
        [SerializeField]
        GameObject ui;
        [SerializeField]
        GameObject stage;

        void Start()
        {
            context = new AppContext(this);
            context.Start();
        }

        public GameObject UI
        {
            get
            {
                return ui;
            }
        }

        public GameObject Stage
        {
            get
            {
                return stage;
            }
        }
    }
}
