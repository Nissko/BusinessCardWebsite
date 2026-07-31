using FilesService.Proto;
using Grpc.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Services.FileService.Application.Application.Command;

namespace Services.FileService.Presentation.Services
{
    public class FileGrpcService(IMediator mediator, ILogger<FileGrpcService> logger) : FilesService.Proto.FilesService.FilesServiceBase
    {
        private readonly ILogger<FileGrpcService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        
        [AllowAnonymous]
        public override async Task<UploadImageResponse> UploadImage(
            IAsyncStreamReader<UploadImageRequest> requestStream,
            ServerCallContext context)
        {
            try
            {
                var fileName = string.Empty;
                var contentType = string.Empty;

                using var ms = new MemoryStream();

                await foreach (var request in requestStream.ReadAllAsync(context.CancellationToken))
                {
                    switch (request.PayloadCase)
                    {
                        case UploadImageRequest.PayloadOneofCase.Metadata:
                            fileName = request.Metadata.FileName;
                            contentType = request.Metadata.ContentType;
                            break;
                        case UploadImageRequest.PayloadOneofCase.Chunk:
                            ms.Write(request.Chunk.ToByteArray());
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }

                ms.Position = 0;
                var command = new UploadImageCommand(ms, fileName, contentType);
                var fileId = await mediator.Send(command, context.CancellationToken);

                return new UploadImageResponse { FileId = fileId };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка в методе {MethodName}", nameof(UploadImage));
                throw new RpcException(new Status(StatusCode.Internal, "Произошла внутренняя ошибка"));
            }
        }

        [AllowAnonymous]
        public override async Task GetImage(
            GetImageRequest request,
            IServerStreamWriter<GetImageResponse> responseStream,
            ServerCallContext context)
        {
            try
            {
                var query = new GetImageQuery(request.FileId);
                var (stream, contentType) = await mediator.Send(query, context.CancellationToken);

                await context.WriteResponseHeadersAsync(new Metadata
                {
                    { "content-type", contentType }
                });

                var buffer = new byte[81920];
                int bytesRead;
                while ((bytesRead = await stream.ReadAsync(buffer, context.CancellationToken)) > 0)
                {
                    await responseStream.WriteAsync(new GetImageResponse
                    {
                        Chunk = Google.Protobuf.ByteString.CopyFrom(buffer, 0, bytesRead)
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка в методе {MethodName}", nameof(GetImage));
                throw new RpcException(new Status(StatusCode.Internal, "Произошла внутренняя ошибка"));
            }
        }
    }
}