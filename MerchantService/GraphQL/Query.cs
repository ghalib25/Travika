using HotChocolate.AspNetCore.Authorization;
using Model.Model;

namespace MerchantService.GraphQL
{
    public class Query
    {
        [Authorize(Roles = new[] { "CUSTOMER" })]
        public IQueryable<Ticketing> GetTicketing([Service] TravikaContext context) =>
            context.Ticketings;
    }
}
