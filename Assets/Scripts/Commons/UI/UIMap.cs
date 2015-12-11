using System;
using System.Collections.Generic;

namespace Commons.UI
{
    public class UIMap
    {
        public static Dictionary<Id, string> map = new Dictionary<Id, string>
        {
            { Id.ScreenMain, "" },
            { Id.ScreenLoading, "UI/ScreenLoading" }
        };

        public enum Id
        {
            ScreenMain,
            ScreenLoading
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