using HotChocolate.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Model.Model;

namespace MerchantService.GraphQL
{
    public class Mutation
    {
        //========================================TICKETING========================================

        [Authorize(Roles = new[] { "MERCHANT" })]
        public async Task<Ticketing> AddTicketingAsync(
           TicketingData input,
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
            foreach (var item in input.Details)
            {
                var detail = new DetailsTicketing
                {
                    TicketingId = ticketing.Id,
                    Quantity = item.Quantity
                };
                ticketing.DetailsTicketings.Add(detail);
            }

            var ret = context.Ticketings.Add(ticketing);
            await context.SaveChangesAsync();

            return ret.Entity;
        }

        //Update 
        [Authorize(Roles = new[] { "MERCHANT" })]
        public async Task<Ticketing> UpdateTicketingAsync(
            TicketingData input,
            [Service] TravikaContext context)
        {
            var ticketing = context.Ticketings.Where(o => o.Id == input.Id).Include(t => t.DetailsTicketings).FirstOrDefault();
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

        //========================================HOTEL========================================

        [Authorize(Roles = new[] { "MERCHANT" })]
        public async Task<Hotel> AddHotelAsync(
            HotelData input,
            [Service] TravikaContext context)
        {
            //EF
            var hotel = new Hotel
            {
                MerchantId = input.MerchantId,
                HotelName = input.HotelName,
                Address = input.Address,
                City = input.City,
                Price = input.Price,
                Status = input.Status,

            };
            foreach (var item in input.Details)
            {
                var detail = new DetailsHotel
                {
                    HotelId = hotel.Id,
                    Quantity = item.Quantity
                };
                hotel.DetailsHotels.Add(detail);
            }

            var ret = context.Hotels.Add(hotel);
            await context.SaveChangesAsync();

            return ret.Entity;
        }

        [Authorize(Roles = new[] { "MERCHANT" })]
        public async Task<Hotel> UpdateHotelAsync(
            HotelInput input,
            [Service] TravikaContext context)
        {
            var hotel = context.Hotels.Where(o => o.Id == input.Id).Include(h => h.DetailsHotels).FirstOrDefault();
            if (hotel != null)
            {
                hotel.HotelName = input.HotelName;
                hotel.Price = input.Price;
                hotel.Status = input.Status;

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
