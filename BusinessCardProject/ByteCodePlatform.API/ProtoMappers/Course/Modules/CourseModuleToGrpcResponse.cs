using ByteCodePlatform.API.ProtoMappers.Themes;
using ByteCodePlatform.Application.Application.Extensions;
using CourseService.Proto;
using Dtos.DTO.Course.Module;

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
                UpdatedAt = dto.UpdatedAt?.ToTimestamp(),
                Theme = dto.Theme.ToProtoCourseThemeInfo()
            };
        }

        public static List<CourseModuleInfoResponse> ToProtoCourseModuleInfoList(
            this List<CourseModuleDto> dtos)
        {
            return dtos.Select(ToProtoCourseModuleInfo).ToList();
        }
    }
}