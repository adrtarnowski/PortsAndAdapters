using System;
using System.Threading.Tasks;
using Kitbag.Builder.CQRS.Core.Commands;
using Kitbag.Builder.Persistence.Core.Common.Logs;

namespace Kitbag.Persistence.EntityFramework.Audit.Common
{
    public class AuditTrailCommandHandlerDecorator<TCommand> : ICommandHandler<TCommand>
        where TCommand : class, ICommand
    {
        private readonly IAuditTrailProvider _auditTrailProvider;
        private readonly ICommandHandler<TCommand> _decoratedHandler;

        public AuditTrailCommandHandlerDecorator(
            IAuditTrailProvider unitOfWork, 
            ICommandHandler<TCommand> decoratedHandler)
        {
            _auditTrailProvider = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _decoratedHandler = decoratedHandler ?? throw new ArgumentNullException(nameof(decoratedHandler));
        }

        public async Task HandleAsync(TCommand command)
        {
            await _decoratedHandler.HandleAsync(command);
            _auditTrailProvider.LogChanges();
        }
    }
}