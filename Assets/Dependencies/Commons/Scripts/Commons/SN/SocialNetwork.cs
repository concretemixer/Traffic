using System;
using System.Collections.Generic;
using Commons.SN.Extensions;

namespace Commons.SN
{
    public abstract class SocialNetwork
    {
        protected List<IExtension> extensions = new List<IExtension>();

        public void Init()
        {
            ConfigureExtensions();
        }

        protected abstract void ConfigureExtensions();

        protected void AddExtension(IExtension _extension)
        {
            if (extensions.Contains(_extension))
                throw new ArgumentException(string.Format("Try to add already existing extension: {0}", _extension));

            extensions.Add(_extension);
        }

        public TExtension GetExt<TExtension>() where TExtension : IExtension
        {
            var extType = typeof(TExtension);
            foreach (var ext in extensions)
            {
                if (ext is TExtension)
                    return (TExtension)ext;
            }

            throw new ArgumentException(string.Format("Undefined extension: {0}", extType.ToString()));
        }

        public bool IsSupportedExt<TExtension>() where TExtension : IExtension
        {
            var extType = typeof(TExtension);
            foreach (var ext in extensions)
            {
                if (ext.GetType() == extType)
                    return true;
            }

            return false;
        }
    }
}