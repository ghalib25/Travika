namespace UserService.GraphQL
{
    public record MerchantProfileInput

   (
       int? Id,
       int UserId,
       string CompanyName
   );
}
