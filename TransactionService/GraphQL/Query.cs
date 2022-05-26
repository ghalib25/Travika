using HotChocolate.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Model.Model;
using System.Security.Claims;

namespace TransactionService.GraphQL
{
    public class Query
    {
        [Authorize(Roles = new[] { "CUSTOMER" })]
        public IQueryable<Transaction> GetTransactions(ClaimsPrincipal claimsPrincipal, [Service] TravikaContext context)
        {
            var username = claimsPrincipal.Identity.Name;
            var user = context.Users.Where(o => o.Username == username).FirstOrDefault();
            if (user != null)
            {
                var transactions = context.Transactions
                    .Where(o => o.UserId == user.Id)
                    .Include(t => t.DetailsTicketings)
                    .Include(t => t.DetailsHotels);
                return transactions.AsQueryable();
            }
            return new List<Transaction>().AsQueryable();
        }

        [Authorize(Roles = new[] { "MANAGER" })]
        public IQueryable<Transaction> GetAllTransactions([Service] TravikaContext context)=>
            context.Transactions.Include(t => t.DetailsTicketings).Include(t => t.DetailsHotels);
       


        //Check Transaction
        [Authorize(Roles = new[] { "CUSTOMER" })]
        public async Task<TransactionStatus> GetPaymentStatusAsync(
            int id,
            ClaimsPrincipal claimsPrincipal,
            [Service] TravikaContext context)
        {
            var userName = claimsPrincipal.Identity.Name;
            var customer = context.Users.Where(o => o.Username == userName).FirstOrDefault();
            var profilecustomer = context.CustomerProfiles.Where(c => c.UserId == customer.Id).FirstOrDefault();
            var transaction = context.Transactions.Where(t => t.UserId == customer.Id).FirstOrDefault();
            if (transaction.PaymentStatus == "Paid")
            {
                return await Task.FromResult(new TransactionStatus
            (
                true, "Pembayaran Berhasil"
            ));
            }
            return await Task.FromResult(new TransactionStatus
            (
                false, "Silahkan Melakukan Pembayaran"
            ));
        }
    }
}
