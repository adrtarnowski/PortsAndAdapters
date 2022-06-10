using System;
using System.Linq.Expressions;

namespace Kitbag.Builder.CQRS.Dapper.Split
{
    public abstract class QuerySplitBuilder
    {
        public static SingleQuerySplitBuilder<T, TId> QuerySingle<T, TId>(Expression<Func<T, TId>> idExpression) where T : class, new()
        {
            return new SingleQuerySplitBuilder<T, TId>(null, idExpression);
        }

        public static MultipleQuerySplitBuilder<T, TId> QueryMultiple<T, TId>(Expression<Func<T, TId>> idExpression) where T : class, new()
        {
            return new MultipleQuerySplitBuilder<T, TId>(null, idExpression);
        }

        public abstract Type[] CreateTypesArray();

        public abstract string CreateSplitOnString();

        public bool SkipParent { get; set; }
    }
}