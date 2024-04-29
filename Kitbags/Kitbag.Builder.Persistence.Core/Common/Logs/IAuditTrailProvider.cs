using System.Threading.Tasks;

namespace Kitbag.Builder.Persistence.Core.Common.Logs;

public interface IAuditTrailProvider
{
    Task LogChangesAsync();
}