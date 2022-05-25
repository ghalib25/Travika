namespace TransactionService.GraphQL
{
    public record DetailTicketingData
    (
        int? Id,
        int? TransactionId,
        int TicketingId,
        int Quantity,
        DateTime? Created
    );
}
