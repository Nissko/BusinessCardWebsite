namespace DTOs.DTO.User
{
    public record UserAuthorDto(
        Guid AuthorId,
        UserCoreDto UserInfo,
        string Name,
        string Surname,
        string AboutUs,
        string AvatarId);
}