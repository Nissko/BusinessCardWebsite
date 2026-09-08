namespace DTOs.DTO.Pagination
{
    /// <summary>
    /// Для пагинации
    /// </summary>
    /// <param name="Items">Записи</param>
    /// <param name="TotalCount">Всего записей</param>
    public record PaginationDto<T>(
        List<T> Items,
        int TotalCount);
}