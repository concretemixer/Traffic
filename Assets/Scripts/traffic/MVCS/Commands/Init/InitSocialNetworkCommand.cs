using strange.extensions.command.impl;
using Commons.SN.Facebook.Extensions;
using System;
using Commons.SN.Facebook;
using Commons.Utils;
using Traffic.MVCS.Commands.SN;
using Traffic.MVCS.Commands.Signals;

namespace Traffic.MVCS.Commands.Init
{
    public class InitSocialNetworkCommand : Command
    {
        [Inject]
        public UnityEventProvider eventProvider { private get; set; }

        public override void Execute()
        {
            Retain();

            injectionBinder.Bind<FacebookSN>().ToSingleton();
            var facebook = injectionBinder.GetInstance<FacebookSN>();
            facebook.Init(eventProvider);

            var initExt = facebook.GetExt<InitSNExtension>();
            initExt.Execute().Done(Release, onInitFail);
            
            commandBinder.Bind<LoginToSNSignal>().To<LoginToFbCommand>();
            injectionBinder.Bind<LoginCompleteSignal>().ToSingleton();
        }

        void onInitFail(Exception _exception)
        {
            throw _exception;
        }
    }
}