using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Kitbag.Builder.CQRS.Dapper.Split;

public class MultipleWithOneIncludedQuerySplitBuilder<T, TId, TPrevious, TChild, TChildId> : MultipleQuerySplitBuilder<T, TId>
    where T : class, new()
{
    private readonly Expression<Func<TChild, TChildId>> _idExpression;

    internal MultipleWithOneIncludedQuerySplitBuilder(
        MultipleQuerySplitBuilder<T, TId>? parent,
        Expression<Func<TPrevious, TChild>> includeExpression,
        Expression<Func<TChild, TChildId>> idExpression)
        : base(parent, null, idExpression, includeExpression)
    {
        _idExpression = idExpression;
    }


    public MultipleWithOneIncludedQuerySplitBuilder<T, TId, TChild, TGrandChild, TGrandChildId> ThenIncludeOne<TGrandChild, TGrandChildId>(
        Expression<Func<TChild, TGrandChild>> selectFunc,
        Expression<Func<TGrandChild, TGrandChildId>> idExpression)
    {
        var child = new MultipleWithOneIncludedQuerySplitBuilder<T, TId, TChild, TGrandChild, TGrandChildId>(this, selectFunc, idExpression);
        return ThenInclude(child);
    }

    public MultipleWithOneIncludedQuerySplitBuilder<T, TId, TPrevious, TChild, TChildId> ThenIncludeOneForParent<TGrandChild, TGrandChildId>(
        Expression<Func<TChild, TGrandChild>> selectFunc,
        Expression<Func<TGrandChild, TGrandChildId>> idExpression)
    {
        var child = new MultipleWithOneIncludedQuerySplitBuilder<T, TId, TChild, TGrandChild, TGrandChildId>(this, selectFunc, idExpression);
        child.SkipParent = true;
        ThenInclude(child);
        return this;
    }

    public MultipleWithMultipleIncludedQuerySplitBuilder<T, TId, TChild, TGrandChild, TGrandChildId> ThenIncludeMultiple<TGrandChild, TGrandChildId>(
        Expression<Func<TChild, IEnumerable<TGrandChild>?>> selectFunc,
        Expression<Func<TGrandChild, TGrandChildId>> idExpression)
    {
        var child = new MultipleWithMultipleIncludedQuerySplitBuilder<T, TId, TChild, TGrandChild, TGrandChildId>(this, selectFunc, idExpression);
        return ThenInclude(child);
    }

    public MultipleWithOneIncludedQuerySplitBuilder<T, TId, TPrevious, TChild, TChildId> ThenIncludeMultipleForParent<TGrandChild, TGrandChildId>(
        Expression<Func<TChild, IEnumerable<TGrandChild>?>> selectFunc,
        Expression<Func<TGrandChild, TGrandChildId>> idExpression)
    {
        var child = new MultipleWithMultipleIncludedQuerySplitBuilder<T, TId, TChild, TGrandChild, TGrandChildId>(this, selectFunc, idExpression);
        child.SkipParent = true;
        ThenInclude(child);
        return this;
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