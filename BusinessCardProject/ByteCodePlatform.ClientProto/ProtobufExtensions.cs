using Google.Protobuf.WellKnownTypes;
using NodaTime;
using NodaTime.Extensions;

namespace ByteCodePlatform.ClientProto
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
    }
}