using BusinessCardProject.Server.Core.Domain.Aggregates.Course.Abstracts;

namespace BusinessCardProject.Server.Core.Domain.Aggregates.Course;

/// <summary>
/// Текстовые курсы
/// </summary>
public class TextCourseAbstract(
    Guid courseAuthorId,
    string courseName,
    string courseDescription,
    string courseImg,
    DateTime courseDatePublished,
    double coursePrice,
    int courseDiscount,
    bool isShow,
    int displayOrder,
    bool isFree = false) : CourseAbstract(courseAuthorId, courseName, courseDescription, courseImg, courseDatePublished,
    coursePrice, courseDiscount, isShow, displayOrder, isFree)
{

    #region private properties

    

    #endregion
}