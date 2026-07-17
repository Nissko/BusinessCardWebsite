using ByteCodePlatform.Application.Application.Extensions;
using CourseService.Proto;
using Dtos.DTO.Course.ProgramLanguage;
using Dtos.DTO.Course.Theme;

namespace ByteCodePlatform.API.ProtoMappers.Course.Themes
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
                Price = dto.Price,
                OldPrice = dto.OldPrice ?? 0,
                IsFree = dto.IsFree,
                CreatedAt = dto.CreatedAt.ToTimestamp(),
                UpdatedAt = dto.UpdatedAt?.ToTimestamp() ?? null,
                Author = new()
                {
                    AuthorId = dto.Author.AuthorId.ToString(),
                    UserId = dto.Author.UserInfo.Id.ToString(),
                },
                ProgrammingLanguage = new()
                {
                    Id = dto.ProgrammingLanguage.Id.ToString()
                },
                Success = true
            };
        }

        public static List<CourseThemeInfoResponse> ToProtoCourseThemeInfoList(
            this List<CourseThemeDto> dtos)
        {
            return dtos.Select(ToProtoCourseThemeInfo).ToList();
        }
        
        public static LightCourseThemeInfo ToProtoLightCourseThemeInfo(this LightCourseThemeDto dto)
        {
            return new()
            {
                Id = dto.Id.ToString(),
                Name = dto.Name
            };
        }

        public static List<LightCourseThemeInfo> ToProtoLightCourseThemeInfoList(
            this List<LightCourseThemeDto> dtos)
        {
            return dtos.Select(ToProtoLightCourseThemeInfo).ToList();
        }

        public static CourseThemePropertiesResponse ToProtoGetFieldPropertiesCourseThemeId(this CourseThemePropertiesDto dto)
        {
            return new CourseThemePropertiesResponse
            {
                CourseThemeId = dto.CourseThemeId.ToString(),
                DisplayOrder = dto.DisplayOrder,
                IsShow = dto.IsShow,
                IsFree = dto.IsFree,
                IsDiscount = dto.IsDiscount
            };
        }
        
        public static List<CourseThemePropertiesResponse> ToProtoGetFieldPropertiesCourseThemeIdList(
            this List<CourseThemePropertiesDto> dtos)
        {
            return dtos.Select(ToProtoGetFieldPropertiesCourseThemeId).ToList();
        }
    }
}