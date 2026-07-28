using MediatR;

namespace Services.FileService.Application.Application.Command
{
    /// <summary>
    /// Запрос на получение изображения
    /// </summary>
    public class GetImageQuery(string fileId) : IRequest<(Stream Stream, string ContentType)>
    {
        public string FileId  { get; } = fileId;
    }
}