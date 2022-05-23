using HotChocolate.AspNetCore.Authorization;
using Model.Model;

namespace MerchantService.GraphQL
{
    public class Mutation
    {
        //========================================Ticketing========================================
        [Authorize(Roles = new[] { "MERCHANT" })]
        public async Task<Ticketing> AddTicketingAsync(
           TicketInput input,
            [Service] TravikaContext context)
        {

            // EF
            var ticketing = new Ticketing
            {
                MerchantId = input.MerchantId,
                Category = input.Category,
                Origin = input.Origin,
                Destination = input.Destination,
                Departure = DateTime.Now,
                Arrival = DateTime.Now.AddHours(9),
                Price = input.Price,
                
            };

            var ret = context.Ticketings.Add(ticketing);
            await context.SaveChangesAsync();

            return ret.Entity;
        }

        //Update 
        [Authorize(Roles = new[] { "MERCHANT" })]
        public async Task<Ticketing> UpdateTicketingAsync(
            TicketInput input,
            [Service] TravikaContext context)
        {
            var ticketing = context.Ticketings.Where(o => o.Id == input.Id).FirstOrDefault();
            if (ticketing != null)
            {
                ticketing.MerchantId = input.MerchantId;
                ticketing.Category = input.Category;
                ticketing.Origin = input.Origin;
                ticketing.Destination = input.Destination;
                ticketing.Price = input.Price;
                ticketing.Departure = DateTime.Now;
                ticketing.Arrival = DateTime.Now.AddHours(9).AddMinutes(30);

                context.Ticketings.Update(ticketing);
                await context.SaveChangesAsync();
            }
            return await Task.FromResult(ticketing);
        }

        //Delete
        [Authorize(Roles = new[] { "MERCHANT" })]
        public async Task<Ticketing> DeleteTicketingByIdAsync(
            int id,
            [Service] TravikaContext context)
        {
            var ticketing = context.Ticketings.Where(o => o.Id == id).FirstOrDefault();
            if (ticketing != null)
            {
                context.Ticketings.Remove(ticketing);
                await context.SaveChangesAsync();
            }
            return await Task.FromResult(ticketing);
        }
    }
}
