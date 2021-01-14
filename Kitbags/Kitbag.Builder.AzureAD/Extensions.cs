using System;
using System.Threading.Tasks;
using Kitbag.Builder.Core.Builders;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;

namespace Kitbag.Builder.AzureAD
{
    public static class Extensions
    {
        public static IKitbagBuilder AddAzureAD(
            this IKitbagBuilder builder,
            string sectionName = "AzureAD",
            string? subsectionName = null)
        {
            if (!builder.TryRegisterKitBag(sectionName))
                return builder;
            var section = string.IsNullOrEmpty(subsectionName) ? sectionName : $"{sectionName}:{subsectionName}";
            builder.Services.AddProtectedWebApi(options =>
            {
                builder.GetSettings(section, options);
                options.Events = new JwtBearerEvents();
                options.Events.OnAuthenticationFailed = (c) =>
                {
                    if (c.Exception is UnauthorizedAccessException)
                        c.Fail(c.Exception);
                    return Task.CompletedTask;
                };
                options.Events.OnChallenge = (c) =>
                {
                    if (string.IsNullOrEmpty(c.ErrorDescription)) 
                        c.ErrorDescription = c.AuthenticateFailure?.Message;
                    return Task.CompletedTask;
                };
            }, options =>
            {
                builder.GetSettings(section, options);
            });

            return builder;
        }
    }
}