namespace Kitbag.Builder.CQRS.Core.Queries.DTO
{
    public class FileResult
    {
        public string? Filename { get; set; }
        public byte[] Content { get; set; } = new byte[0];
        public string? ContentType { get; set; }
    }
}