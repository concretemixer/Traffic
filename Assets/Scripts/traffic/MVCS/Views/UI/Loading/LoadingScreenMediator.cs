using Commons.Utils;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;
using UnityEngine;

namespace Traffic.MVCS.Views.UI.Loading
{
    public class LoadingScreenMediator : Mediator
    {
        [Inject]
        public LoadingScreenView view { set; private get; }

        Signal timerDoneSignal;

        override public void OnRegister()
        {
            view.Layout(Screen.width, Screen.height);

            timerDoneSignal = DummyTimer.WaitFor(2000, "WaitTimer");
            timerDoneSignal.AddListener(timerDoneHandler);
        }

        override public void OnRemove()
        {
            timerDoneSignal.RemoveListener(timerDoneHandler);
        }

        void timerDoneHandler()
        {
            view.ShowPreloader();
        }
    }
}