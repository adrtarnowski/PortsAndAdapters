namespace Kitbag.Builder.CQRS.Dapper.Queries;

public interface IResultWithTotalRows
{
    int? TotalRows { get; set; }
}