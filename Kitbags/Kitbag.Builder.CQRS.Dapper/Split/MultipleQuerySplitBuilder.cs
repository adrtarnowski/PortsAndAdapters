using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Kitbag.Builder.CQRS.Dapper.Split;

public class MultipleQuerySplitBuilder<T, TId> : QuerySplitBuilder<MultipleQuerySplitBuilder<T, TId>>
    where T : class, new()
{
    private readonly Expression<Func<T, TId>>? _idExpression;

    internal MultipleQuerySplitBuilder(
        MultipleQuerySplitBuilder<T, TId>? parent,
        Expression<Func<T, TId>>? idExpression = null,
        LambdaExpression? derivedIdExpression = null,
        LambdaExpression? includeExpression = null)
        : base(parent, idExpression ?? derivedIdExpression, includeExpression)
    {
        _idExpression = idExpression;
    }

    public MultipleWithOneIncludedQuerySplitBuilder<T, TId, T, TChild, TChildId> IncludeOne<TChild, TChildId>(
        Expression<Func<T, TChild>> selectFunc,
        Expression<Func<TChild, TChildId>> idExpression)
    {
        return Include(builder => new MultipleWithOneIncludedQuerySplitBuilder<T, TId, T, TChild, TChildId>(builder, selectFunc, idExpression));
    }

    public MultipleWithMultipleIncludedQuerySplitBuilder<T, TId, T, TChild, TChildId> IncludeMultiple<TChild, TChildId>(
        Expression<Func<T, IEnumerable<TChild>?>> selectFunc,
        Expression<Func<TChild, TChildId>> idExpression)
    {
        return Include(builder => new MultipleWithMultipleIncludedQuerySplitBuilder<T, TId, T, TChild, TChildId>(builder, selectFunc, idExpression));
    }


    public Func<object[], T> CreateSplitFunc()
    {
        return GetRoot().CreateSplitFunc_Internal();
    }

    private Func<object[], T> CreateSplitFunc_Internal()
    {
        var builders = GetChildren();
        IEnumerable<T>? baseObjs = new List<T>();

        T SplitFunc(object[] objs)
        {
            var baseObj = (T?)EnsureValidObject(baseObjs, objs[0]);
            EnsureChildren(builders, objs, baseObj);
            return baseObj ?? throw new ArgumentNullException();
        }

        return SplitFunc;
    }

    protected override Type GetUnderlyingType()
    {
        return typeof(T);
    }

    private Func<object, object, bool> GetMatchingFunc()
    {
        if (_idExpression == null)
            throw new InvalidOperationException();
        var func = _idExpression.Compile();
        return (a, b) => func.Invoke((T)a)?.Equals(func.Invoke((T)b)) ?? false;
    }

    protected override object? EnsureValidObject(object? parent, object? child)
    {
        if (parent == null || child == null)
            return null;

        var predicate = GetMatchingFunc();
        var children = (parent as List<T>);
        var oldChild = children?.FirstOrDefault(c => predicate(c, child));

        if (oldChild != null)
            return oldChild;

        children?.Add((T)child);
        return child;
    }
}