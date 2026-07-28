using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using Services.FileService.Application.Common.Interfaces;

namespace Services.FileService.Infrastructure.Repositories
{
    public class ImageRepository(IMongoDatabase database) : IImageRepository
    {
        private readonly IGridFSBucket _bucket = new GridFSBucket(database, new GridFSBucketOptions
        {
            BucketName = "images",
            ChunkSizeBytes = 1048576, // ограничение в 1 MB
        });

        public async Task<string> SaveAsync(Stream stream, string fileName, string contentType, CancellationToken ct)
        {
            var options = new GridFSUploadOptions
            {
                Metadata = new MongoDB.Bson.BsonDocument
                {
                    { "contentType", contentType },
                    { "fileName", fileName }
                }
            };

            var id = await _bucket.UploadFromStreamAsync(fileName, stream, options, ct);
            return id.ToString();
        }

        public async Task<(Stream Stream, string ContentType)> GetAsync(string fileId, CancellationToken ct)
        {
            var objectId = MongoDB.Bson.ObjectId.Parse(fileId);
            var filter = Builders<GridFSFileInfo>.Filter.Eq(x => x.Id, objectId);
            var cursor = await _bucket.FindAsync(filter, cancellationToken: ct);
        
            var fileInfo = await cursor.FirstOrDefaultAsync(ct);
        
            if (fileInfo == null)
            {
                throw new FileNotFoundException("Изображение не найдено");
            }

            var contentType = fileInfo.Metadata?["contentType"]?.AsString ?? "application/octet-stream";
        
            var stream = await _bucket.OpenDownloadStreamAsync(objectId, cancellationToken: ct);
            return (stream, contentType);
        }
    }
}