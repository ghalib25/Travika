namespace MerchantService.GraphQL
{
    public record TicketingInput
    (
        int? Id,
        int? MerchantId,
        string Category,
        string Origin,
        string Destination,
        int Price
    );
}
