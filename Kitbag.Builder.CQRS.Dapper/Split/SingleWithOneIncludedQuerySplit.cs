using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Kitbag.Builder.CQRS.Dapper.Split
{
    public class SingleWithOneIncludedQuerySplit<T, TId, TPrevious, TChild, TChildId> : SingleQuerySplitBuilder<T, TId>
        where T : class, new()
    {
        private readonly Expression<Func<TChild, TChildId>> _idExpression;

        internal SingleWithOneIncludedQuerySplit(
            SingleQuerySplitBuilder<T, TId>? parent,
            Expression<Func<TPrevious, TChild>> includeExpression,
            Expression<Func<TChild, TChildId>> idExpression,
            string? idPropertyName = null)
            : base(parent, null, idExpression, includeExpression, idPropertyName)
        {
            _idExpression = idExpression;
        }

        public SingleWithOneIncludedQuerySplit<T, TId, TChild, TGrandChild, TGrandChildId> ThenIncludeOne<TGrandChild, TGrandChildId>(
            Expression<Func<TChild, TGrandChild>> selectFunc,
            Expression<Func<TGrandChild, TGrandChildId>> idExpression)
        {
            var child = new SingleWithOneIncludedQuerySplit<T, TId, TChild, TGrandChild, TGrandChildId>(this, selectFunc, idExpression);
            return ThenInclude(child);
        }

        public SingleWithOneIncludedQuerySplit<T, TId, TPrevious, TChild, TChildId> ThenIncludeOneForParent<TGrandChild, TGrandChildId>(
            Expression<Func<TChild, TGrandChild>> selectFunc,
            Expression<Func<TGrandChild, TGrandChildId>> idExpression)
        {
            var child = new SingleWithOneIncludedQuerySplit<T, TId, TChild, TGrandChild, TGrandChildId>(this, selectFunc, idExpression);
            child.SkipParent = true;
            ThenInclude(child);

            return this;
        }

        public SingleWithMultipleIncludedQuerySplit<T, TId, TChild, TGrandChild, TGrandChildId> ThenIncludeMultiple<TGrandChild, TGrandChildId>(
            Expression<Func<TChild, IEnumerable<TGrandChild>?>> selectFunc,
            Expression<Func<TGrandChild, TGrandChildId>> idExpression)
        {
            var child = new SingleWithMultipleIncludedQuerySplit<T, TId, TChild, TGrandChild, TGrandChildId>(this, selectFunc, idExpression);
            return ThenInclude(child);
        }

        public SingleWithOneIncludedQuerySplit<T, TId, TPrevious, TChild, TChildId> ThenIncludeMultipleForParent<TGrandChild, TGrandChildId>(
            Expression<Func<TChild, IEnumerable<TGrandChild>?>> selectFunc,
            Expression<Func<TGrandChild, TGrandChildId>> idExpression)
        {
            var child = new SingleWithMultipleIncludedQuerySplit<T, TId, TChild, TGrandChild, TGrandChildId>(this, selectFunc, idExpression);

            child.SkipParent = true;
            ThenInclude(child);

            return this!;
        }

        protected override Type GetUnderlyingType()
        {
            return typeof(TChild);
        }

        protected override object? EnsureValidObject(object? parent, object? newValue)
        {
            return EnsureValidObjectForOneToOneRelation(parent, newValue);
        }
    }
}
