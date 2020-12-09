using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Kitbag.Builder.Core.Domain;
using Kitbag.Builder.Persistence.Core.Common.Logs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TLJ.PortsAndAdapters.Infrastructure.Persistence.Configurations;

namespace TLJ.PortsAndAdapters.Infrastructure.Persistence
{
    public class DatabaseContext : DbContext
    {
        protected internal DbSet<AuditTrail>? Audits { get; set; }
        
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options) { }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(AuditConfiguration).Assembly);
            
            var allTypes = typeof(IAggregateRoot).Assembly.GetTypes();
            var entities = allTypes.Where(t => typeof(IAggregateRoot).IsAssignableFrom(t)).Where(t => t.IsClass);

            foreach (var entityType in entities)
            {
                var entity = builder.Entity(entityType);
                foreach (var property in entityType.GetProperties())
                {
                    var type = property.PropertyType;

                    if (IsId(property, out var rawValueType))
                    {
                        var converterType = 
                            typeof(IdValueConverter<,>).MakeGenericType(type, rawValueType);
                        entity.Property(type, property.Name)
                            .HasConversion(
                                (ValueConverter)Activator.CreateInstance(converterType)!);
                    }
                }
            }
        }
        
        private static bool IsId(PropertyInfo property, out Type? rawValueType)
        {
            var type = property.PropertyType;
            while (type != null)
            {
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Id))
                {
                    rawValueType = type.GetGenericArguments()[0];
                    return true;
                }
                type = type.BaseType;
            }

            rawValueType = null;
            return false;
        }
        
        private class IdValueConverter<TId, TValue> : ValueConverter<TId, TValue>
            where TId : Id 
            where TValue : IEquatable<TValue>
        {
            public IdValueConverter()
                : base(BuildConvertToProvider(), BuildConvertFromProvider())
            {
            }

            private static Expression<Func<TId, TValue>> BuildConvertToProvider()
            {
                var param = Expression.Parameter(typeof(TId));
                return Expression.Lambda<Func<TId, TValue>>(Expression.PropertyOrField(param, "Value"), param);
            }

            private static Expression<Func<TValue, TId>> BuildConvertFromProvider()
            {
                var param = Expression.Parameter(typeof(TValue));
                var ctor = typeof(TId).GetConstructor(new[] {typeof(TValue)});
                return Expression.Lambda<Func<TValue, TId>>(Expression.New(ctor, param), param);
            }
        }
    }
}