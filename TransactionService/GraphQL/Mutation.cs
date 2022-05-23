using HotChocolate.AspNetCore.Authorization;
using Model.Model;
using System.Security.Claims;

namespace TransactionService.GraphQL
{
    public class Mutation
    {
        [Authorize(Roles = new[] { "CUSTOMER" })]
        public async Task<Transaction> AddTransactionAsync(
           TransactionInput input,
            [Service] TravikaContext context)
        {

            // EF
            var transaction = new Transaction
            {
                UserId = input.UserId,
                VirtualAccount = input.VirtualAccount,
                DetailsTicketingId = input.DetailsTicketingId,
                DetailsHotelId = input.DetailsHotelId,
                PaymentId = input.PaymentId,
                Total = input.Total,
                Status = input.Status
            };

            var ret = context.Transactions.Add(transaction);
            await context.SaveChangesAsync();

            return ret.Entity;
        }

        //Update
        [Authorize(Roles = new[] { "CUSTOMER" })]
        public async Task<Transaction> UpdateTransactionAsync(
          TransactionData input,
           [Service] TravikaContext context)
        {
            var transaction = context.Transactions.Where(o => o.Id == input.Id).FirstOrDefault();
            if (transaction != null)
            {
                transaction.UserId = input.UserId;
                transaction.VirtualAccount = input.VirtualAccount;
                transaction.DetailsTicketingId = input.DetailsTicketingId;
                transaction.DetailsHotelId = input.DetailsHotelId;
                transaction.PaymentId = input.PaymentId;
                transaction.Total = input.Total;
                transaction.Status = input.Status;

                context.Transactions.Update(transaction);
                await context.SaveChangesAsync();
            }

            return await Task.FromResult(transaction);
        }

        //Delete
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
