using strange.extensions.mediation.impl;
using UnityEngine;

namespace Traffic.MVCS.Views.UI.Loading
{
    public class LoadingScreenMediator : Mediator
    {
        [Inject]
        public LoadingScreenView view { set; private get; }
		
        override public void OnRegister()
        {
            view.Layout(Screen.width, Screen.height);   
        }
    }
}