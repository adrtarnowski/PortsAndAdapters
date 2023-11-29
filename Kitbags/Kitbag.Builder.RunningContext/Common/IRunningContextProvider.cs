namespace Kitbag.Builder.RunningContext.Common;

public interface IRunningContextProvider
{
    public string? UserId { get; }
    public Guid? RequestId { get; }
    public Guid? CorrelationId { get; }
    public Guid? ConversationId { get; }
}