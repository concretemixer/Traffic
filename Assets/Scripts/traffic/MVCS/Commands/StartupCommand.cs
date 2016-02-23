using Commons.UI;
using strange.extensions.command.impl;
using UnityEngine;

namespace Traffic.MVCS.Commands
{
    public class StartupCommand : Command
    {
        [Inject]
        public IUIManager UI { private get; set; }

        public override void Execute()
        {
            AudioSource gameMusic = GameObject.Find("GameMusic").GetComponent<AudioSource>();
            AudioSource menuMusic = GameObject.Find("MenuMusic").GetComponent<AudioSource>();

            gameMusic.volume = PlayerPrefs.GetFloat("volume.music", 1);
            menuMusic.volume = PlayerPrefs.GetFloat("volume.sound", 1);

            // UI.Show(UIMap.Id.ScreenDebug);
            UI.Show(UIMap.Id.ScreenMain);

            //UI.Show(UIMap.Id.InfoMessage);
        }
    }
}