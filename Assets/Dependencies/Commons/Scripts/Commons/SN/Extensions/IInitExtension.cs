using RSG;

namespace Commons.SN.Extensions
{
    public interface IInitExtension : IExtension
    {
        IPromise Execute();
    }
}