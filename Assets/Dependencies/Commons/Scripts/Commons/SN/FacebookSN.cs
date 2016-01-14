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
        List<String> permissions = new List<string>() { "public_profile", "user_friends" };

        void _initFb(ActionQueue.Complete _callback)
        {
            if (FB.IsInitialized)
                _onInitComplete(_callback);
            else
                FB.Init(() => _onInitComplete(_callback));
        }

        void _onInitComplete(ActionQueue.Complete _callback)
        {
            if (FB.IsInitialized)
            {
                FB.ActivateApp();
                Loggr.Log("FB app initialized");
            }
            else
                Loggr.Log("Failed to init FB");

            _callback();
        }

        void _loginWithReadPerms(ActionQueue.Complete _callback)
        {
            if (!FB.IsLoggedIn)
                unityEvents.onGui.AddOnce(() => FB.LogInWithReadPermissions(permissions, (_result) => _onLoginComplete(_callback)));
            else
                _onLoginComplete(_callback);
        }

        void _onLoginComplete(ActionQueue.Complete _callback)
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

            _callback();
        }

        void _loginWithPublish(ActionQueue.Complete _complete)
        {
            unityEvents.onGui.AddOnce(() =>
            {
                FB.LogInWithPublishPermissions(new List<string>() { "publish_actions" }, (result) =>
                {
                    if (AccessToken.CurrentAccessToken.Permissions.Contains("publish_actions"))
                        Loggr.Log("have publish actions");
                    else
                        Loggr.Log("no publish actions");

                    _complete();
                });
            });
        }

        void _share(string _title, string _message, ActionQueue.Complete _complete)
        {
            if (isHavePublishAccess)
            {
                unityEvents.onGui.AddOnce(() =>
                {
                    FB.ShareLink(new Uri("https://www.facebook.com/alexi.smallruss"), _title, _message, null, (shareResult) =>
                    {
                        if (shareResult.Cancelled || shareResult.Error != null)
                            Loggr.Error("publish falied" + shareResult.Error);
                        else
                            Loggr.Log("successefly published");

                        _complete();
                    });
                });
            }
            else
                _complete();
        }

        public void Init(ActionQueue.Complete _complete = null)
        {
            Loggr.Log("Try to init facebook");

            new ActionQueue()
            .Series(
                _initFb,
                _loginWithReadPerms
            )
            .Run(_complete);
        }

        public void Publish(string _title, string _message, ActionQueue.Complete _complete)
        {
            new ActionQueue().Series(
                _initFb,
                _loginWithPublish,
                (_callback) =>
                {
                    _share(_title, _message, _callback);
                }
            ).Run(_complete);
        }

        public void AddPremission(string _premission)
        {
            if (!permissions.Contains(_premission))
                permissions.Add(_premission);
        }

        bool isHavePublishAccess
        {
            get
            {
                return AccessToken.CurrentAccessToken.Permissions.Contains("publish_actions");
            }
        }
    }
}

public class ActionQueue
{
    public delegate void Complete();

    public delegate void ActionFn(Complete _complete);
    Queue<ActionFn> actions;
    Complete completeCallback;

    public ActionQueue Series(params ActionFn[] _actions)
    {
        actions = new Queue<ActionFn>(_actions);
        return this;
    }

    public ActionQueue Run(Complete _callback)
    {
        completeCallback = _callback;
        onComplete();
        return this;
    }

    void onComplete()
    {
        if (actions.Count > 0)
        {
            var action = actions.Dequeue();
            action(onComplete);
        }
        else
        {
            if (completeCallback != null)
                completeCallback();
        }
    }
}