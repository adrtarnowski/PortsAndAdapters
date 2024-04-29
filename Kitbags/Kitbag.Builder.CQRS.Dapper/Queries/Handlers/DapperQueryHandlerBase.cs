using Kitbag.Builder.CQRS.Dapper.Sql;

namespace Kitbag.Builder.CQRS.Dapper.Queries.Handlers;

public abstract class DapperQueryHandlerBase
{
    protected readonly ISqlConnectionFactory _connectionFactory;
    protected DapperQueryHandlerBase(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    protected virtual string TableOrViewName
    {
        get
        {
            var name = GetType().Name;

            return name
                .Substring(0, name.Length - "QueryHandler".Length);
        }
    }

    protected virtual string CreateSqlTemplate()
    {
        return $"SELECT /**select**/ FROM {TableOrViewName}\n" +
               $"/**where**/ /**orderby**/";
    }

    protected virtual void ConfigureQuery(IDapperQuery query)
    {
        query.SqlBuilder.Select("*");
    }
}