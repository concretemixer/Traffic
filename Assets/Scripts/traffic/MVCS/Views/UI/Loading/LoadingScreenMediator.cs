using strange.extensions.mediation.impl;

namespace Traffic.MVCS.Views.UI.Loading
{
    public class LoadingScreenMediator : Mediator
    {
        [Inject]
        public LoadingScreenView View { set; private get; }
		
        override public void OnRegister()
        {
            View.SetStatus("default status");
        }
    }
}