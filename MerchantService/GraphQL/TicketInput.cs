namespace MerchantService.GraphQL
{
    public record TicketInput
    (
        int? Id,
        int MerchantId,
        string Category,
        string Origin,
        string Destination,
        int Price
    );
}
