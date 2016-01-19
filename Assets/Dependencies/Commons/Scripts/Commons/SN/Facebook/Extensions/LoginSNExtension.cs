using System;
using System.Collections.Generic;
using Commons.SN.Extensions;
using Commons.Utils;
using Facebook.Unity;
using RSG;
using Unifb = Facebook.Unity;

namespace Commons.SN.Facebook.Extensions
{
    public class LoginSNExtension : ExtensionBase, ILoginExtension
    {
        UnityEventProvider unityEvents;

        List<String> permissions = new List<string>() { "public_profile", "user_friends" };

        public LoginSNExtension(FacebookSN _facebookSN, UnityEventProvider _eventProvider) : base(_facebookSN)
        {
            unityEvents = _eventProvider;
        }

        public IPromise Execute()
        {
            return Promise.Sequence(
                  network.GetExt<IInitExtension>().Execute,
                  makeLogin
            );
        }

        Promise makeLogin()
        {
            var promise = new Promise();

            if (FB.IsLoggedIn)
                onLoginComplete(promise);
            else
                unityEvents.onGui.AddOnce(() => FB.LogInWithReadPermissions(permissions, (_result) => onLoginComplete(promise)));

            return promise;
        }

        void onLoginComplete(Promise _promise)
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
                ((FacebookSN)network).IsLoggedIn = true;
                _promise.Resolve();
            }
            else
            {
                Loggr.Log("Fail to login in FB");
                _promise.Reject(new Exception("Fail to login in FB"));
            }
        }
    }
}