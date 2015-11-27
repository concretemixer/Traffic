using Commons.Utils;
using strange.extensions.command.impl;

namespace Traffic.MVCS.Commands.Init
{
	public class InitUICommand : Command
	{
		public override void Execute()
		{
			Logger.Log("init UI");
		}
	}
}