using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Kitbag.Builder.CQRS.Dapper.Split
{
    public class SingleQuerySplitBuilder<T, TId> : QuerySplitBuilder<SingleQuerySplitBuilder<T, TId>>
        where T : class, new()
    {
        private readonly Expression<Func<T, TId>>? _idExpression;
        private string? _idPropertyName;

        internal SingleQuerySplitBuilder(SingleQuerySplitBuilder<T, TId>? parent, Expression<Func<T, TId>>? idExpression = null, LambdaExpression? derivedIdExpression = null, LambdaExpression? includeExpression = null, string? idPropertyName = null)
            : base(parent, idExpression ?? derivedIdExpression, includeExpression, idPropertyName)
        {
            _idExpression = idExpression;
            _idPropertyName = idPropertyName;
        }

        public SingleWithOneIncludedQuerySplit<T, TId, T, TChild, TChildId> IncludeOne<TChild, TChildId>(
            Expression<Func<T, TChild>> selectFunc,
            Expression<Func<TChild, TChildId>> idExpression)
        {
            return Include(builder => new SingleWithOneIncludedQuerySplit<T, TId, T, TChild, TChildId>(builder, selectFunc, idExpression));
        }

        public SingleWithMultipleIncludedQuerySplit<T, TId, T, TChild, TChildId> IncludeMultiple<TChild, TChildId>(
            Expression<Func<T, IEnumerable<TChild>?>> selectFunc,
            Expression<Func<TChild, TChildId>> idExpression)
        {
            return Include(builder => new SingleWithMultipleIncludedQuerySplit<T, TId, T, TChild, TChildId>(builder, selectFunc, idExpression));
        }

        public Func<object[], T> CreateSplitFunc()
        {
            return GetRoot().CreateSplitFunc_Internal();
        }

        private Func<object[], T> CreateSplitFunc_Internal()
        {
            var builders = GetChildren();
            T? baseObj = null;

            T SplitFunc(object[] objs)
            {
                baseObj = (T?)EnsureValidObject(baseObj, objs[0]);
                EnsureChildren(builders, objs, baseObj);
                return baseObj ?? throw new ArgumentNullException();
            }

            return SplitFunc;
        }

        protected override Type GetUnderlyingType()
        {
            return typeof(T);
        }

        protected override object? EnsureValidObject(object? parent, object? newValue)
        {
            return parent ?? newValue;
        }
    }
}
