using ByteCodePlatform.API.ProtoMappers.Course.Modules;
using ByteCodePlatform.Application.Application.Extensions;
using CourseService.Proto;
using Dtos.DTO.Course.Content;

namespace ByteCodePlatform.API.ProtoMappers.Course.Contents
{
    public static class CourseContentToGrpcResponse
    {
        public static CourseContentInfoResponse ToProtoCourseContentInfo(this CourseContentDto dto)
        {
            return new()
            {
                Id = dto.Id.ToString(),
                Name = dto.Name,
                LinkRutube = dto.LinkRutube,
                LinkVk = dto.LinkVk,
                LinkYoutube = dto.LinkYoutube,
                ImgUrl = dto.ImgUrl,
                CreatedAt = dto.CreatedAt.ToTimestamp(),
                UpdatedAt = dto.UpdatedAt?.ToTimestamp(),
                Module = dto.Module.ToProtoCourseModuleInfo()
            };
        }

        public static List<CourseContentInfoResponse> ToProtoCourseContentInfoList(
            this List<CourseContentDto> dtos)
        {
            return dtos.Select(ToProtoCourseContentInfo).ToList();
        }
    }
}