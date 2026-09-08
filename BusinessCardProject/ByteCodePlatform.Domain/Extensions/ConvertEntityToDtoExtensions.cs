using ByteCodePlatform.Domain.Entities;
using ByteCodePlatform.Domain.Entities.Course;
using ByteCodePlatform.Domain.Entities.Course.Content;
using ByteCodePlatform.Domain.Entities.Course.Module;
using ByteCodePlatform.Domain.Entities.Course.Theme;
using ByteCodePlatform.Domain.Enums;
using DTOs.DTO.Course.Content;
using DTOs.DTO.Course.Module;
using DTOs.DTO.Course.ProgramLanguage;
using DTOs.DTO.Course.Theme;
using DTOs.DTO.User;

namespace ByteCodePlatform.Domain.Extensions
{
    public static class ConvertEntityToDtoExtensions
    {
        #region User

        public static UserCoreDto GetUserCoreDto(this UserEntity e)
        {
            return new(
                e.UserId
            );
        }

        public static List<UserCoreDto> GetUserCoreDto(this List<UserEntity> en)
        {
            return en.Select(e => new UserCoreDto(
                e.UserId
            )).ToList();
        }

        public static UserAuthorDto GetUserAuthorDto(this AuthorEntity e)
        {
            return new UserAuthorDto(e.Id, e.User.GetUserCoreDto(), e.Name, e.Surname, e.AboutUs, e.AvatarId);
        }

        public static List<UserAuthorDto> GetUserAuthorDto(this List<AuthorEntity> en)
        {
            return en.Select(e => new UserAuthorDto(e.Id, e.User.GetUserCoreDto(), e.Name, e.Surname, e.AboutUs, e.AvatarId)).ToList();
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

        public static LightProgrammingLanguageDto GetLightProgrammingLanguageDto(
            this ProgrammingLanguageCategoryEntity e)
        {
            return new(e.Id);
        }

        public static List<LightProgrammingLanguageDto> GetLightProgrammingLanguageDtos(
            this IEnumerable<ProgrammingLanguageCategoryEntity> en)
        {
            return en.Select(e => new LightProgrammingLanguageDto(e.Id)).ToList();
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
                e.GetIsFreeProperty(),
                e.GetCountLessons(),
                e.Price,
                e.OldPrice,
                e.CreatedAt,
                e.UpdatedAt ?? null,
                e.Author.GetUserAuthorDto(),
                e.ProgrammingLanguageCategory.GetLightProgrammingLanguageDto()
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
                e.GetIsFreeProperty(),
                e.GetCountLessons(),
                e.Price,
                e.OldPrice,
                e.CreatedAt,
                e.UpdatedAt ?? null,
                new(e.AuthorId, e.Author.User.GetUserCoreDto(),
                    e.Author.Name, e.Author.Surname,
                    e.Author.AboutUs, e.Author.AvatarId),
                e.ProgrammingLanguageCategory.GetLightProgrammingLanguageDto())
            ).ToList();
        }
        
        public static LightCourseThemeDto GetLightCourseThemeDto(this CourseThemeEntity e)
        {
            return new(e.Id, e.Name);
        }

        public static List<LightCourseThemeDto> GetLightCourseThemeDtos(
            this IEnumerable<CourseThemeEntity> en)
        {
            return en.Select(e => new LightCourseThemeDto(e.Id, e.Name)).ToList();
        }
        
        public static CourseThemePropertiesDto GetCourseThemePropertiesDto(this List<CourseThemeFieldPropertyEntity> e)
        {
            return new CourseThemePropertiesDto(
                e.Select(x => x.CourseThemeId).FirstOrDefault(),
                int.TryParse(e.FirstOrDefault(x => x.FieldPropertyTypeId == FieldPropertyTypesEnum.DisplayOrder)?.Value,
                    out var displayOrder)
                    ? displayOrder
                    : 1,
                bool.TryParse(e.FirstOrDefault(x => x.FieldPropertyTypeId == FieldPropertyTypesEnum.IsShow)?.Value,
                    out var isShow) && isShow,
                !bool.TryParse(e.FirstOrDefault(x => x.FieldPropertyTypeId == FieldPropertyTypesEnum.IsFree)?.Value,
                    out var isFree) || isFree,
                bool.TryParse(e.FirstOrDefault(x => x.FieldPropertyTypeId == FieldPropertyTypesEnum.IsDiscount)?.Value,
                    out var isDiscount) && isDiscount);
        }

        #endregion

        #region CourseModule

        public static CourseModuleDto GetCourseModuleDto(this CourseModuleEntity e)
        {
            return new(
                e.Id,
                e.Name,
                e.CreatedAt,
                e.UpdatedAt ?? null,
                e.CourseTheme.GetLightCourseThemeDto()
            );
        }

        public static List<CourseModuleDto> GetCourseModuleDtos(
            this IEnumerable<CourseModuleEntity> en)
        {
            return en.Select(e => new CourseModuleDto(
                e.Id,
                e.Name,
                e.CreatedAt,
                e.UpdatedAt ?? null,
                e.CourseTheme.GetLightCourseThemeDto())
            ).ToList();
        }
        
        public static LightCourseModuleDto GetLightCourseModuleDto(this CourseModuleEntity e)
        {
            return new(e.Id);
        }

        public static List<LightCourseModuleDto> GetLightCourseModuleDtos(
            this IEnumerable<CourseModuleEntity> en)
        {
            return en.Select(e => new LightCourseModuleDto(e.Id)).ToList();
        }
        
        public static CourseModulePropertiesDto GetCourseModulePropertiesDto(this List<CourseModuleFieldPropertyEntity> e)
        {
            return new CourseModulePropertiesDto(
                e.Select(x => x.CourseModuleId).FirstOrDefault(),
                int.TryParse(e.FirstOrDefault(x => x.FieldPropertyTypeId == FieldPropertyTypesEnum.DisplayOrder)?.Value,
                    out var displayOrder)
                    ? displayOrder
                    : 1,
                bool.TryParse(e.FirstOrDefault(x => x.FieldPropertyTypeId == FieldPropertyTypesEnum.IsShow)?.Value,
                    out var isShow) && isShow);
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
                e.UpdatedAt ?? null,
                e.CourseModule.GetLightCourseModuleDto()
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
                e.UpdatedAt ?? null,
                e.CourseModule.GetLightCourseModuleDto()
            )).ToList();
        }
        
        public static CourseContentPropertiesDto GetCourseContentPropertiesDto(this List<CourseContentFieldPropertyEntity> e)
        {
            return new CourseContentPropertiesDto(
                e.Select(x => x.CourseContentId).FirstOrDefault(),
                int.TryParse(e.FirstOrDefault(x => x.FieldPropertyTypeId == FieldPropertyTypesEnum.DisplayOrder)?.Value,
                    out var displayOrder)
                    ? displayOrder
                    : 1,
                bool.TryParse(e.FirstOrDefault(x => x.FieldPropertyTypeId == FieldPropertyTypesEnum.IsShow)?.Value,
                    out var isShow) && isShow);
        }

        #endregion
    }
}