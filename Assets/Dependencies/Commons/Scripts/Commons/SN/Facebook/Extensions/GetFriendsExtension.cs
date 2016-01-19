using System.Collections.Generic;
using Commons.SN.Extensions;
using Commons.Utils;
using Facebook.Unity;
using RSG;

namespace Commons.SN.Facebook.Extensions
{
    public class GetFriendsExtension : ExtensionBase, IGetFriendsExtension
    {
        private static List<ISNUser> friends = new List<ISNUser>();

        public GetFriendsExtension(ISocialNetwork _network) : base(_network) { }

        public IPromise<ISNUser[]> Execute()
        {
            var promise = new Promise<ISNUser[]>();

            network.GetExt<ILoginExtension>().Execute()
            .Then(() =>
            {
                getFriendsRequest()
                    .Then(_friends => promise.Resolve(_friends))
                    .Catch(promise.Reject);
            })
            .Catch(promise.Reject);

            return promise;
        }

        private IPromise<ISNUser[]> getFriendsRequest()
        {
            var promise = new Promise<ISNUser[]>();

            string queryString = "/me/friends?fields=id,first_name,picture.width(128).height(128)&limit=100";
            FB.API(queryString, HttpMethod.GET, (result) =>
            {
                Loggr.Log("Friends received: ", result.RawResult);

                object dataList;
                if (result.ResultDictionary.TryGetValue("data", out dataList))
                {
                    var friendsList = (List<object>)dataList;
                    CacheFriends(friendsList);
                    promise.Resolve(friends.ToArray());
                }
            });

            return promise;
        }

        private static void CacheFriends(List<object> _friends)
        {
            foreach (var friendInfo in _friends)
                Loggr.Log(friendInfo.ToString());
        }
    }
}