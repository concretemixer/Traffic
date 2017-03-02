using System;
using Commons.SN.Facebook.Commands;
using Commons.Utils;
using Commons.Utils.Commands;
using Facebook.Unity;
using RSG;

namespace Commons.SN.Facebook
{
    
    public class FacebookSN : ISocialNetwork
    {
        UnityEventProvider eventProvider;

        public bool IsLoggedIn
        {
            get
            {
                return FB.IsLoggedIn;
            }
        }

        public void LogInBefore(SNAction _action)
        {
            throw new NotImplementedException();
        }

        public IPromise Init()
        {
            return runCommand(new InitFBCommand());
        }

        public void SetEventProvider(UnityEventProvider _eventProvider)
        {
            eventProvider = _eventProvider;
        }

        public IPromise<ISNUser[]> LoadFriends()
        {
            throw new NotImplementedException();
        }

        public IPromise Login()
        {
            return Promise.Sequence(
                Init,
                () => runCommand(new LoginFBCommand(eventProvider))
            );
        }

        public IPromise Post(IPostData _data)
        {
            return Promise.Sequence(
                () => runCommand(new LoginFBCommand(eventProvider)),
                () => runCommand(new PostToFBCommand(eventProvider).SetData(_data))
            );
        }

        IPromise runCommand(IAsyncCommand _command)
        {
            var promise = new Promise();
            _command.Run()
                .Then(c => promise.Resolve())
                .Catch(promise.Reject);

            return promise;
        }
    }
}