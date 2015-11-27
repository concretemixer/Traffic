using Commons.Utils;
using strange.extensions.command.impl;

namespace Traffic.MVCS.Commands.Init
{
    public class LoadConfigCommand : Command
    {
        public override void Execute()
        {
            Logger.Log("load config");
        }
    }
}