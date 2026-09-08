using ByteCodePlatform.API.ProtoMappers.Course.Themes;
using ByteCodePlatform.Application.Application.Extensions;
using CourseService.Proto;
using DTOs.DTO.Course.Content;
using DTOs.DTO.Course.Module;

namespace ByteCodePlatform.API.ProtoMappers.Course.Modules
{
    public static class CourseModuleToGrpcResponse
    {
        public static CourseModuleInfoResponse ToProtoCourseModuleInfo(this CourseModuleDto dto)
        {
            return new()
            {
                Id = dto.Id.ToString(),
                Name = dto.Name,
                CreatedAt = dto.CreatedAt.ToTimestamp(),
                UpdatedAt = dto.UpdatedAt?.ToTimestamp() ?? null,
                Theme = dto.Theme.ToProtoLightCourseThemeInfo(),
                Success = true
            };
        }

        public static List<CourseModuleInfoResponse> ToProtoCourseModuleInfoList(
            this List<CourseModuleDto> dtos)
        {
            return dtos.Select(ToProtoCourseModuleInfo).ToList();
        }
        
        public static LightCourseModuleInfo ToProtoLightCourseModuleInfo(this LightCourseModuleDto dto)
        {
            return new()
            {
                Id = dto.Id.ToString(),
            };
        }

        public static List<LightCourseModuleInfo> ToProtoLightCourseModuleInfoList(
            this List<LightCourseModuleDto> dtos)
        {
            return dtos.Select(ToProtoLightCourseModuleInfo).ToList();
        }
        
        public static CourseModulePropertiesResponse ToProtoGetFieldPropertiesCourseModuleId(this CourseModulePropertiesDto dto)
        {
            return new CourseModulePropertiesResponse
            {
                CourseModuleId = dto.CourseModuleId.ToString(),
                DisplayOrder = dto.DisplayOrder,
                IsShow = dto.IsShow
            };
        }
        
        public static List<CourseModulePropertiesResponse> ToProtoGetFieldPropertiesCourseModuleIdList(
            this List<CourseModulePropertiesDto> dtos)
        {
            return dtos.Select(ToProtoGetFieldPropertiesCourseModuleId).ToList();
        }
    }
}