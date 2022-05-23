namespace TransactionService.GraphQL
{
    public record TransactionInput
    (
        int? Id,
        int UserId,
        string VirtualAccount,
        int DetailsTicketingId,
        int DetailsHotelId,
        int PaymentId,
        int Total,
        string Status
    );
}
