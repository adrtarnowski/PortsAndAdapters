using System.Collections.Generic;

namespace Kitbag.Builder.Persistence.Core.Common.Logs
{
    public interface IAuditTrailProvider
    {
        void LogChanges();
    }
}