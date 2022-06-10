using Kitbag.Builder.CQRS.Dapper.Sql;

namespace Kitbag.Builder.CQRS.Dapper.Queries
{
    public class DapperQuery<T> : IDapperQuery<T>
    {
        public DapperQuery()
        {
            SqlBuilder = new SqlBuilderAdapter();
        }

        public ISqlBuilder SqlBuilder { get; }
    }
}