namespace TransactionService
{
    public record TransactionStatus
    (
        bool Success,
        string? Message
    );
}
