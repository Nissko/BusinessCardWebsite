namespace ContractualDtos.DTO.Table
{
    /// <summary>
    /// Для пагинации
    /// </summary>
    /// <param name="Items">Записи</param>
    /// <param name="TotalCount">Всего записей</param>
    public record TableResponse<T>(
        List<T> Items,
        int TotalCount);
}