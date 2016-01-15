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
            {Id.LevelListScreen, "UI/LevelListScreen"},
            { Id.ScreenMain, "" },
            { Id.ScreenLoading, "UI/ScreenLoading" },
            { Id.ScreenTutorial, "UI/TutorialStepScreen" },
            { Id.ScreenDebug, "UI/ScreenDebug" }
        };

        public enum Id
        {
            ScreenHUD,
            PauseMenu,
            LevelDoneMenu,
            LevelFailedMenu,
            LevelListScreen,
            ScreenMain,
            ScreenLoading,
            ScreenTutorial,
            ScreenDebug
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