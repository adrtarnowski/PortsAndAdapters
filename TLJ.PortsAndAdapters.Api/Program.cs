using System.Threading.Tasks;
using Kitbag.Builder.Core;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace TLJ.PortsAndAdapters.Api
{
    class Program
    {
        public static Task Main(string[] args)
            => CreateWebHostBuilder(args).Build().RunAsync();
        
        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .ConfigureServices((webHostBuilderContext, services) => services
                    .AddKitbag(webHostBuilderContext.Configuration)
                    .Build())
                .Configure(app => app
                    .UseKitbag()
                );
        }
    }
}