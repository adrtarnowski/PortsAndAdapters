using Kitbag.Builder.Core.Builders;
using Kitbag.Builder.HttpClient.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Kitbag.Builder.HttpClient
{
    public static class Extension
    {
        public static IKitbagBuilder AddHttpClient(this IKitbagBuilder builder, string sectionName = "httpClient")
        {
            if (!builder.TryRegisterKitBag(sectionName))
                return builder;

            var properties = builder.GetSettings<HttpClientProperties>(sectionName);
            builder.Services.AddSingleton(properties);
            builder.Services.AddHttpClient<IHttpClient, KitBagHttpClient>("kitbag.httpclient");
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            return builder;
        }
    }
}