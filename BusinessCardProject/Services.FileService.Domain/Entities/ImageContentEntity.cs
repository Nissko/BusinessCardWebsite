using NodaTime;
using Services.FileService.Domain.Common;

namespace Services.FileService.Domain.Entities
{
    public class ImageContentEntity : Entity
    {
        /// <summary>
        /// Название файла
        /// </summary>
        public string FileName { get; private set; }
        /// <summary>
        /// Тип
        /// </summary>
        public string ContentType { get; private set; }
        /// <summary>
        /// Длина
        /// </summary>
        public long Length { get; private set; }
        /// <summary>
        /// Дата загрузки
        /// </summary>
        public Instant UploadedAt { get; private set; }
    
        private static readonly HashSet<string> AllowedTypes = new(StringComparer.OrdinalIgnoreCase)
        {
            "image/jpeg", "image/png", "image/webp"
        };

        public ImageContentEntity(string fileName, string contentType)
        {
            if (!AllowedTypes.Contains(contentType))
            {
                throw new ArgumentException("Unsupported image format");
            }

            FileName = fileName;
            ContentType = contentType;
            UploadedAt = SystemClock.Instance.GetCurrentInstant();
        }
    }
}