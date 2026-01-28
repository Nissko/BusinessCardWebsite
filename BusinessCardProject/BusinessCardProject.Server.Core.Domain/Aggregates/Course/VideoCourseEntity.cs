using BusinessCardProject.Server.Core.Domain.Aggregates.Course.Abstracts;
using BusinessCardProject.Server.Core.Domain.Enums.UserSetting;

namespace BusinessCardProject.Server.Core.Domain.Aggregates.Course
{
    /// <summary>
    /// Видеокурсы
    /// </summary>
    public class VideoCourseEntity : CourseAbstract
    {
        public VideoCourseEntity(string linkCourseOnYoutube, string linkCourseOnRutube, string linkCourseOnVkVideo,
            Guid courseAuthorId, string courseName, string courseDescription, string courseImg,
            DateTime courseDatePublished, double coursePrice, int courseDiscount, bool isShow, int displayOrder,
            Guid courseModuleId, bool isFree = false) : base(courseAuthorId, courseName, courseDescription, courseImg,
            courseDatePublished, coursePrice, courseDiscount, isShow, displayOrder, isFree)
        {
            _linkCourseOnYoutube = linkCourseOnYoutube;
            _linkCourseOnRutube = linkCourseOnRutube;
            _linkCourseOnVkVideo = linkCourseOnVkVideo;
            _courseAuthorId = courseAuthorId;
            _courseModuleId = courseModuleId;
        }

        #region Public Fields

        /// <summary>
        /// Ссылка курса на YouTube
        /// </summary>
        public string LinkOnYoutube => _linkCourseOnYoutube;

        /// <summary>
        /// Ссылка курса на RuTube
        /// </summary>
        public string LinkOnRutube => _linkCourseOnRutube;

        /// <summary>
        /// Ссылка курса на VkVideo
        /// </summary>
        public string LinkOnVkVideo => _linkCourseOnVkVideo;

        #endregion

        #region Private properties

        /// <summary>
        /// Ссылка курса на YouTube
        /// </summary>
        private string _linkCourseOnYoutube;

        /// <summary>
        /// Ссылка курса на RuTube
        /// </summary>
        private string _linkCourseOnRutube;

        /// <summary>
        /// Ссылка курса на VkVideo
        /// </summary>
        private string _linkCourseOnVkVideo;

        /// <summary>
        /// Предмет для категории подготовки
        /// </summary>
        public virtual CourseModuleEntity CourseModule { get; private set; }
        private Guid _courseModuleId;

        /// <summary>
        /// Автор, которому принадлежит курс
        /// </summary>
        public virtual CourseAuthorEntity CourseAuthor { get; private set; }
        private Guid _courseAuthorId;

        #endregion

        #region Functions

        /// <summary>
        /// Получение ссылки на курс в зависимости от выбранного
        /// </summary>
        /// <param name="platform">выбранная платформа</param>
        public string GetLinkOnSelected(SelectPlatformEnum platform)
        {
            return platform switch
            {
                SelectPlatformEnum.YouTube => _linkCourseOnYoutube,
                SelectPlatformEnum.RuTube => _linkCourseOnRutube,
                SelectPlatformEnum.VkVideo => _linkCourseOnVkVideo,
                _ => _linkCourseOnYoutube
            };
        }

        #endregion
    }
}