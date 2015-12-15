using UnityEngine;
using UnityEngine.UI;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;

using strange.extensions.mediation.impl;

namespace Traffic.MVCS.Views.UI
{
    public class PauseMenuView : RotatableView
    {
        [SerializeField]
        Button homeButton;

        [SerializeField]
        Button goButton;

        [SerializeField]
        Button optionsButton;


        public readonly Signal onButtonResumeLevel = new Signal();
        public readonly Signal onButtonHome = new Signal();

        protected override void Awake()
        {
            goButton.onClick.AddListener(onButtonResumeLevel.Dispatch);
            homeButton.onClick.AddListener(onButtonHome.Dispatch);
            base.Awake();
        }

       

        protected override void OnDestroy()
        {
            goButton.onClick.RemoveListener(onButtonResumeLevel.Dispatch);
            homeButton.onClick.RemoveListener(onButtonHome.Dispatch);
            base.OnDestroy();
        }

    }
}