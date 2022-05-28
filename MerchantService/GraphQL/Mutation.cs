using HotChocolate.AspNetCore.Authorization;
using Model.Model;
using System.Security.Claims;

namespace MerchantService.GraphQL
{
    public class Mutation
    {
        //========================================TICKETING========================================

        [Authorize(Roles = new[] { "MERCHANT" })]
        public async Task<Ticketing> AddTicketingAsync(
           TicketingInput input,
           [Service] TravikaContext context, ClaimsPrincipal claimsPrincipal)
        {
            var userName = claimsPrincipal.Identity.Name;
            var user = context.Users.Where(o => o.Username == userName).FirstOrDefault();
            var merchant = context.MerchantProfiles.Where(m => m.UserId == user.Id).FirstOrDefault();

            // EF
            var ticketing = new Ticketing
            {
                MerchantId = merchant.Id,
                Category = input.Category,
                Origin = input.Origin,
                Destination = input.Destination,
                Departure = DateTime.Now,
                Arrival = DateTime.Now.AddHours(9),
                Price = input.Price,
                Seat = input.Seat

            };

            var ret = context.Ticketings.Add(ticketing);
            await context.SaveChangesAsync();

            return ret.Entity;
        }

        //Update 
        [Authorize(Roles = new[] { "MERCHANT" })]
        public async Task<Ticketing> UpdateTicketingAsync(
            TicketingInput input,
            [Service] TravikaContext context)
        {
            var ticketing = context.Ticketings.Where(o => o.Id == input.Id).FirstOrDefault();
            if (ticketing != null)
            {
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

        //========================================HOTEL========================================

        [Authorize(Roles = new[] { "MERCHANT" })]
        public async Task<Hotel> AddHotelAsync(
            HotelInput input,
            [Service] TravikaContext context, ClaimsPrincipal claimsPrincipal)
        {
            var userName = claimsPrincipal.Identity.Name;
            var user = context.Users.Where(o => o.Username == userName).FirstOrDefault();
            var merchant = context.MerchantProfiles.Where(m => m.UserId == user.Id).FirstOrDefault();
            //EF
            var hotel = new Hotel
            {
                MerchantId = merchant.Id,
                HotelName = input.HotelName,
                Address = input.Address,
                City = input.City,
                Price = input.Price,
                Room = input.Room
            };

            var ret = context.Hotels.Add(hotel);
            await context.SaveChangesAsync();

            return ret.Entity;
        }

        [Authorize(Roles = new[] { "MERCHANT" })]
        public async Task<Hotel> UpdateHotelAsync(
            HotelInput input,
            [Service] TravikaContext context)
        {
            var hotel = context.Hotels.Where(o => o.Id == input.Id).FirstOrDefault();
            if (hotel != null)
            {
                hotel.HotelName = input.HotelName;
                hotel.Address = input.Address;
                hotel.City = input.City;
                hotel.Price = input.Price;

                context.Hotels.Update(hotel);
                await context.SaveChangesAsync();
            }


            return await Task.FromResult(hotel);
        }

        [Authorize(Roles = new[] { "MERCHANT" })]
        public async Task<Hotel> DeleteHotelByIdAsync(
            int id,
            [Service] TravikaContext context)
        {
            var hotel = context.Hotels.Where(o => o.Id == id).FirstOrDefault();
            if (hotel != null)
            {
                context.Hotels.Remove(hotel);
                await context.SaveChangesAsync();
            }


            return await Task.FromResult(hotel);
        }
    }
}
