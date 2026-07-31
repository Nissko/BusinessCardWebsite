namespace Services.FileService.Application.Common.Interfaces
{
    public interface IImageRepository
    {
        /// <summary>
        /// Сохранение
        /// </summary>
        Task<string> SaveImage(Stream stream, string fileName, string contentType, CancellationToken ct);

        /// <summary>
        /// Получение файла
        /// </summary>
        Task<(Stream Stream, string ContentType)> GetImage(string fileId, CancellationToken ct);
    }
}