namespace Services.FileService.Application.Common.Interfaces
{
    public interface IImageRepository
    {
        /// <summary>
        /// Сохранение
        /// </summary>
        Task<string> SaveAsync(Stream stream, string fileName, string contentType, CancellationToken ct);

        /// <summary>
        /// Получение файла
        /// </summary>
        Task<(Stream Stream, string ContentType)> GetAsync(string fileId, CancellationToken ct);
    }
}