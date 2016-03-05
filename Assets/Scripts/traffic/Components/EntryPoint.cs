using strange.extensions.context.impl;
using Traffic.MVCS;
using UnityEngine;
using UnityEngine.CrashLog;

namespace Traffic.Components
{
    public class EntryPoint : ContextView
    {
        public static string DebugMessage = "";

        [SerializeField] GameObject ui;
        [SerializeField] GameObject stage;
        [SerializeField] GameObject stageMenu;

        void Start()
        {
            CrashReporting.Init("6d41d8c6-1449-4459-b1df-45d40c474002");
            context = new AppContext(this);
            context.Start();
        }

        public GameObject UI
        {
            get { return ui; }
        }

        public GameObject Stage
        {
            get { return stage; }
        }

        public GameObject StageMenu
        {
            get { return stageMenu; }
        }

        public enum Container
        {
            StageMenu,
            Stage,
            UI
        }
    }
}