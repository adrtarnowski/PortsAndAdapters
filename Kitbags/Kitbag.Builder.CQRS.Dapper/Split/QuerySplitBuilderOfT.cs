using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Kitbag.Builder.CQRS.Dapper.Split
{
    public abstract class QuerySplitBuilder<TBuilder> : QuerySplitBuilder
        where TBuilder : QuerySplitBuilder<TBuilder>
    {
        protected TBuilder? Parent { get; }
        protected List<TBuilder> Children;
        private readonly string? _idPropertyName;
        private readonly LambdaExpression? _idExpression;
        private readonly LambdaExpression? _includeExpression;

        protected QuerySplitBuilder(TBuilder? parent, LambdaExpression? idExpression, LambdaExpression? includeExpression, string? idPropertyName = null)
        {
            Parent = parent;
            _idExpression = idExpression;
            _includeExpression = includeExpression;
            _idPropertyName = idPropertyName;

            Children = new List<TBuilder>();
        }

        public override Type[] CreateTypesArray()
        {
            return GetRoot().CreateTypesArray_Internal();
        }

        public override string CreateSplitOnString()
        {
            return GetRoot().CreateSplitOnString_Internal();
        }

        protected abstract Type GetUnderlyingType();
        protected abstract object? EnsureValidObject(object? parent, object? newValue);

        protected TBuilder GetRoot()
        {
            return Parent != null
                ? Parent.GetRoot()
                : (TBuilder)this;
        }

        protected TChildBuilder Include<TChildBuilder>(Func<TBuilder, TChildBuilder> builderFunc)
            where TChildBuilder : TBuilder
        {
            if (Parent != null)
            {
                return Parent.Include(builderFunc);
            }
            var builder = builderFunc((TBuilder)this);
            Children.Add(builder);

            return builder;
        }

        protected TChildBuilder ThenInclude<TChildBuilder>(TChildBuilder builder)
            where TChildBuilder : TBuilder
        {
            Children.Add(builder);
            return builder;
        }

        protected static void EnsureChildren(IEnumerable<TBuilder> builders, object[] objs, object? baseObj)
        {
            object? lastParent = baseObj;

            foreach ((var child, var obj) in builders.Zip(objs[1..]))
            {
                var parentObj = IsChildOfTheRoot(child) ? baseObj : lastParent;

                object? lastObj = child.EnsureValidObject(parentObj, obj);

                lastParent = child.SkipParent ? lastParent : lastObj;
            }
        }

        protected IEnumerable<TBuilder> GetChildren()
        {
            return GetAll().ToList().Skip(1).ToList();
        }

        protected IEnumerable<TBuilder> GetAll()
        {
            yield return (TBuilder)this;
            foreach (var queryBuilder in Children)
            {
                foreach (var grandChild in queryBuilder.GetAll())
                {
                    yield return grandChild;
                }
            }
        }

        protected object? EnsureValidObjectForOneToOneRelation(object? parent, object? newValue)
        {
            if (parent == null)
                return null;
            var value = GetValue(parent) ?? newValue;
            SetValue(parent, value);
            return value;
        }

        protected void SetValue(object? parent, object? child)
        {
            if (_includeExpression != null && _includeExpression.Body is MemberExpression memberExpression)
            {
                var property = memberExpression.Member as PropertyInfo;
                if (property != null)
                {
                    property.SetValue(parent, child);
                }
            }
        }

        protected object? GetValue(object? parent)
        {
            if ( _includeExpression != null && _includeExpression.Body is MemberExpression memberExpression)
            {
                var property = memberExpression.Member as PropertyInfo;
                if (property != null)
                {
                    return property.GetValue(parent);
                }
            }

            throw new InvalidOperationException();
        }

        protected virtual string GetIdPropertyName()
        {
            if (_idExpression != null && _idExpression.Body is MemberExpression memberExpression)
            {
                var property = memberExpression.Member as PropertyInfo;
                if (property != null)
                {
                    return property.Name;
                }
            }

            throw new InvalidOperationException();
        }

        protected static bool IsChildOfTheRoot(TBuilder child)
        {
            return child.Parent?.Parent == null;
        }

        private Type[] CreateTypesArray_Internal()
        {
            var builders = GetAll();
            return builders.Select(b => b.GetUnderlyingType()).ToArray();
        }

        private string CreateSplitOnString_Internal()
        {
            var builders = GetChildren();
            return builders.Select(b => b._idPropertyName ?? b.GetIdPropertyName())
                .Aggregate("", (sum, x) => sum + $"{x},").TrimEnd(',');
        }
    }
}