using ByteCodePlatform.API.ProtoMappers.ProgramLanguages;
using ByteCodePlatform.Application.Application.Extensions;
using CourseService.Proto;
using Dtos.DTO.Course.Theme;

namespace ByteCodePlatform.API.ProtoMappers.Themes
{
    public static class CourseThemeToGrpcResponse
    {
        public static CourseThemeInfoResponse ToProtoCourseThemeInfo(this CourseThemeDto dto)
        {
            return new()
            {
                Id = dto.Id.ToString(),
                Name = dto.Name,
                Description = dto.Description,
                AvatarUrl = dto.AvatarUrl,
                Price = dto.Price.ToDouble(),
                OldPrice = dto.OldPrice.ToDoubleOrNull(),
                CreatedAt = dto.CreatedAt.ToTimestamp(),
                UpdatedAt = dto.UpdatedAt?.ToTimestamp(),
                Author = new()
                {
                    AuthorId = dto.Author.AuthorId.ToString(),
                    UserId = dto.Author.UserInfo.Id.ToString(),
                    Surname = dto.Author.UserInfo.Surname,
                    Name = dto.Author.UserInfo.Name,
                    NickName = dto.Author.UserInfo.NickName
                },
                ProgrammingLanguage = new()
                {
                    Id = dto.ProgrammingLanguage.Id.ToString(),
                    Name = dto.ProgrammingLanguage.Name,
                    CourseThemes =
                    {
                        dto.ProgrammingLanguage.CourseThemes
                            .ToProtoLightCourseThemeInfoInfoList()
                    }
                }
            };
        }

        public static List<CourseThemeInfoResponse> ToProtoCourseThemeInfoList(
            this List<CourseThemeDto> dtos)
        {
            return dtos.Select(ToProtoCourseThemeInfo).ToList();
        }
    }
}