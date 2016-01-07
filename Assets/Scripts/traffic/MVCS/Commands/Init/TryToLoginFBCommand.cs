using strange.extensions.command.impl;
using Facebook.Unity;
using UnityEngine;
using System.Collections.Generic;
using Commons.Utils;

namespace Traffic.MVCS.Commands.Init
{
    public class TryToLoginFBCommand : Command
    {
        [Inject]
        public UnityEventProvider eventProvider { private get; set; }
        public override void Execute()
        {
            Retain();
            FB.Init(() =>
            {
                Debug.Log("App successufly inited");
                FB.ActivateApp();

                var perms = new List<string>() { "public_profile", "email", "user_friends" };

                eventProvider.onGui.AddOnce(() =>
                {
                    FB.LogInWithReadPermissions(perms, (result) =>
                    {
                        if (FB.IsLoggedIn)
                        {
                            var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
                            // Print current access token's User ID
                            Debug.Log(aToken.UserId);
                            // Print current access token's granted permissions
                            foreach (string perm in aToken.Permissions)
                            {
                                Debug.Log(perm);
                            }
                            Release();
                        }
                        else
                        {
                            Debug.Log("User cancelled login");
                            Release();
                        }
                    });
                });
            }, (isActive) =>
            {
                Debug.Log(string.Format("change active project: {0}", isActive));
            });
        }
    }
}