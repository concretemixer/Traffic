using Commons.UI;
using Commons.Utils;
using strange.extensions.command.impl;
using Traffic.Core;
using Traffic.MVCS.Views.UI;

namespace Traffic.MVCS.Commands
{
    public class AddLivesForAdsCommad : Command
    {
        [Inject]
        public ILevelListModel levels { get; set; }

        [Inject]
        public IUIManager UI { get; set; }

        public override void Execute()
        {
            Loggr.Log("increease tryies for ad watching");
            var receivedTries = levels.TriesTotal; //TODO: подумать сколько нужно выдавать попыток
            levels.TriesLeft = receivedTries;

            var view = UI.Show<InfoMessageView>(UIMap.Id.InfoMessage);
            view.SetCaption("Congrats!");
            view.SetText("Your tries completely restored!");
            view.SetMessageMode(true);
        }
    }
}