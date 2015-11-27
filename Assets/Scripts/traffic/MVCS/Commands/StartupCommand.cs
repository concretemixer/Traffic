using strange.extensions.command.impl;
using UnityEngine;

namespace Traffic.MVCS.Commands
{
    public class StartupCommand : Command
    {
        [Inject]
        public GameObject stage { private get; set; }

        public override void Execute()
        {

        }
    }
}