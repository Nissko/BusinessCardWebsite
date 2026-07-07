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
                UpdatedAt = dto.UpdatedAt?.ToTimestamp() ?? null,
                Module = dto.Module.ToProtoLightCourseModuleInfo(),
                Success = true
            };
        }

        public static List<CourseContentInfoResponse> ToProtoCourseContentInfoList(
            this List<CourseContentDto> dtos)
        {
            return dtos.Select(ToProtoCourseContentInfo).ToList();
        }
        
        public static CourseContentPropertiesResponse ToProtoGetFieldPropertiesCourseContentId(this CourseContentPropertiesDto dto)
        {
            return new CourseContentPropertiesResponse
            {
                CourseContentId = dto.CourseContentId.ToString(),
                DisplayOrder = dto.DisplayOrder,
                IsShow = dto.IsShow
            };
        }
        
        public static List<CourseContentPropertiesResponse> ToProtoGetFieldPropertiesCourseContentIdList(
            this List<CourseContentPropertiesDto> dtos)
        {
            return dtos.Select(ToProtoGetFieldPropertiesCourseContentId).ToList();
        }
    }
}