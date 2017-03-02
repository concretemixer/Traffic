using UnityEngine;
using System.Collections;
using Commons.Utils;
using strange.extensions.command.impl;
using LocalyticsUnity;    
using System;


namespace Traffic.MVCS.Commands
{
   
    
    public class InitializeLocalyticsCommand : Command
    {


        public override void Execute()
        {
#if (UNITY_ANDROID || UNITY_IOS)
            Localytics.LoggingEnabled = true;
            Localytics.SessionTimeoutInterval = 300;
            Localytics.RegisterForAnalyticsEvents();
            Localytics.RegisterForMessagingEvents();
#endif
        }
        
    }
      
}     