using HotChocolate.AspNetCore.Authorization;
using Model.Model;

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
    }
}
