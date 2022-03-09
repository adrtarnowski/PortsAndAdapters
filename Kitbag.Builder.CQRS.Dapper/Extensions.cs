using Kitbag.Builder.Core.Builders;
using Kitbag.Builder.CQRS.Core.Queries;
using Kitbag.Builder.CQRS.Dapper.Queries.Handlers;
using Kitbag.Builder.CQRS.Dapper.Queries.Providers;
using Kitbag.Builder.CQRS.Dapper.Sql;
using Microsoft.Extensions.DependencyInjection;

namespace Kitbag.Builder.CQRS.Dapper
{
    public static class Extensions
    {
        public static IKitbagBuilder AddDapperForQueries(this IKitbagBuilder builder, IDapperInitializer? dapperInitializer = null, string sectionName = "Dapper")
        {
            if (!builder.TryRegisterKitBag(sectionName))
                return builder;
            
            dapperInitializer?.Init();
            builder.Services.AddSingleton<ISqlConnectionFactory, SqlConnectionFactory>();
            builder.Services.Decorate(typeof(IQueryHandler<,>), typeof(FilteredQueryHandler<,>));
            builder.Services.Decorate(typeof(IQueryHandler<,>), typeof(PagedQueryHandler<,>));
            builder.Services.Decorate(typeof(IQueryHandler<,>), typeof(SortedQueryHandler<,>));
            builder.Services.AddTransient<IDynamicParametersExtractor, DynamicParametersExtractor>();
            
            return builder;
        }
    }
}