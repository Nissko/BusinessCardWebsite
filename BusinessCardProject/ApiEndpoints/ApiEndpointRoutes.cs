namespace ApiEndpoints;

public static class ApiEndpointRoutes
{
    /// <summary>
    /// Url для backend(-a)
    /// </summary>
    public static string BaseUrl => "https://localhost:7284/";

    /// <summary>
    /// Url для frontend(-a)
    /// </summary>
    public static string BaseFrontUrl => "https://localhost:7209";

    /// <summary>
    /// Ссылки на темы курсов
    /// </summary>
    public static class CourseThemes
    {
        public static readonly string GetAll = "api/CourseTheme/get";
        public const string GetById = "api/CourseTheme/find/{id}";
        public const string Create = "api/CourseTheme/create";
        public const string Update = "api/CourseTheme/update";
        public const string Delete = "api/CourseTheme/delete/{id}";
    }
}