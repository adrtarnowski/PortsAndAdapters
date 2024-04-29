using Dapper;

namespace Kitbag.Builder.CQRS.Dapper.Sql;

internal class ExtendedSqlBuilder : SqlBuilder
{
    public SqlBuilder Offset(string sql, dynamic? parameters = null) =>
        AddClause("offset", sql, parameters, ", ", "OFFSET ", " ROWS\n", false);

    public SqlBuilder FetchNext(string sql, dynamic? parameters = null) =>
        AddClause("fetchnext", sql, parameters, ", ", "FETCH NEXT ", " ROWS ONLY\n", false);

    public SqlBuilder DefaultOrderBy() => OrderBy("(SELECT NULL)");
}