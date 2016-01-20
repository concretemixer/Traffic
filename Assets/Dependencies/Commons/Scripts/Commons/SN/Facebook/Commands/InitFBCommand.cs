using System;
using Commons.Utils;
using Commons.Utils.Commands;
using Facebook.Unity;
using RSG;

namespace Commons.SN.Facebook.Commands
{
    public class InitFBCommand : IAsyncCommand
    {
        public IPromise<IAsyncCommand> Run()
        {
            var promise = new Promise<IAsyncCommand>();
            if (FB.IsInitialized)
                onInitComplete(promise);
            else
                FB.Init(() =>
                {
                    onInitComplete(promise);
                });
            return promise;
        }

        void onInitComplete(Promise<IAsyncCommand> _promise)
        {
            if (FB.IsInitialized)
            {
                FB.ActivateApp();
                Loggr.Log("FB app initialized");
                _promise.Resolve(this);
            }
            else
            {
                Loggr.Log("Failed to init FB");
                _promise.Reject(new Exception("Can't init Facebook SDK."));
            }
        }
    }
}