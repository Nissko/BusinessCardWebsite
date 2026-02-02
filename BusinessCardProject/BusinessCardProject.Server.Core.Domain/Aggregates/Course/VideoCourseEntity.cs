using BusinessCardProject.Server.Core.Domain.Aggregates.Course.Abstracts;
using BusinessCardProject.Server.Core.Domain.Enums.UserSetting;
using NodaTime;

namespace BusinessCardProject.Server.Core.Domain.Aggregates.Course
{
    /// <summary>
    /// Видеокурсы
    /// </summary>
    public class VideoCourseEntity : CourseAbstract
    {
        public VideoCourseEntity(string linkCourseOnYoutube, string linkCourseOnRutube, string linkCourseOnVkVideo,
            Guid courseAuthorId, string courseName, string courseDescription, string courseImg,
            Instant courseDatePublished, double coursePrice, int courseDiscount, bool isShow, int displayOrder,
            Guid courseModuleId, bool isFree = false) : base(courseName, courseDescription, courseImg,
            courseDatePublished, coursePrice, courseDiscount, isShow, displayOrder, isFree)
        {
            _linkCourseOnYoutube = linkCourseOnYoutube;
            _linkCourseOnRutube = linkCourseOnRutube;
            _linkCourseOnVkVideo = linkCourseOnVkVideo;
            CourseAuthorId = courseAuthorId;
            CourseModuleId = courseModuleId;
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
        public Guid CourseModuleId;

        /// <summary>
        /// Автор, которому принадлежит курс
        /// </summary>
        public virtual CourseAuthorEntity CourseAuthor { get; private set; }
        public Guid CourseAuthorId;

        #endregion

        #region Functions
        
        public void Update(string linkOnYoutube, string linkOnRutube, string linkOnVkVideo, Guid courseAuthorId,
            Guid courseModuleId, string name, string description, string imgUrl, double price, int discount,
            bool isShow, int displayOrder, bool isFree)
        {
            _linkCourseOnYoutube = linkOnYoutube;
            _linkCourseOnRutube = linkOnRutube;
            _linkCourseOnVkVideo = linkOnVkVideo;
            CourseAuthorId = courseAuthorId;
            CourseModuleId = courseModuleId;
            UpdateCourseAbstract(name, description, imgUrl, price, discount, isShow, displayOrder, isFree);
        }
        
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