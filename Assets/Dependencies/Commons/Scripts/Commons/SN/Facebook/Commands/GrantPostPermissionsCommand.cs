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
            Loggr.Log("Try to grant post permissions");
            unityEvents = _eventProvider;
        }

        public IPromise<IAsyncCommand> Run()
        {
            var promise = new Promise<IAsyncCommand>();

            if (hasPublishPremissions)
                onLoginComplete(promise);
            else
                unityEvents.onGui.AddOnce(() => FB.LogInWithPublishPermissions(new List<string> { PUBLISH_PERMISSION }, (_result) => onLoginComplete(promise)));

            return promise;
        }

        void onLoginComplete(Promise<IAsyncCommand> _promise)
        {
            foreach (var perm in AccessToken.CurrentAccessToken.Permissions)
                Loggr.Log("Granted: " + perm);

            if (hasPublishPremissions)
            {
                Loggr.Log("Publish permissions grunted!");
                _promise.Resolve(this);
            }
            else
            {
                Loggr.Log("Premissions not granted ((");
                _promise.Reject(new Exception("Publish permissions not grunted"));
            }
        }

        bool hasPublishPremissions
        {
            get
            {
                return AccessToken.CurrentAccessToken.Permissions.Contains(PUBLISH_PERMISSION);
            }
        }
    }
}