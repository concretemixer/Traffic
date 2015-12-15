using UnityEngine;
using UnityEngine.UI;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;

namespace Traffic.MVCS.Views.UI
{
    public class LevelFailedMenuView : RotatableView
    {
        [SerializeField]
        Button homeButton;

        [SerializeField]
        Button retryButton;

        public readonly Signal onButtonRetryLevel = new Signal();
        public readonly Signal onButtonHome = new Signal();

        protected override void Awake()
        {
            homeButton.onClick.AddListener(onButtonHome.Dispatch);
            retryButton.onClick.AddListener(onButtonRetryLevel.Dispatch);
            base.Awake();
        }

       

        protected override void OnDestroy()
        {
            homeButton.onClick.RemoveListener(onButtonHome.Dispatch);
            retryButton.onClick.RemoveListener(onButtonRetryLevel.Dispatch);
            base.OnDestroy();
        }

    }
}