using Kitbag.Builder.Core.Types;
using Microsoft.AspNetCore.Builder;

namespace TLJ.PortsAndAdapters.Infrastructure
{
    public static class Extensions
    {
        public static IKitbagBuilder AddInfrastructure(this IKitbagBuilder builder)
        {
            return builder;
        }

        public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder builder)
        {
            return builder;
        }
    }
}