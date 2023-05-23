using System;
using System.Threading.Tasks;
using Kitbag.Builder.Core.Builders;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Web;

namespace Kitbag.Builder.AzureAD
{
    public static class Extensions
    {
        public static IKitbagBuilder AddAzureAD(
            this IKitbagBuilder builder,
            string sectionName = "AzureAD",
            string? subsectionName = null,
            IConfiguration? configuration = null)
        {
            if (!builder.TryRegisterKitBag(sectionName))
                return builder;
            var section = string.IsNullOrEmpty(subsectionName) ? sectionName : $"{sectionName}:{subsectionName}";
            builder.Services.AddMicrosoftIdentityWebAppAuthentication(configuration, section);

            return builder;
        }
    }
}