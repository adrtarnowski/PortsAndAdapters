namespace Kitbag.Builder.Outbox.EntityFramework.Common
{
    public interface IOutboxEventDispatcher
    {
        /// <summary>
        /// Local domain events dispatcher
        /// </summary>
        /// <returns></returns>
        Task DispatchScopedDomainEvents();
        
        /// <summary>
        /// Enqueue domain events to outbox (for scheduler)
        /// </summary>
        /// <returns></returns>
        Task EnqueueEvents();
    }
}