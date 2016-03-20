using System;
using System.Collections.Generic;

namespace Commons.UI
{
    public class UIMap
    {
        public static Dictionary<Id, string> map = new Dictionary<Id, string>
        {
            {Id.ScreenHUD, "UI/ScreenHUDView"},
            {Id.PauseMenu, "UI/ScreenPauseMenu"},
            {Id.LevelFailedMenu, "UI/LevelFailedMenu"},
            {Id.LevelDoneMenu, "UI/LevelDoneMenu"},
            {Id.TutorialDoneMenu,"UI/TutorialDoneMenu"},
            {Id.LevelListScreen, "UI/LevelListScreen"},
            { Id.ScreenMain, "UI/StartGameScreen" },
            { Id.ScreenLoading, "UI/ScreenLoading" },
            { Id.ScreenTutorial, "UI/TutorialStepScreen" },
            { Id.ScreenDebug, "UI/ScreenDebug" },
            { Id.ScreenSettings, "UI/SettingsMenu" },
            { Id.InfoMessage, "UI/InfoMessage" },
            { Id.NoTriesMessage, "UI/NoTriesMessage" },
            { Id.LevelPackDoneMessage, "UI/LevelPackDoneMessage" },
            { Id.GameCompleteMessage, "UI/GameCompleteMessage" }
        };

        public enum Id
        {
            ScreenHUD,
            PauseMenu,
            LevelDoneMenu,
            TutorialDoneMenu,
            LevelFailedMenu,
            LevelListScreen,
            ScreenMain,
            ScreenLoading,
            ScreenTutorial,
            ScreenDebug,
            ScreenSettings,
            InfoMessage,
            NoTriesMessage,
            LevelPackDoneMessage,
            GameCompleteMessage
        }

        public static string GetPath(UIMap.Id _id)
        {
            string path;
            if (!map.TryGetValue(_id, out path))
                throw new ArgumentException(string.Format("Undefined resource for UI id: {0}", _id.ToString()));

            return map[_id];
        }
    }
}