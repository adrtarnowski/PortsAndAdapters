using Kitbag.Builder.Core.Builders;
using Microsoft.Extensions.DependencyInjection;

namespace Kitbag.Builder.ServiceHealthCheck.Types;

public class BlobStorageServiceHealthCheck : IServiceHealthCheck
{
    private readonly string _serviceName;

    public BlobStorageServiceHealthCheck(string serviceName)
    {
        _serviceName = serviceName;
    }

    //TODO: Move to the specific BlobStorage Kitbag dll
    private class BlobStorageProperties
    {
        public string? ConnectionString { get; set; }
    }
    
    public void Register(IKitbagBuilder kitbagBuilder, IHealthChecksBuilder healthChecksBuilder)
    {
        if (kitbagBuilder is null) throw new ArgumentNullException($"{nameof(IKitbagBuilder)}");

        var blobStorageProperties = kitbagBuilder.GetSettings<BlobStorageProperties>(_serviceName);

        if (blobStorageProperties?.ConnectionString is null)
            throw new ArgumentException($"{nameof(BlobStorageProperties)} could not be loaded from configuration. Please check, if section names are matching");

        healthChecksBuilder.AddAzureBlobStorage(
            blobStorageProperties.ConnectionString,
            name: "BlobStorage",
            tags: new[] { "Azure", "BlobStorage" }
        );
    }
}