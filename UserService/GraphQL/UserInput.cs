namespace UserService.GraphQL
{
    public record UserInput
    (
        int? Id,
        string Email,
        string Username,
        string Password
    );
}
