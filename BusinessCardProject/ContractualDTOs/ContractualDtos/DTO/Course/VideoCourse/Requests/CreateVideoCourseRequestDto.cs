namespace ContractualDtos.DTO.Course.VideoCourse.Requests;

/// <summary>
/// DTO для запросов
/// </summary>
public record CreateVideoCourseRequestDto(
    string Name,
    string Description,
    string ImgUrl,
    double Price,
    int Discount,
    bool IsShow,
    int DisplayOrder,
    Guid CourseAuthorId,
    Guid CourseModuleId,
    string LinkOnYoutube,
    string LinkOnRutube,
    string LinkOnVkVideo,
    bool IsFree = false);