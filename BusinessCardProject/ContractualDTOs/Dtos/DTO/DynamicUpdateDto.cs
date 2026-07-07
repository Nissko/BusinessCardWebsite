namespace Dtos.DTO
{
    public record DynamicUpdateDto(
        Guid Id,
        string ClassApiName,
        string FieldApiName,
        string Value);
}