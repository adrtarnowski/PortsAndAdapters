using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Kitbag.Builder.CQRS.Dapper.Split;

public class MultipleWithMultipleIncludedQuerySplitBuilder<T, TId, TPrevious, TChild, TChildId> : MultipleQuerySplitBuilder<T, TId>
    where T : class, new()
{
    private readonly Expression<Func<TChild, TChildId>> _idExpression;
    internal MultipleWithMultipleIncludedQuerySplitBuilder(
        MultipleQuerySplitBuilder<T, TId>? parent,
        Expression<Func<TPrevious, IEnumerable<TChild>?>> includeExpression,
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
        Children.Add(child);

        return child;
    }

    public MultipleWithMultipleIncludedQuerySplitBuilder<T, TId, TPrevious, TChild, TChildId> ThenIncludeOneForParent<TGrandChild, TGrandChildId>(
        Expression<Func<TChild, TGrandChild>> selectFunc,
        Expression<Func<TGrandChild, TGrandChildId>> idExpression)
    {
        var child = new MultipleWithOneIncludedQuerySplitBuilder<T, TId, TChild, TGrandChild, TGrandChildId>(this, selectFunc, idExpression);
        child.SkipParent = true;
        Children.Add(child);

        return this;
    }

    public MultipleWithMultipleIncludedQuerySplitBuilder<T, TId, TChild, TGrandChild, TGrandChildId> ThenIncludeMultiple<TGrandChild, TGrandChildId>(
        Expression<Func<TChild, IEnumerable<TGrandChild>?>> selectFunc,
        Expression<Func<TGrandChild, TGrandChildId>> idExpression)
    {
        var child = new MultipleWithMultipleIncludedQuerySplitBuilder<T, TId, TChild, TGrandChild, TGrandChildId>(this, selectFunc, idExpression);
        Children.Add(child);

        return child;
    }


    public MultipleWithMultipleIncludedQuerySplitBuilder<T, TId, TPrevious, TChild, TChildId> ThenIncludeMultipleForParent<TGrandChild, TGrandChildId>(
        Expression<Func<TChild, IEnumerable<TGrandChild>?>> selectFunc,
        Expression<Func<TGrandChild, TGrandChildId>> idExpression)
    {
        var child = new MultipleWithMultipleIncludedQuerySplitBuilder<T, TId, TChild, TGrandChild, TGrandChildId>(this, selectFunc, idExpression);
        child.SkipParent = true;
        Children.Add(child);

        return this;
    }

    protected override Type GetUnderlyingType()
    {
        return typeof(TChild);
    }

    protected override object? EnsureValidObject(object? parent, object? child)
    {
        if (parent == null || child == null)
            return null;

        var predicate = GetMatchingFunc();
        var children = (GetValue(parent) as IEnumerable<TChild>)?.ToList();
        children ??= new List<TChild>();
        var oldChild = children.FirstOrDefault(c => predicate(c, child));

        if (oldChild != null)
            return oldChild;

        children.Add((TChild)child);
        SetValue(parent, children);
        return child;
    }

    private Func<object, object, bool> GetMatchingFunc()
    {
        if (_idExpression == null)
            throw new InvalidOperationException();
        var func = _idExpression.Compile();
        return (a, b) => func.Invoke((TChild)a)?.Equals(func.Invoke((TChild)b)) ?? false;
    }
}