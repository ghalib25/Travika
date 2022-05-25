namespace TransactionService.GraphQL
{
    public record TransactionData
    (
        int PaymentId,
        string Status,
        DetailHotelData DetailHotels,
        DetailTicketingData DetailTicketings
    );
}
