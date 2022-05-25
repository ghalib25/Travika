using HotChocolate.AspNetCore.Authorization;
using Model.Model;
using System.Security.Claims;

namespace TransactionService.GraphQL
{
    public class Query
    {
        [Authorize(Roles = new[] { "CUSTOMER" })]
        public IQueryable<Transaction> GetTransactions([Service] TravikaContext context) =>
            context.Transactions.Select(t => new Transaction()
            {
                Id = t.Id,
                UserId = t.UserId,
                VirtualAccount = t.VirtualAccount,
                PaymentId = t.PaymentId,
                TotalBill = t.TotalBill,
                PaymentStatus = t.PaymentStatus
            });

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
