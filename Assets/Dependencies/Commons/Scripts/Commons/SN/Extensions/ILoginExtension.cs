using RSG;

namespace Commons.SN.Extensions
{
    public interface ILoginExtension : IExtension
    {
        IPromise Execute();
    }
}