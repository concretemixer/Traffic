using System;
using System.Collections.Generic;
using System.Linq;
using Commons.Utils;
using Commons.Utils.Commands;
using Facebook.Unity;
using RSG;

namespace Commons.SN.Facebook.Commands
{
    public class GrantPostPermissionsCommand : IAsyncCommand
    {
        const string PUBLISH_PERMISSION = "publish_actions";

        UnityEventProvider unityEvents;

        public GrantPostPermissionsCommand(UnityEventProvider _eventProvider)
        {
            unityEvents = _eventProvider;
        }

        public IPromise<IAsyncCommand> Run()
        {
            var promise = new Promise<IAsyncCommand>();

            if (FB.IsLoggedIn)
                onLoginComplete(promise);
            else
                unityEvents.onGui.AddOnce(() => FB.LogInWithReadPermissions(new List<string> { PUBLISH_PERMISSION }, (_result) => onLoginComplete(promise)));

            return promise;
        }

        void onLoginComplete(Promise<IAsyncCommand> _promise)
        {
            if (AccessToken.CurrentAccessToken.Permissions.Contains(PUBLISH_PERMISSION))
            {
                Loggr.Log("Publish permissions grunted!");
                _promise.Resolve(this);
            }
            else
            {
                _promise.Reject(new Exception("Publish permissions not grunted"));
            }
        }
    }
}