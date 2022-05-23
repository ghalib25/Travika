using HotChocolate.AspNetCore.Authorization;
using Model.Model;
using System.Security.Claims;

namespace TransactionService.GraphQL
{
    public class Query
    {
        [Authorize(Roles = new[] { "CUSTOMER" })]
        public IQueryable<TransactionData> GetTransactions([Service] TravikaContext context) =>
            context.Transactions.Select(t => new TransactionData()
            {
                Id = t.Id,
                UserId = t.UserId,
                VirtualAccount = t.VirtualAccount,
                DetailsTicketingId = t.DetailsTicketingId,
                DetailsHotelId = t.DetailsHotelId,
                PaymentId = t.PaymentId,
                Total = t.Total,
                Status = t.Status
            });
    }
}
