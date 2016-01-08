using strange.extensions.signal.impl;
using Facebook.Unity;
using Commons.Utils;
using System.Collections.Generic;
using System;

namespace Commons.SN
{
    public class FacebookSN
    {
        [Inject]
        public UnityEventProvider unityEvents { private get; set; }
        public Signal<bool> OnInitComplete = new Signal<bool>();
        List<String> permissions = new List<string>() { "public_profile", "email", "user_friends" };

        public void Init()
        {
            Loggr.Log("Try to init facebook");

            if (FB.IsInitialized)
                onInitFBComplete();
            else
                FB.Init(onInitFBComplete);
        }

        public void AddPremission(string premission)
        {
            if (!permissions.Contains(premission))
                permissions.Add(premission);
        }

        void onInitFBComplete()
        {
            if (FB.IsInitialized)
            {
                FB.ActivateApp();

                if (!FB.IsLoggedIn)
                    unityEvents.onGui.AddOnce(() => FB.LogInWithReadPermissions(permissions, onLoginComplete));
                else
                    onLoginComplete();
            }
            else
            {
                Loggr.Log("Failed to init FB");
                OnInitComplete.Dispatch(false);
            }
        }

        void onLoginComplete(ILoginResult result = null)
        {
            if (FB.IsLoggedIn)
            {
                var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
                // Print current access token's User ID
                Loggr.Log(aToken.UserId);
                // Print current access token's granted permissions
                foreach (string perm in aToken.Permissions)
                {
                    Loggr.Log(perm);
                }
                OnInitComplete.Dispatch(true);
            }
            else
            {
                Loggr.Log("Fail to login in FB");
                OnInitComplete.Dispatch(false);
            }
        }
    }
}