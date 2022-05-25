using HotChocolate.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Model.Model;

namespace MerchantService.GraphQL
{
    public class Query
    {
        [Authorize(Roles = new[] { "CUSTOMER" })]
        public IQueryable<Ticketing> GetTicketing([Service] TravikaContext context) =>
            context.Ticketings.Include(t => t.DetailsTicketings);

        [Authorize(Roles = new[] { "CUSTOMER" })]
        public IQueryable<Hotel> GetHotel([Service] TravikaContext context) =>
            context.Hotels.Include(t => t.DetailsHotels);
    }
}
