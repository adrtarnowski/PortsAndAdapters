using System.Threading.Tasks;

namespace Kitbag.Builder.CQRS.Core.Commands
{
    public interface ICommandDispatcher
    {
        Task SendAsync<T>(T command) where T : class, ICommand;
    }
}