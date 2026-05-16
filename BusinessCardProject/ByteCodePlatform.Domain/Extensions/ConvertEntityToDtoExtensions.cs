using ByteCodePlatform.Domain.Entities;
using ByteCodePlatform.Domain.Entities.Course;
using ByteCodePlatform.Domain.Entities.Course.Content;
using ByteCodePlatform.Domain.Entities.Course.Module;
using ByteCodePlatform.Domain.Entities.Course.Theme;
using Dtos.DTO.Course.Content;
using Dtos.DTO.Course.Module;
using Dtos.DTO.Course.ProgramLanguage;
using Dtos.DTO.Course.Theme;
using Dtos.DTO.User;

namespace ByteCodePlatform.Domain.Extensions
{
    public static class ConvertEntityToDtoExtensions
    {
        #region User

        public static UserDto GetUserDto(this UserEntity e)
        {
            return new(
                e.Id,
                e.Surname,
                e.Name,
                e.NickName,
                e.Email,
                e.IsAuthor,
                e.CreatedAt,
                e.UpdatedAt ?? null,
                e.DeletedAt ?? null
            );
        }

        public static List<UserDto> GetUserDto(this List<UserEntity> en)
        {
            return en.Select(e => new UserDto(
                e.Id,
                e.Surname,
                e.Name,
                e.NickName,
                e.Email,
                e.IsAuthor,
                e.CreatedAt,
                e.UpdatedAt ?? null,
                e.DeletedAt ?? null
            )).ToList();
        }
        
        public static UserAuthorDto GetUserAuthorDto(this AuthorEntity e)
        {
            return new UserAuthorDto(e.Id, e.User.GetUserDto());
        }

        public static List<UserAuthorDto> GetUserAuthorDto(this List<AuthorEntity> en)
        {
            return en.Select(e => new UserAuthorDto(e.Id, e.User.GetUserDto())).ToList();
        }

        #endregion

        #region ProgrammingLanguage

        public static ProgrammingLanguageDto GetProgrammingLanguageDto(this ProgrammingLanguageCategoryEntity e)
        {
            return new(
                e.Id,
                e.Name,
                e.CoursesThemes.Select(ct => new LightCourseThemeDto(ct.Id, ct.Name)).ToList());
        }

        public static List<ProgrammingLanguageDto> GetProgrammingLanguageDtos(
            this IEnumerable<ProgrammingLanguageCategoryEntity> en)
        {
            return en.Select(e => new ProgrammingLanguageDto(
                e.Id,
                e.Name,
                e.CoursesThemes.Select(ct => new LightCourseThemeDto(ct.Id, ct.Name)).ToList()
            )).ToList();
        }

        #endregion

        #region CourseTheme

        public static CourseThemeDto GetCourseThemeDto(this CourseThemeEntity e)
        {
            return new(
                e.Id,
                e.Name,
                e.Description,
                e.AvatarUrl,
                e.Price,
                e.OldPrice,
                e.CreatedAt,
                e.UpdatedAt,
                e.Author.GetUserAuthorDto(),
                e.ProgrammingLanguageCategory.GetProgrammingLanguageDto()
            );
        }

        public static List<CourseThemeDto> GetCourseThemeDtos(
            this IEnumerable<CourseThemeEntity> en)
        {
            return en.Select(e => new CourseThemeDto(
                e.Id,
                e.Name,
                e.Description,
                e.AvatarUrl,
                e.Price,
                e.OldPrice,
                e.CreatedAt,
                e.UpdatedAt,
                new(e.AuthorId, e.Author.User.GetUserDto()),
                e.ProgrammingLanguageCategory.GetProgrammingLanguageDto())
            ).ToList();
        }

        #endregion

        #region CourseModule

        public static CourseModuleDto GetCourseModuleDto(this CourseModuleEntity e)
        {
            return new(
                e.Id,
                e.Name,
                e.CreatedAt,
                e.UpdatedAt,
                e.CourseTheme.GetCourseThemeDto()
            );
        }

        public static List<CourseModuleDto> GetCourseModuleDtos(
            this IEnumerable<CourseModuleEntity> en)
        {
            return en.Select(e => new CourseModuleDto(
                e.Id,
                e.Name,
                e.CreatedAt,
                e.UpdatedAt,
                e.CourseTheme.GetCourseThemeDto())
            ).ToList();
        }
        
        #endregion

        #region CourseContent

        public static CourseContentDto GetCourseContentDto(this CourseContentEntity e)
        {
            return new(
                e.Id,
                e.Name,
                e.LinkOnRutube,
                e.LinkOnVk,
                e.LinkOnYouTube,
                e.ImgUrl,
                e.CreatedAt,
                e.UpdatedAt,
                e.CourseModule.GetCourseModuleDto()
            );
        }

        public static List<CourseContentDto> GetCourseContentDtos(
            this IEnumerable<CourseContentEntity> en)
        {
            return en.Select(e => new CourseContentDto(
                e.Id,
                e.Name,
                e.LinkOnRutube,
                e.LinkOnVk,
                e.LinkOnYouTube,
                e.ImgUrl,
                e.CreatedAt,
                e.UpdatedAt,
                e.CourseModule.GetCourseModuleDto()
            )).ToList();
        }
        
        #endregion
    }
}