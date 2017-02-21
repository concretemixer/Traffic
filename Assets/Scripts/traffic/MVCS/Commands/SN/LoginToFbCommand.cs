using Commons.SN.Facebook;
using strange.extensions.command.impl;
using Traffic.MVCS.Commands.Signals;

namespace Traffic.MVCS.Commands.SN
{
    /*
    public class LoginToFbCommand : Command
    {
        [Inject]
        public FacebookSN facebook { private get; set; }

        [Inject]
        public LoginCompleteSignal loginComplete { private get; set; }

        public override void Execute()
        {
            Retain();

            facebook.Login().Done(() =>
            {
                loginComplete.Dispatch();
                Release();
            });
        }
    } */
}