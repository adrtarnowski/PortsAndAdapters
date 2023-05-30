using Kitbag.Builder.Core.Builders;
using Kitbag.Builder.RunningContext.Common;
using Microsoft.Extensions.DependencyInjection;

namespace Kitbag.Builder.RunningContext
{
    public static class Extensions
    {
        public static IKitbagBuilder AddRunningContext(this IKitbagBuilder builder, Func<IServiceProvider, IRunningContextProvider?> contextFactory, string sectionName = "RunningContext")
        {
            if (!builder.TryRegisterKitBag(sectionName))
                return builder;

            builder.Services.AddScoped(contextFactory);
            return builder;
        }
    }
}