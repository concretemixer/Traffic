using Commons.Utils;
using strange.extensions.command.impl;

namespace Traffic.MVCS.Commands.Init
{
    public class CreateServiceItemsCommand : Command
    {
        public override void Execute()
        {
            var provider = InstantiateUtil.AddToPath<UnityEventProvider>("service.EventProvider");
            injectionBinder.Bind<UnityEventProvider>().To(provider);
        }
    }
}