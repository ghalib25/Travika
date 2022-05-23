using Model.Model;

namespace MerchantService.GraphQL
{
    public class Mutation
    {
        public async Task<Hotel> AddHotelAsync(
            HotelInput input,
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

            var ret = context.Hotels.Add(hotel);
            await context.SaveChangesAsync();

            return ret.Entity;
        }

        public async Task<Hotel> UpdateHotelAsync(
            HotelInput input,
            [Service] TravikaContext context)
        {
            var hotel = context.Hotels.Where(o => o.Id == input.Id).FirstOrDefault();
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
