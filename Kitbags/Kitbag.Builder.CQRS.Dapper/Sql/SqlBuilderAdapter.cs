using Dapper;

namespace Kitbag.Builder.CQRS.Dapper.Sql;

public class SqlBuilderAdapter : ISqlBuilder
{
    private readonly ExtendedSqlBuilder _sqlBuilder;

    public SqlBuilderAdapter()
    {
        _sqlBuilder = new ExtendedSqlBuilder();
    }

    public ISqlBuilder Offset(string sql, dynamic? parameters = null)
    {
        _sqlBuilder.Offset(sql, parameters);
        return this;
    }

    public ISqlBuilder FetchNext(string sql, dynamic? parameters = null)
    {
        _sqlBuilder.FetchNext(sql, parameters);
        return this;
    }

    public ISqlBuilder DefaultOrderBy()
    {
        _sqlBuilder.DefaultOrderBy();
        return this;
    }

    public SqlBuilder.Template AddTemplate(string sql, dynamic? parameters = null)
    {
        return _sqlBuilder.AddTemplate(sql, parameters);
    }

    public ISqlBuilder Intersect(string sql, dynamic? parameters = null)
    {
        _sqlBuilder.Intersect(sql, parameters);
        return this;
    }

    public ISqlBuilder InnerJoin(string sql, dynamic? parameters = null)
    {
        _sqlBuilder.InnerJoin(sql, parameters);
        return this;
    }

    public ISqlBuilder LeftJoin(string sql, dynamic? parameters = null)
    {
        _sqlBuilder.LeftJoin(sql, parameters);
        return this;
    }

    public ISqlBuilder RightJoin(string sql, dynamic? parameters = null)
    {
        _sqlBuilder.RightJoin(sql, parameters);
        return this;
    }

    public ISqlBuilder Where(string sql, dynamic? parameters = null)
    {
        _sqlBuilder.Where(sql, parameters);
        return this;
    }

    public ISqlBuilder OrWhere(string sql, dynamic? parameters = null)
    {
        _sqlBuilder.OrWhere(sql, parameters);
        return this;
    }

    public ISqlBuilder OrderBy(string sql, dynamic? parameters = null)
    {
        _sqlBuilder.OrderBy(sql, parameters);
        return this;
    }

    public ISqlBuilder Select(string sql, dynamic? parameters = null)
    {
        _sqlBuilder.Select(sql, parameters);
        return this;
    }

    public ISqlBuilder AddParameters(dynamic parameters)
    {
        _sqlBuilder.AddParameters(parameters);
        return this;
    }

    public ISqlBuilder Join(string sql, dynamic? parameters = null)
    {
        _sqlBuilder.Join(sql, parameters);
        return this;
    }

    public ISqlBuilder GroupBy(string sql, dynamic? parameters = null)
    {
        _sqlBuilder.GroupBy(sql, parameters);
        return this;
    }

    public ISqlBuilder Having(string sql, dynamic? parameters = null)
    {
        _sqlBuilder.Having(sql, parameters);
        return this;
    }
}