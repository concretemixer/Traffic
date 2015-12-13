using UnityEngine;
using UnityEngine.UI;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;

namespace Traffic.MVCS.Views.UI
{
    public class LevelDoneMenuView : View
    {
        [SerializeField]
        Button homeButton;

        [SerializeField]
        Button nextButton;

        public readonly Signal onButtonNextLevel = new Signal();
        public readonly Signal onButtonHome = new Signal();

        protected override void Awake()
        {
            homeButton.onClick.AddListener(onButtonHome.Dispatch);
            nextButton.onClick.AddListener(onButtonNextLevel.Dispatch);
            base.Awake();
        }

       

        protected override void OnDestroy()
        {
            nextButton.onClick.RemoveListener(onButtonNextLevel.Dispatch);
            homeButton.onClick.RemoveListener(onButtonHome.Dispatch);
            base.OnDestroy();
        }

    }
}