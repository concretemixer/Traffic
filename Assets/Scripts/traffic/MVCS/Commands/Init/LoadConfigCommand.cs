using Commons.Utils;
using strange.extensions.command.impl;
using JsonFx.Json;
using System.Collections.Generic;
using Traffic.Core;
using UnityEngine;

namespace Traffic.MVCS.Commands.Init
{
    public class LoadConfigCommand : Command
    {
        [Inject]
        public ILevelListModel levels
        {
            get;
            set;
        }

        public override void Execute()
        {
          //  return;

            string json = Resources.Load<TextAsset>("config/config").text;          
            GameplayConfig cfg = new GameplayConfig();         
            cfg = JsonReader.Deserialize<GameplayConfig>(json);

            levels.LevelConfigs = new LevelConfig[cfg.levels.Count];
            foreach(string key in cfg.levels.Keys) {
                int index = int.Parse(key)-1;
                levels.LevelConfigs[index] = cfg.levels[key];
            }
        }
    }
}