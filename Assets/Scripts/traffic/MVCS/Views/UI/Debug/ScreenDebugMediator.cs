using Commons.SN;
using Commons.SN.Facebook;
using Commons.Utils;
using strange.extensions.mediation.impl;
using Traffic.MVCS.Commands.Signals;

namespace Traffic.MVCS.Views.UI.Debug
{
    public class ScreenDebugMediator : Mediator
    {
        [Inject]
        public ScreenDebugView view { set; private get; }

  //      [Inject]
   //     public FacebookSN facebook { set; private get; }

        [Inject]
        public LoginCompleteSignal loginComplete { private get; set; }

        [Inject]
        public LoginToSNSignal tryLogin { private get; set; }

        [Inject]
        public GetSNFriendsSignal getFriends { private get; set; }

        [Inject]
        public FriendsLoadedSignal friendsLoaded { private get; set; }
        public override void OnRegister()
        {
            view.loginToFBButton.onClick.AddListener(tryLogin.Dispatch);
            loginComplete.AddListener(onLoginComplete);

            view.getFBFriendsButton.onClick.AddListener(getFriends.Dispatch);
            friendsLoaded.AddListener(onFriendsLoaded);

            view.postToFBButton.onClick.AddListener(postClickHandler);
        }

        void postClickHandler()
        {
            var postData = new PostData()
            {
                Link = "https://example.com/myapp/?storyID=thelarch",
                LinkName = "The Larch",
                LinkCaption = "I thought up a witty tagline about larches",
                LinkDescription = "There are a lot of larch trees around here, aren't there?",
                Picture = "https://example.com/myapp/assets/1/larch.jpg"
            };

            /*
            facebook.Post(postData).Done(
                () =>
                {
                    Loggr.Log("Post successful!");
                },
                (e) =>
                {
                    Loggr.Log(e.Message);
                }
            );*/
        }

        void onFriendsLoaded(ISNUser[] _friends)
        {
            Loggr.Log("Friends received!");
            foreach (var friend in _friends)
            {
                Loggr.Log(friend.ToString());
            }
        }

        void onLoginComplete()
        {
            Loggr.Log("debug: login complete.");
        }
    }
}