using strange.extensions.command.impl;
using System.Collections;
using System.Collections.Generic;
using Traffic.Core;
using Traffic.Components;
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

        [Inject(EntryPoint.Container.Stage)]
        public GameObject stage { get; set; }

        private IEnumerator WaitForRequest(WWW www)
        {
            yield return www;

            // check for errors
            if (www.error != null)
            {
                Debug.Log("WWW Error: " + www.error);
            }
        }

        public override void Execute()
        {
             
            //https://docs.google.com/uc?authuser=0&id=0B0US96bFVj6rdGxCd1ZyOS1DLXM&export=download

            string json = Resources.Load<TextAsset>("config/config").text;

//            var cfg = JsonUtility.FromJson<GameplayConfig>(json);
            var cfg = Newtonsoft.Json.JsonConvert.DeserializeObject<GameplayConfig>(json);



//            cfg = JsonReader.Deserialize<GameplayConfig>(json);

            levels.LevelConfigs = new LevelConfig[cfg.levels.Count];
            int c = levels.LevelNames.Length;
            foreach (string key in cfg.levels.Keys)
            {
                int index = -1;
                for (int a = 0; a < levels.LevelNames.Length; a++)
                {
                    if (levels.LevelNames[a] == key)
                    {
                        c--;
                        index = a;
                        levels.LevelConfigs[index] = cfg.levels[key];
                        break;
                    }
                }

                if (index == -1)
                {
                    Debug.LogError("Config error: " + key);
                }
            }
            if (c > 0)
            {
                Debug.LogError(c.ToString() + " level(s) without cfg");
            }

            EntryPoint.DebugMessage = "Local config, v" + cfg.version;

            // return;
            // EntryPoint entryPoint = stage.GetComponentInParent<EntryPoint>();
            
            // WWW req = new WWW("https://docs.google.com/uc?authuser=0&id=0B0US96bFVj6rdGxCd1ZyOS1DLXM&export=download");
            // entryPoint.StartCoroutine(WaitForRequest(req));

            // do
            // {
            //     System.Threading.Thread.Sleep(100);
            // } while (!req.isDone);

            // if (req.error == null)
            // {
            //     GameplayConfig cfg2 = new GameplayConfig();
            //     try
            //     {
            //         cfg2 = JsonReader.Deserialize<GameplayConfig>(req.text);

            //         if (cfg2.version > cfg.version)
            //         {
            //             Debug.Log("New cfg version: " + cfg2.version + ", was " + cfg.version);
            //             levels.LevelConfigs = new LevelConfig[cfg2.levels.Count];
            //             foreach (string key in cfg2.levels.Keys)
            //             {
            //                 int index = int.Parse(key);
            //                 levels.LevelConfigs[index] = cfg2.levels[key];
            //             }
            //             EntryPoint.DebugMessage = "Config downloaded OK, v" + cfg2.version;
            //             Debug.Log(EntryPoint.DebugMessage);
            //         }                    
            //     }
            //     catch (Exception ex)
            //     {
            //         EntryPoint.DebugMessage = "Error parsing config json";
            //         Debug.LogException(ex);
            //     }
            // }
            // else
            // {                
            //     EntryPoint.DebugMessage = "Error dowloading config";
            //     Debug.Log(EntryPoint.DebugMessage);
            // }

        }
    }
}