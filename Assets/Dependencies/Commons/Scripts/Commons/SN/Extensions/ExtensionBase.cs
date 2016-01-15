using System;
using RSG;

namespace Commons.SN.Extensions
{
    public class ExtensionBase
    {
        protected ISocialNetwork network;
        
        public ExtensionBase(ISocialNetwork _network)
        {
            network = _network;
        }
    }
}