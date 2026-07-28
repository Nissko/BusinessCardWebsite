using MediatR;

namespace Services.FileService.Application.Application.Command
{
    /// <summary>
    /// Метод на загрузку изображения
    /// </summary>
    public class UploadImageCommand(Stream fileStream, string fileName, string contentType) : IRequest<string>
    {
        public Stream FileStream { get; } = fileStream;
        public string FileName { get; } = fileName;
        public string ContentType { get; } = contentType;
    }
}