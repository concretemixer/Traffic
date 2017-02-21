using System;
using System.Collections.Generic;
using Commons.Utils;
using Commons.Utils.Commands;
//using Facebook.Unity;
using RSG;
//using Unifb = Facebook.Unity;

namespace Commons.SN.Facebook.Commands
{
    /*
    public class LoginFBCommand : IAsyncCommand
    {
        UnityEventProvider unityEvents;

        List<String> permissions = new List<string>() { "public_profile" };

        public LoginFBCommand(UnityEventProvider _eventProvider)
        {
            unityEvents = _eventProvider;
        }

        public IPromise<IAsyncCommand> Run()
        {
            var promise = new Promise<IAsyncCommand>();

            if (FB.IsLoggedIn)
                onLoginComplete(promise);
            else
                unityEvents.onGui.AddOnce(() => FB.LogInWithReadPermissions(permissions, (_result) => onLoginComplete(promise)));

            return promise;
        }

        void onLoginComplete(Promise<IAsyncCommand> _promise)
        {
            if (FB.IsLoggedIn)
            {
                var aToken = Unifb.AccessToken.CurrentAccessToken;
                // Print current access token's User ID
                Loggr.Log(aToken.UserId);
                // Print current access token's granted permissions
                foreach (string perm in aToken.Permissions)
                {
                    Loggr.Log(perm);
                }
                _promise.Resolve(this);
            }
            else
            {
                Loggr.Log("Fail to login in FB");
                _promise.Reject(new Exception("Fail to login in FB"));
            }
        }
    }*/
}