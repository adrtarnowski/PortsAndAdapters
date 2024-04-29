using Kitbag.Builder.CQRS.Core.Commands;
using Kitbag.Persistence.EntityFramework.UnitOfWork.Common;

namespace Kitbag.Builder.Outbox.EntityFramework.Common;

public class OutboxHandlerDecorator<TCommand> : ICommandHandler<TCommand>
    where TCommand : class, ICommand
{
    private readonly ICommandHandler<TCommand> _decoratedHandler;
    private readonly IOutboxEventDispatcher _outboxEventDispatcher;
    private readonly ITransactionalUnitOfWork _unitOfWork;

    public OutboxHandlerDecorator(
        ICommandHandler<TCommand> decoratedHandler,
        IOutboxEventDispatcher outboxEventDispatcher,
        ITransactionalUnitOfWork unitOfWork)
    {
        _decoratedHandler = decoratedHandler;
        _outboxEventDispatcher = outboxEventDispatcher;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(TCommand command)
    {
        if (_unitOfWork.HasActiveTransaction)
        {
            await HandleAndDispatch(command);
        }
        else
        {
            using var transaction = await _unitOfWork.BeginTransactionAsync();
            await HandleAndDispatch(command);
            await _unitOfWork.CommitTransactionAsync(transaction!);
        }
            
    }

    private async Task HandleAndDispatch(TCommand command)
    {
        await _decoratedHandler.Handle(command);
        await _outboxEventDispatcher.EnqueueEvents();
    }
}