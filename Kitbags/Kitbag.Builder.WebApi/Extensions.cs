using System;
using System.Linq;
using System.Text.Json.Serialization;
using Kitbag.Builder.Core.Builders;
using Kitbag.Builder.WebApi.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Kitbag.Builder.WebApi
{
public static class Extensions
    {
        public static string AllowedSpecificOrigins = "_allowedSpecificOrigins";
        public static IKitbagBuilder AddWebApi(this IKitbagBuilder builder, string sectionName = "WebApi")
        {
            if (!builder.TryRegisterKitBag(sectionName)) 
                return builder;
            
            var assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault();
            var webApiOptions = builder.GetSettings<WebApiOptions>(sectionName);
            var corsAllowedOrigins = webApiOptions.CorsAllowedOrigins?.ToArray() ?? new string[0];
            
            builder.Services
                .AddMvcCore(option => option.EnableEndpointRouting = false)
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.DateParseHandling = DateParseHandling.DateTimeOffset;
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                })
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                })
                .AddApplicationPart(assembly)
                .AddControllersAsServices()
                .AddAuthorization()
                .AddApiExplorer()
                .AddCors(options => options.AddPolicy(
                    AllowedSpecificOrigins,
                    b => b.WithOrigins(corsAllowedOrigins)
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                ));

            return builder;
        }

        public static IKitbagBuilder AddApiContext(this IKitbagBuilder builder, string sectionName = "WebApiContext")
        {
            if (!builder.TryRegisterKitBag(sectionName)) 
                return builder;
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            return builder;
        }

        public static IApplicationBuilder UseControllers(this IApplicationBuilder builder)
        {
            builder.UseCors(AllowedSpecificOrigins);
            builder.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            return builder;
        }
    }
}