using Commons.SN.Facebook.Extensions;
using Commons.Utils;

namespace Commons.SN.Facebook
{
    public class FacebookSN : SocialNetwork
    {
        UnityEventProvider eventProvider;
        
        public void Init(UnityEventProvider _eventProvider)
        {
            eventProvider = _eventProvider;
            Init();
        }
        
        protected override void ConfigureExtensions()
        {
            AddExtension(new InitSNExtension(this));
            AddExtension(new LoginSNExtension(this, eventProvider));
        }
    }
}