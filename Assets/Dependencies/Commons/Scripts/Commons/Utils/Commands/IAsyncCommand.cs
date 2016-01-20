using RSG;

namespace Commons.Utils.Commands
{
    public interface IAsyncCommand {
        IPromise<IAsyncCommand> Run();
    }
}