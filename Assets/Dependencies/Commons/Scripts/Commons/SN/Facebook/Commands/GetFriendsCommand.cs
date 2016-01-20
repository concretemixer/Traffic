using System.Collections.Generic;
using Commons.Utils;
using Commons.Utils.Commands;
using Facebook.Unity;
using RSG;

namespace Commons.SN.Facebook.Commands
{
    public class GetFriendsCommand : IAsyncCommand
    {
        public ISNUser[] Users { get; private set; }

        public IPromise<IAsyncCommand> Run()
        {
            var promise = new Promise<IAsyncCommand>();

            string queryString = "/me/friends?fields=id,first_name,picture.width(128).height(128)&limit=100";
            FB.API(queryString, HttpMethod.GET, (result) =>
            {
                Loggr.Log("Friends received: ", result.RawResult);

                object dataList;
                if (result.ResultDictionary.TryGetValue("data", out dataList))
                {
                    var friendsList = (List<object>)dataList;
                    Users = parseUsers(friendsList);
                    promise.Resolve(this);
                }
            });

            return promise;
        }

        ISNUser[] parseUsers(List<object> _rawFriends)
        {
            var friends = new List<ISNUser>();

            return friends.ToArray();
        }
    }
}