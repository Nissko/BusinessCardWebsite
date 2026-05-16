using Google.Protobuf.WellKnownTypes;
using NodaTime;
using NodaTime.Extensions;

namespace ByteCodePlatform.Application.Application.Extensions
{
    public static class ProtobufExtensions
    {
        public static Instant ToInstant(this Timestamp timestamp) => 
            timestamp?.ToDateTimeOffset().ToInstant() ?? Instant.FromUnixTimeSeconds(0);
    
        public static Timestamp ToTimestamp(this Instant instant) => 
            instant.ToDateTimeUtc().ToTimestamp();
        
        public static Guid ToGuid(this string id) =>
            Guid.TryParse(id, out var guid) ? guid : Guid.Empty;
        
        public static Guid? ToGuidOrNull(this string id) =>
            Guid.TryParse(id, out var guid) ? guid : null;

        public static decimal? ToDecimalOrNull(this Double value) =>
            decimal.TryParse(value.ToString(), out var result) ? result : null;
        
        public static decimal ToDecimal(this Double value) =>
            decimal.TryParse(value.ToString(), out var result) ? result : 0;

        public static double ToDoubleOrNull(this Decimal? value) =>
            double.TryParse(value.ToString(), out var result) ? result : 0;

        public static double ToDouble(this Decimal value) =>
            double.TryParse(value.ToString(), out var result) ? result : 0;
    }
}