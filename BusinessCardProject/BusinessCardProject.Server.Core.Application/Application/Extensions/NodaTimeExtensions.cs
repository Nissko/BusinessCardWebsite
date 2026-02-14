using System.Globalization;
using NodaTime;

namespace BusinessCardProject.Server.Core.Application.Application.Extensions
{
    public static class NodaTimeExtensions
    {
        public static LocalDateTime ToLocalDateTime(this Instant instant)
        {
            var systemZone = DateTimeZoneProviders.Tzdb.GetSystemDefault();
            return instant.InZone(systemZone).LocalDateTime;
        }
    
        public static string ToLocalString(this Instant instant)
        {
            return instant.ToLocalDateTime().ToString("dd.MM.yyyy HH:mm", CultureInfo.CurrentCulture);
        }
    
        public static DateTime ToDateTimeUtc(this Instant instant)
        {
            return instant.ToDateTimeUtc();
        }
    }
}