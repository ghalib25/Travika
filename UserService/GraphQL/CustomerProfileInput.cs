namespace UserService.GraphQL
{
    public record CustomerProfileInput

    (
        int? Id,
        int? UserId,
        string Name,
        string Address,
        string City,
        string Phone

    );
}
