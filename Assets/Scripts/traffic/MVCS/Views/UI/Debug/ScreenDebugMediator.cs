using Commons.SN.Facebook;
using Commons.Utils;
using strange.extensions.mediation.impl;
using Traffic.MVCS.Commands.Signals;

namespace Traffic.MVCS.Views.UI.Debug
{
    public class ScreenDebugMediator : Mediator
    {
        [Inject]
        public ScreenDebugView view { set; private get; }

        [Inject]
        public FacebookSN facebook { set; private get; }

        [Inject]
        public LoginCompleteSignal loginComplete { private get; set; }

        [Inject]
        public LoginToSNSignal tryLogin { private get; set; }

        public override void OnRegister()
        {
            view.loginToFBButton.onClick.AddListener(tryLogin.Dispatch);
            loginComplete.AddListener(onLoginComplete);

            // view.postToFBButton.onClick.AddListener(() => facebook.Publish("super title", "super message", completePublishHandler));
        }
        
        void onLoginComplete()
        {
            Loggr.Log("debug: login complete.");
        }
    }
}