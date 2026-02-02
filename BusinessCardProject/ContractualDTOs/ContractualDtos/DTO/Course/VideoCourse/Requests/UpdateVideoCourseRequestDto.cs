namespace ContractualDtos.DTO.Course.VideoCourse.Requests;

public record UpdateVideoCourseRequestDto(
    Guid Id,
    string Name,
    string Description,
    string ImgUrl,
    string DatePublished,
    double Price,
    int Discount,
    double Rate,
    bool IsFree,
    bool IsShow,
    int DisplayOrder,
    Guid CourseAuthorId,
    Guid CourseModuleId,
    string LinkOnYoutube,
    string LinkOnRutube,
    string LinkOnVkVideo
);