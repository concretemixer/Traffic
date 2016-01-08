using System;
using Commons.SN;
using Commons.Utils;
using strange.extensions.mediation.impl;

namespace Traffic.MVCS.Views.UI.Debug
{
    public class ScreenDebugMediator : Mediator
    {
        [Inject]
        public ScreenDebugView view { set; private get; }

        [Inject]
        public FacebookSN facebook { set; private get; }

        public override void OnRegister()
        {
            view.postToFBButton.onClick.AddListener(() => facebook.Publish("super title", "super message", completePublishHandler));
        }

        private void completePublishHandler()
        {
            Loggr.Log("publish complete");
        }
    }
}