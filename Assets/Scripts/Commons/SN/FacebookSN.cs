using strange.extensions.signal.impl;
using Facebook.Unity;
using Commons.Utils;
using System.Collections.Generic;
using System;
using System.Linq;

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

        public void Publish(string _title, string _message, Action _complete)
        {
            if (!FB.IsInitialized || !FB.IsLoggedIn)
            {
                Init();
                OnInitComplete.AddOnce((isSuccess) =>
                {
                    if (isSuccess)
                        publish(_title, _message, _complete);
                    else
                        _complete();
                });
            }
            else
                publish(_title, _message, _complete);
        }

        void publish(string _title, string _message, Action _complete)
        {
            unityEvents.onGui.AddOnce(() =>
            {
                FB.LogInWithPublishPermissions(new List<string>() { "publish_actions" }, (result) =>
                {
                    if (AccessToken.CurrentAccessToken.Permissions.Contains("publish_actions"))
                    {
                        Loggr.Log("have publish actions");
                        FB.ShareLink(new Uri("https://www.facebook.com/alexi.smallruss"), _title, _message, null, (shareResult) =>
                        {
                            if (shareResult.Cancelled || shareResult.Error != null)
                            {
                                Loggr.Error("publish falied" + shareResult.Error);
                                _complete();
                            }
                            else
                            {
                                Loggr.Log("successefly published");
                                _complete();
                            }
                        });
                    }
                    else
                    {
                        Loggr.Log("no publish actions");
                        _complete();
                    }
                });
            });
        }

        public void AddPremission(string _premission)
        {
            if (!permissions.Contains(_premission))
                permissions.Add(_premission);
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

        void onLoginComplete(ILoginResult _result = null)
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