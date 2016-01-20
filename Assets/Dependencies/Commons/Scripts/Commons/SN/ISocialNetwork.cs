using RSG;

namespace Commons.SN
{
    public interface ISocialNetwork
    {
        IPromise Init();
        IPromise Login();
        bool IsLoggedIn { get; }
        IPromise<ISNUser[]> LoadFriends();
        void LogInBefore(SNAction _action);
        IPromise Post(IPostData _data);
    }
    public delegate void SNAction();
}