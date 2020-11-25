using System.Threading.Tasks;

namespace Kitbag.Builder.CQRS.Core.Commands
{
    public interface ICommandHandler<in TCommand> where TCommand : class, ICommand 
    {
        Task HandleAsync(TCommand command);
    }
}