namespace Kitbag.Builder.CQRS.Core.Queries.DTO
{
    public class SortingConfiguration
    {
        public string? Field { get; set; }
        public SortOrder? Order { get; set; }

        public enum SortOrder
        {
            Asc,
            Desc
        }
    }
}