using CourseService.Proto;
using DTOs.DTO.Course.ProgramLanguage;

namespace ByteCodePlatform.API.ProtoMappers.Course.ProgramLanguages
{
    public static class CourseProgramLanguageToGrpcResponse
    {
        public static ProgrammingLanguageInfoResponse ToProtoProgramLanguageInfo(this ProgrammingLanguageDto dto)
        {
            return new()
            {
                Id = dto.Id.ToString(),
                Name = dto.Name,
                CourseThemes = { dto.CourseThemes.ToProtoLightCourseThemeProgramLanguagesList() }
            };
        }

        public static List<ProgrammingLanguageInfoResponse> ToProtoProgramLanguageInfoList(
            this List<ProgrammingLanguageDto> dtos)
        {
            return dtos.Select(ToProtoProgramLanguageInfo).ToList();
        }
    
        public static LightCourseThemeInfo ToProtoLightCourseThemeProgramLanguages(this LightCourseThemeDto dto)
        {
            return new()
            {
                Id = dto.Id.ToString(),
                Name = dto.Name,
            };
        }

        public static List<LightCourseThemeInfo> ToProtoLightCourseThemeProgramLanguagesList(
            this List<LightCourseThemeDto> dtos)
        {
            return dtos.Select(ToProtoLightCourseThemeProgramLanguages).ToList();
        }
    }
}