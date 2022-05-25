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
            var checkstatus = context.Transactions.Where(c => c.UserId == customer.Id).LastOrDefault();

            if (checkstatus.PaymentStatus != "Unpaid")
            {
                var transaction = new Transaction
                {
                    UserId = customer.Id,
                    VirtualAccount = "0778" + customerprofile.Phone,
                    PaymentId = input.PaymentId,
                    TotalBill = 0,
                    PaymentStatus = "Unpaid"
                };

                int TotalHotelPrice = 0;
                var itemhotel = input.DetailHotels;
                {
                    var detailhotel = new DetailsHotel
                    {
                        TransactionId = transaction.Id,
                        HotelId = itemhotel.HotelId,
                        Quantity = itemhotel.Quantity
                    };
                    var hotel = context.Hotels.Where(h => h.Id == detailhotel.HotelId).FirstOrDefault();
                    transaction.DetailsHotels.Add(detailhotel);
                    TotalHotelPrice += hotel.Price * detailhotel.Quantity;
                };

                int TotalTicketPrice = 0;
                var itemticket = input.DetailTicketings;
                {
                    var detailTicket = new DetailsTicketing
                    {
                        TranscationId = transaction.Id,
                        TicketingId = itemticket.TicketingId,
                        Quantity = itemticket.Quantity
                    };
                    var ticket = context.Ticketings.Where(t => t.Id == detailTicket.TicketingId).FirstOrDefault();
                    transaction.DetailsTicketings.Add(detailTicket);
                    TotalTicketPrice += ticket.Price * detailTicket.Quantity;
                };

                transaction.TotalBill = TotalHotelPrice + TotalTicketPrice;
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
        public async Task<TransactionStatus> DeleteTransactionByIdAsync(
            int id,
            [Service] TravikaContext context)
        {
            var transaction = context.Transactions.Where(o => o.Id == id).FirstOrDefault();
            if (transaction == null)
            {
                return await Task.FromResult(new TransactionStatus(false, "Transaction not Found!"));
            }
            if (transaction.PaymentStatus == "UNPAID")
            {
                context.Transactions.Remove(transaction);
                await context.SaveChangesAsync();
                return await Task.FromResult(new TransactionStatus
            (
                true, "Transaction Deleted"
            ));
            }

            return await Task.FromResult(new TransactionStatus
            (
                true, "Transaction Cancelled"
            ));
        }
    }
}
