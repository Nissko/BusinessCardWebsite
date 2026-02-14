namespace ContractualDtos.DTO.Course.VideoCourse.Dtos
{
    /// <summary>
    /// DTO для возврата объекта
    /// </summary>
    public record VideoCourseDtos(
        Guid Id,
        string Name,
        string Description,
        string ImgUrl,
        string DatePublished,
        double Price,
        int Discount,
        double Rate,
        bool IsFree,
        Guid CourseAuthorId,
        Guid CourseModuleId,
        string LinkOnYoutube,
        string LinkOnRutube,
        string LinkOnVkVideo);
}