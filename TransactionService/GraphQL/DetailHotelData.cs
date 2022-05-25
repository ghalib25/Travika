namespace TransactionService.GraphQL
{
    public record DetailHotelData
    (
        int? Id,
        int? TransactionId,
        int HotelId,
        int Quantity,
        DateTime? Created
    );
}
