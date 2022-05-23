namespace TransactionService.GraphQL
{
    public record TransactionData
    (
        int? DetailHotelId,
        int? DetailTicketingId,
        int PaymentId,
        string Status

    );
}
