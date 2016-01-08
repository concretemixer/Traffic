using Commons.UI;
using strange.extensions.command.impl;

namespace Traffic.MVCS.Commands
{
    public class StartupCommand : Command
    {
        [Inject]
        public IUIManager UI { private get; set; }

        public override void Execute()
        {
            UI.Show(UIMap.Id.LevelListScreen);
            UI.Show(UIMap.Id.ScreenDebug);
        }
    }
}