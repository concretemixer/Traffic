using System;
using Commons.SN.Extensions;
using Commons.Utils;
using Facebook.Unity;
using RSG;

namespace Commons.SN.Facebook.Extensions
{
    public class InitSNExtension : ExtensionBase, IInitExtension
    {
        public InitSNExtension(FacebookSN _facebookSN) : base(_facebookSN) { }

        public IPromise Execute()
        {
            var promise = new Promise();
            if (FB.IsInitialized)
                onInitComplete(promise);
            else
                FB.Init(() =>
                {
                    onInitComplete(promise);
                });
            return promise;
        }

        void onInitComplete(Promise _promise)
        {
            if (FB.IsInitialized)
            {
                FB.ActivateApp();
                Loggr.Log("FB app initialized");
                _promise.Resolve();
            }
            else
            {
                Loggr.Log("Failed to init FB");
                _promise.Reject(new Exception("Can't init Facebook SDK."));
            }
        }
    }
}