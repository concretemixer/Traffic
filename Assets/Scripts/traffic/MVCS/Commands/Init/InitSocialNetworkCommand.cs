using strange.extensions.command.impl;
using System;
//using Commons.SN.Facebook;
using Commons.Utils;
//using Traffic.MVCS.Commands.SN;
using Traffic.MVCS.Commands.Signals;

namespace Traffic.MVCS.Commands.Init
{
    
    public class InitSocialNetworkCommand : Command
    {
        [Inject]
        public UnityEventProvider eventProvider { private get; set; }

        public override void Execute()
        {
#if UNITY_STANDALONE
            injectionBinder.Bind<FacebookSN>().ToSingleton();
            return;
#endif

#if UNITY_WEBGL
            return;
#else
            Retain();

            injectionBinder.Bind<FacebookSN>().ToSingleton();
            var facebook = injectionBinder.GetInstance<FacebookSN>();
            facebook.SetEventProvider(eventProvider);

            facebook.Init().Done(Release, onInitFail);

            commandBinder.Bind<LoginToSNSignal>().To<LoginToFbCommand>();
            injectionBinder.Bind<LoginCompleteSignal>().ToSingleton();

            commandBinder.Bind<GetSNFriendsSignal>().To<GetFrindsCommand>();
            injectionBinder.Bind<FriendsLoadedSignal>().ToSingleton();
#endif
        }

        void onInitFail(Exception _exception)
        {
            throw _exception;
        }
    } 
}