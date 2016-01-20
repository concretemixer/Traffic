using Commons.SN.Facebook;
using strange.extensions.command.impl;
using Traffic.MVCS.Commands.Signals;

namespace Traffic.MVCS.Commands.SN
{
    public class GetFrindsCommand : Command
    {
        [Inject]
        public FacebookSN facebook { private get; set; }

        [Inject]
        public FriendsLoadedSignal friendsLoaded { private get; set; }

        public override void Execute()
        {
            Retain();
            facebook.LoadFriends().Done((_friends) =>
            {
                friendsLoaded.Dispatch(_friends);
                Release();
            });
        }
    }
}