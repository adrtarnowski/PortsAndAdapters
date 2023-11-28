using System;
using System.Threading.Tasks;
using Kitbag.Builder.CQRS.Core.Commands;

namespace Kitbag.Persistence.EntityFramework.UnitOfWork.Common
{
    public class UnitOfWorkCommandHandlerDecorator<TCommand> : ICommandHandler<TCommand>
        where TCommand : class, ICommand
    {
        private readonly ITransactionalUnitOfWork _unitOfWork;
        private readonly ICommandHandler<TCommand> _decoratedHandler;

        public UnitOfWorkCommandHandlerDecorator(
            ITransactionalUnitOfWork unitOfWork, 
            ICommandHandler<TCommand> decoratedHandler)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _decoratedHandler = decoratedHandler ?? throw new ArgumentNullException(nameof(decoratedHandler));
        }

        public async Task Handle(TCommand command)
        {
            if (_unitOfWork.HasActiveTransaction)
            {
                await _decoratedHandler.Handle(command);
            }
            else
            {
                await using var transaction = await _unitOfWork.BeginTransactionAsync();
                await _decoratedHandler.Handle(command);
                await _unitOfWork.CommitTransactionAsync(transaction!);
            }
        }
    }
}