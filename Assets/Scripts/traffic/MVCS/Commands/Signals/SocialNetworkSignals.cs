using Commons.SN;
using strange.extensions.signal.impl;

namespace Traffic.MVCS.Commands.Signals
{
    public class LoginToSNSignal : Signal { }
    public class LoginCompleteSignal : Signal { }
    public class GetSNFriendsSignal : Signal { }
    public class FriendsLoadedSignal : Signal<ISNUser[]> { }
}