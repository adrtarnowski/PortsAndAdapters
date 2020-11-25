using System;

namespace Kitbag.Builder.Core.Common
{
    public static class SystemTime
    {
        private static DateTime? _customDateTime;
        public static DateTime Now() => _customDateTime ?? DateTime.UtcNow;
        public static string NowAsString() => Now().ToString("u").Replace("Z", "");
        public static DateTimeOffset OffsetNow() => _customDateTime ?? DateTimeOffset.UtcNow;
        public static void Set(DateTime customDateTime) => _customDateTime = customDateTime;
        public static void Reset() => _customDateTime = null;
    }
}