using strange.extensions.command.impl;
using Commons.Utils;
using Commons.SN;

namespace Traffic.MVCS.Commands.Init
{
    public class TryToLoginFBCommand : Command
    {
        [Inject]
        public UnityEventProvider eventProvider { private get; set; }

        public override void Execute()
        {
            Retain();

            injectionBinder.Bind<FacebookSN>().ToSingleton();
            var facebook = injectionBinder.GetInstance<FacebookSN>();

            facebook.OnInitComplete.AddOnce(onCompleteInit);
            facebook.Init();
        }

        void onCompleteInit(bool isSuccess)
        {
            Release();
        }
    }
}