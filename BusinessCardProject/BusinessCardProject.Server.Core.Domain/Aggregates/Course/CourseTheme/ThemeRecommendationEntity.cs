using BusinessCardProject.Server.Core.Domain.Commons;

namespace BusinessCardProject.Server.Core.Domain.Aggregates.Course.CourseTheme
{
    public class ThemeRecommendationEntity : Entity
    {
        public ThemeRecommendationEntity(string title, Guid courseThemeId)
        {
            _title = title;
            CourseThemeId = courseThemeId;
        }

        /// <summary>
        /// Рекомендации
        /// <example>Простое изучение C#</example>
        /// </summary>
        public string Title => _title;
        private string _title;
    
        /// <summary>
        /// Тема к которой принадлежит рекомендация 
        /// </summary>
        public virtual CourseThemeEntity CourseTheme { get; private set; }
        public Guid CourseThemeId;
    }
}