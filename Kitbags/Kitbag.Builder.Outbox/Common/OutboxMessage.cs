namespace Kitbag.Builder.Outbox.Common;

public class OutboxMessage
{
    public Guid Id { get; set; }
    
    public Guid BatchId { get; set; }
    public DateTime CreationDate { get; set; }
    
    public DateTime? ProcessedDate { get; set; }
    
    public string? Payload { get; set; }
    public string? Type { get; set; }
    public OutboxMessageDiscriminator Discriminator { get; set; }
    
}