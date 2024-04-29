using Kitbag.Builder.CQRS.Core.Queries;
using Kitbag.Builder.CQRS.Dapper.Sql;

namespace Kitbag.Builder.CQRS.Dapper.Queries;

public interface IDapperQuery<T> : IQuery<T>, IDapperQuery
{
}

public interface IDapperQuery : IQuery
{
    public ISqlBuilder SqlBuilder { get; }
}