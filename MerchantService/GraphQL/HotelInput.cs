namespace MerchantService.GraphQL
{
    public record HotelInput
   (
       int? Id,
       int? MerchantId,
       string HotelName,
       string Address,
       string City,
       int Price
   );
}
