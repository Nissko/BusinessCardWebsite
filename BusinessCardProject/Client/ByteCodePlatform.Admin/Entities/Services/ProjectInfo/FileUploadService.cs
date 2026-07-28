using FilesService.Proto;
using Microsoft.AspNetCore.Components.Forms;

namespace ByteCodePlatform.Admin.Entities.Services.ProjectInfo
{
    public class FileUploadService(FilesService.Proto.FilesService.FilesServiceClient grpcClient)
    {
        private readonly FilesService.Proto.FilesService.FilesServiceClient _grpcClient =
            grpcClient ?? throw new ArgumentNullException(nameof(grpcClient));

        public async Task<string> UploadImageAsync(IBrowserFile file, CancellationToken ct = default)
        {
            var allowedTypes = new[] { "image/jpeg", "image/png", "image/webp" };
            if (!allowedTypes.Contains(file.ContentType))
            {
                throw new ArgumentException($"Неподдерживаемый формат: {file.ContentType}. Разрешены: {string.Join(", ", allowedTypes)}");
            }
        
            await using var stream = file.OpenReadStream(maxAllowedSize: 10 * 1024 * 1024, cancellationToken: ct);
        
            using var call = _grpcClient.UploadImage(cancellationToken: ct);
        
            await call.RequestStream.WriteAsync(new UploadImageRequest
            {
                Metadata = new ImageMetadata
                {
                    FileName = file.Name,
                    ContentType = file.ContentType
                }
            }, ct);
        
            var buffer = new byte[64 * 1024];
            int bytesRead;
            while ((bytesRead = await stream.ReadAsync(buffer, ct)) > 0)
            {
                var chunk = new byte[bytesRead];
                Array.Copy(buffer, chunk, bytesRead);
            
                await call.RequestStream.WriteAsync(new UploadImageRequest
                {
                    Chunk = Google.Protobuf.ByteString.CopyFrom(chunk)
                }, ct);
            }
        
            await call.RequestStream.CompleteAsync();
        
            var response = await call.ResponseAsync;
            return response.FileId;
        }
    }
}