using CourseService.Proto;
using Dtos.DTO.Course.ProgramLanguage;

namespace ByteCodePlatform.API.ProtoMappers.ProgramLanguages
{
    public static class CourseProgramLanguageToGrpcResponse
    {
        public static ProgrammingLanguageInfoResponse ToProtoProgramLanguageInfo(this ProgrammingLanguageDto dto)
        {
            return new()
            {
                Id = dto.Id.ToString(),
                Name = dto.Name,
                CourseThemes = { dto.CourseThemes.ToProtoLightCourseThemeInfoInfoList() }
            };
        }

        public static List<ProgrammingLanguageInfoResponse> ToProtoProgramLanguageInfoList(
            this List<ProgrammingLanguageDto> dtos)
        {
            return dtos.Select(ToProtoProgramLanguageInfo).ToList();
        }
    
        public static LightCourseThemeInfo ToProtoLightCourseThemeInfoInfo(this LightCourseThemeDto dto)
        {
            return new()
            {
                Id = dto.Id.ToString(),
                Name = dto.Name,
            };
        }

        public static List<LightCourseThemeInfo> ToProtoLightCourseThemeInfoInfoList(
            this List<LightCourseThemeDto> dtos)
        {
            return dtos.Select(ToProtoLightCourseThemeInfoInfo).ToList();
        }
    }
}