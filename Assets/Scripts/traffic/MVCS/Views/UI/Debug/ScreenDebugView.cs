using strange.extensions.mediation.impl;
using UnityEngine;
using UnityEngine.UI;

namespace Traffic.MVCS.Views.UI.Debug
{
    public class ScreenDebugView : View
    {
        [SerializeField]
        public Button postToFBButton;
        
        [SerializeField]
        public Button getFBFriendsButton;
        
        [SerializeField]
        public Button loginToFBButton;
    }
}