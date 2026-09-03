namespace BusinessCardProject.Client.Entities.Services.Mains
{
    /// <summary>
    /// Единый сервис по получению ссылок на сервисы
    /// </summary>
    public class GetLinksService : IDisposable
    {
        private readonly string _imageServiceUrl;

        public GetLinksService(IConfiguration configuration)
        {
            _imageServiceUrl = configuration["Services:ImageServiceBaseUrl"] 
                               ?? throw new ArgumentNullException(nameof(configuration));
        }

        /// <summary>
        /// Получение изображения из FileService
        /// </summary>
        public string GetUrlFromImageService(string baseUrl) => $"{_imageServiceUrl}{baseUrl}";

        public void Dispose() { }
    }
}