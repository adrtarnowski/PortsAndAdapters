using Kitbag.Builder.Core.Builders;
using Microsoft.Extensions.DependencyInjection;

namespace Kitbag.Builder.ServiceHealthCheck.Types
{
    //TODO: Move to the specific BlobStorage Kitbag dll
    internal class BlobStorageProperties
    {
        public string? ConnectionString { get; set; }
    }

    public class BlobStorageHealthCheckRegistration
    {
        public static IHealthChecksBuilder RegisterHealthCheck(
            IHealthChecksBuilder healthChecksBuilder,
            IKitbagBuilder kitbagBuilder,
            string sectionName)
        {
            if (kitbagBuilder is null) throw new ArgumentNullException($"{nameof(IKitbagBuilder)}");

            var blobStorageProperties = kitbagBuilder.GetSettings<BlobStorageProperties>(sectionName);

            if (blobStorageProperties?.ConnectionString is null)
                throw new ArgumentException($"{nameof(BlobStorageProperties)} could not be loaded from configuration. Please check, if section names are matching");

            healthChecksBuilder.AddAzureBlobStorage(
                blobStorageProperties.ConnectionString,
                name: "BlobStorage",
                tags: new[] { "Azure", "BlobStorage" }
            );
            return healthChecksBuilder;
        }
    }
}