namespace TransactionService.GraphQL
{
    public record TransactionData
    (
        int PaymentId,
        DetailHotelData DetailHotels,
        DetailTicketingData DetailTicketings
    );
}
