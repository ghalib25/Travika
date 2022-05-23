using Model.Model;

namespace MerchantService.GraphQL
{
    public class Query
    {
        public IQueryable<Hotel> GetHotel([Service] TravikaContext context) =>
            context.Hotels;
    }
}
