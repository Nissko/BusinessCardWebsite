namespace ContractualDtos.DTO.DynamicUpdateEntities;

public record DynamicClassDto(
    Guid Id,
    string ClassApiName,
    string FieldApiName,
    string Value);