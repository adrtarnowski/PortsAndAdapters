using Dapper;
using Kitbag.Builder.CQRS.Core.Queries;

namespace Kitbag.Builder.CQRS.Dapper.Queries.Providers;

public interface IDynamicParametersExtractor
{
    public DynamicParameters ConfigureParameters(IGridQuery query);
}