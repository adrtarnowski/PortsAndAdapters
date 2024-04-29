using System;
using System.Linq;
using System.Text.Json.Serialization;
using Hellang.Middleware.ProblemDetails;
using Kitbag.Builder.Core.Builders;
using Kitbag.Builder.WebApi.Common;
using Kitbag.Builder.WebApi.Exceptions.Handlers;
using Kitbag.Builder.WebApi.RunningContext;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using ProblemDetailsOptions = Hellang.Middleware.ProblemDetails.ProblemDetailsOptions;

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
            
            builder.Services.AddSingleton<IHttpRunningContextProvider, HttpRunningContextProvider>();
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
        
        
        public static IKitbagBuilder AddErrorHandler(this IKitbagBuilder builder, Action<ProblemDetailsOptions>? configure = null, string sectionName = "ErrorHandler")
        {
            if (!builder.TryRegisterKitBag(sectionName))
                return builder;

            builder.Services.ConfigureOptions<ProblemDetailsOptionsCustomSetup>();
            if (configure == null)
            {
                Hellang.Middleware.ProblemDetails.ProblemDetailsExtensions.AddProblemDetails(builder.Services);
            }
            else
            {
                builder.Services.AddProblemDetails(configure);
            }

            builder.Services.AddControllersWithViews(options =>
            {
                options.Filters.Add(typeof(EmptyCommandFilter));
            });
            return builder;
        }
        
        public static IApplicationBuilder UseErrorHandler(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<CorrelationMiddleware>();
            builder.UseMiddleware<ConversationMiddleware>();
            builder.UseProblemDetails();
            return builder;
        }
    }
}