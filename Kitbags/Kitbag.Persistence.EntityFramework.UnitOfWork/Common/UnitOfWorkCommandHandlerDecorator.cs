using System;
using System.Threading.Tasks;
using Kitbag.Builder.CQRS.Core.Commands;
using Kitbag.Builder.Persistence.Core.Common;

namespace Kitbag.Persistence.EntityFramework.UnitOfWork.Common
{
    public class UnitOfWorkCommandHandlerDecorator<TCommand> : ICommandHandler<TCommand>
        where TCommand : class, ICommand
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICommandHandler<TCommand> _decoratedHandler;

        public UnitOfWorkCommandHandlerDecorator(
            IUnitOfWork unitOfWork, 
            ICommandHandler<TCommand> decoratedHandler)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _decoratedHandler = decoratedHandler ?? throw new ArgumentNullException(nameof(decoratedHandler));
        }

        public async Task HandleAsync(TCommand command)
        {
            await _decoratedHandler.HandleAsync(command);
            await _unitOfWork.CommitAsync();
        }
    }
}