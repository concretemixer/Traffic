using Commons.SN.Extensions;

namespace Commons.SN
{
    public interface ISocialNetwork
    {
        void Init();
        bool IsSupportedExt<TExtension>() where TExtension : IExtension;
        TExtension GetExt<TExtension>() where TExtension : IExtension;
    }
}