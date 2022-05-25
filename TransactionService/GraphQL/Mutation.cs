using HotChocolate.AspNetCore.Authorization;
using Model.Model;
using System.Security.Claims;
using TransactionService.GraphQL;

namespace TransactionService.GraphQL
{
    public class Mutation
    {
        // Add Transaction
        [Authorize(Roles = new[] { "CUSTOMER" })]
        public async Task<TransactionStatus> AddTransactionAsync(
            TransactionData input,
            int hotelprice, int ticketingprice,
            int stay, int seat,
            ClaimsPrincipal claimsPrincipal,
            [Service] TravikaContext context)
        {
            var userName = claimsPrincipal.Identity.Name;
            var customer = context.Users.Where(u => u.Username == userName).FirstOrDefault();
            var customerprofile = context.CustomerProfiles.Where(c => c.UserId == customer.Id).FirstOrDefault();
            var pricehotel = context.Hotels.Where(h => h.Price == hotelprice).FirstOrDefault();
            var priceticket = context.Ticketings.Where(t => t.Price == ticketingprice).FirstOrDefault();
            var totalstay = context.DetailsHotels.Where(h => h.Quantity == stay).LastOrDefault();
            var totalseat = context.DetailsTicketings.Where(t => t.Quantity == seat).LastOrDefault();
            var totalpricehotel = Convert.ToInt16(totalstay) * Convert.ToInt16(pricehotel);
            var totalpriceseat = Convert.ToInt16(totalseat) * Convert.ToInt16(priceticket);
            var checkstatus = context.Transactions.Where(c => c.UserId == customer.Id).LastOrDefault();

            if (checkstatus.PaymentStatus != "Unpaid")
            {
                var transaction = new Transaction
                {
                    UserId = customer.Id,
                    VirtualAccount = "0778" + customerprofile.Phone,
                    DetailsTicketingId = input.DetailTicketingId,
                    DetailsHotelId = input.DetailHotelId,
                    PaymentId = input.PaymentId,
                    TotalBill = totalpricehotel + totalpriceseat,
                    PaymentStatus = "Unpaid"
                };
                context.Transactions.Add(transaction);
                await context.SaveChangesAsync();

                return await Task.FromResult(new TransactionStatus
                    (
                        true, $"Order Success! Order Fee:{transaction.TotalBill.ToString()}. Waiting for Payment..."
                    ));
            }
            else
            {
                return await Task.FromResult(new TransactionStatus
                    (
                        false, "Order Failed! Please Finish Your Last Order!"
                    ));
            }
        }

        //Add detail Hotel
        [Authorize(Roles = new[] { "CUSTOMER" })]
        public async Task<TransactionStatus> AddDetailHotelAsync(
            DetailsHotel input,
            ClaimsPrincipal claimsPrincipal,
            [Service] TravikaContext context)
        {
            var userName = claimsPrincipal.Identity.Name;
            var customer = context.Users.Where(u => u.Username == userName).FirstOrDefault();
            var checkstatus = context.Transactions.Where(c => c.UserId == customer.Id).LastOrDefault();

            if (checkstatus.PaymentStatus != "Unpaid")
            {
                var detailsHotel = new DetailsHotel
                {
                    HotelId = input.HotelId,
                    Quantity = input.Quantity
                };
                var ret = context.DetailsHotels.Add(detailsHotel);
                await context.SaveChangesAsync();

                return await Task.FromResult(new TransactionStatus
                (
                    true, "Order Successfull"
                ));

            }

            return await Task.FromResult(new TransactionStatus
               (
                   false, "Finish Your Last Order!!!"
               ));
        }

        //Add detail Hotel
        [Authorize(Roles = new[] { "CUSTOMER" })]
        public async Task<TransactionStatus> AddDetailTicketingAsync(
            DetailsTicketing input,
            ClaimsPrincipal claimsPrincipal,
            [Service] TravikaContext context)
        {
            var userName = claimsPrincipal.Identity.Name;
            var customer = context.Users.Where(u => u.Username == userName).FirstOrDefault();
            var checkstatus = context.Transactions.Where(c => c.UserId == customer.Id).LastOrDefault();

            if (checkstatus.PaymentStatus != "Unpaid")
            {
                var detailsTicketing = new DetailsTicketing
                {
                    TicketingId = input.TicketingId,
                    Quantity = input.Quantity
                };
                var ret = context.DetailsTicketings.Add(detailsTicketing);
                await context.SaveChangesAsync();

                return await Task.FromResult(new TransactionStatus
                (
                    true, "Order Successfull"
                ));

            }

            return await Task.FromResult(new TransactionStatus
               (
                   false, "Finish Your Last Order!!!"
               ));
        }

        //Update Transaction
        [Authorize(Roles = new[] { "CUSTOMER" })]
        public async Task<TransactionStatus> UpdateTransactionAsync(
          int id, int payid,
          TransactionUpdate input,
           [Service] TravikaContext context)
        {
            var transaction = context.Transactions.Where(o => o.Id == id).FirstOrDefault();
            var payment = Convert.ToInt16(context.Payments.Where(p => p.Id == payid).FirstOrDefault());
            if (transaction == null)
            {
                return await Task.FromResult(new TransactionStatus(false, "Transaction Not Found"));
            }
            else
            {
                transaction.PaymentId = input.payment;

                context.Transactions.Update(transaction);
                await context.SaveChangesAsync();

                return await Task.FromResult(new TransactionStatus
                (
                    true, "Order Updated"
                ));
            }
            return await Task.FromResult(new TransactionStatus
               (
                   false, "Update Failed! Courier not Found!"
               ));
        }

        //Delete Transaction
        [Authorize(Roles = new[] { "CUSTOMER" })]
        public async Task<Transaction> DeleteTransactionByIdAsync(
            int id,
            [Service] TravikaContext context)
        {
            var transaction = context.Transactions.Where(o => o.Id == id).FirstOrDefault();
            if (transaction != null)
            {
                context.Transactions.Remove(transaction);
                await context.SaveChangesAsync();
            }
            return await Task.FromResult(transaction);
        }
    }
}
