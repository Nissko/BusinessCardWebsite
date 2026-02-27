using Microsoft.Extensions.Configuration;

namespace ApiEndpoints
{
    public abstract class ApiEndpointRoutes
    {
        private static IConfiguration _configuration;

        public static void Init(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }
    
        /// <summary>
        /// Url для backend(-a)
        /// </summary>
        public static string BaseUrl => _configuration["BaseUrl"] ??  "https://localhost:7284/";

        /// <summary>
        /// Url для frontend(-a)
        /// </summary>
        public static string BaseFrontUrl => _configuration["BaseFrontUrl"] ??  "https://localhost:7209/";
    
        /// <summary>
        /// Url для Production backend(-a)
        /// </summary>
        public static string BaseUrlProd => "http://localhost:5000/api/";

        /// <summary>
        /// Базовые ссылки ядра
        /// </summary>
        public static class CoreBase  
        {
            public const string DynamicUpdate = "api/Core/dynamic-update";
        }
        
        /// <summary>
        /// Ссылки на темы курсов
        /// </summary>
        public static class CourseThemes
        {
            public static readonly string AdminGetAll = "api/CourseTheme/admin-get-all";
            public static readonly string FilteredGetAll = "api/CourseTheme/get";
            public const string GetById = "api/CourseTheme/find/";
            public const string GetByLanguageId = "api/CourseTheme/find-by-language/";
            public const string Create = "api/CourseTheme/create";
            public const string Update = "api/CourseTheme/update";
            public const string Delete = "api/CourseTheme/delete/";
        }
    
        public static class Users  
        {
            public const string GetAll = "api/UserProfile";
            public const string GetById = "api/UserProfile/find/";
            public const string Create = "api/UserProfile/create";
            public const string Update = "api/UserProfile/update";
            public const string Delete = "api/UserProfile/delete/";
        }
        
        public static class ProgrammingLanguage  
        {
            public const string GetAll = "api/ProgrammingLanguageCourse/get";
            public const string GetById = "api/ProgrammingLanguageCourse/find/";
            public const string Create = "api/ProgrammingLanguageCourse/create";
            public const string Update = "api/ProgrammingLanguageCourse/update";
            public const string Delete = "api/ProgrammingLanguageCourse/delete/";
        }
    }
}