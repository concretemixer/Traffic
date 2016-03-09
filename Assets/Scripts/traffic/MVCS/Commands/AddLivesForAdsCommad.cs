using Commons.UI;
using Commons.Utils;
using strange.extensions.command.impl;
using Traffic.Core;
using Traffic.MVCS.Views.UI;
using Traffic.MVCS.Models;

namespace Traffic.MVCS.Commands
{
    public class AddLivesForAdsCommad : Command
    {
        [Inject]
        public ILevelListModel levels { get; set; }

        [Inject]
        public IUIManager UI { get; set; }

        [Inject]
        public ILocaleService localeService { get; set; }

        public override void Execute()
        {
            Loggr.Log("increease tryies for ad watching");
            var receivedTries = levels.TriesTotal; //TODO: подумать сколько нужно выдавать попыток
            levels.TriesLeft = receivedTries;

            var view = UI.Show<InfoMessageView>(UIMap.Id.InfoMessage);
            view.SetCaption(localeService.ProcessString("%NEW_TRIES_TITLE%"));
            view.SetText(localeService.ProcessString("%NEW_TRIES_DESC%"));
            view.SetMessageMode(true);
        }
    }
}