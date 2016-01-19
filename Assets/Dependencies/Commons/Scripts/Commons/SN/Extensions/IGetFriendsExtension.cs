using RSG;

namespace Commons.SN.Extensions
{
    public interface IGetFriendsExtension : IExtension
    {
        IPromise<ISNUser[]> Execute();
    }
}