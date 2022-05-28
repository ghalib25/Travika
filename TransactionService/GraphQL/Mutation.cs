using HotChocolate.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Model.Model;
using Newtonsoft.Json;
using System.Security.Claims;
using TransactionService.GraphQL;
using TransactionService.Kafka;

namespace TransactionService.GraphQL
{
    public class Mutation
    {
        // Add Transaction
        [Authorize(Roles = new[] { "CUSTOMER" })]
        public async Task<TransactionStatus> AddTransactionAsync(
            TransactionData input,
            ClaimsPrincipal claimsPrincipal,
            [Service] TravikaContext context,
            [Service] IOptions<KafkaSettings> settings)
        {
            using var begintransaction = context.Database.BeginTransaction();
            var userName = claimsPrincipal.Identity.Name;

            try
            {
                var customer = context.Users.Include(u => u.Transactions).Where(u => u.Username == userName).FirstOrDefault();
                var customerprofile = context.CustomerProfiles.Where(c => c.UserId == customer.Id).FirstOrDefault();

                if (customer != null)
                {
                    int orderCustomer = customer.Transactions.Where(o => o.PaymentStatus == StatusOrder.Unpaid).Count();
                    if (orderCustomer >= 1)
                    {
                        return await Task.FromResult(new TransactionStatus
                        (
                            false, "Tidak Bisa Menambah Transaksi Sebelum Melakukan Pembayaran"
                        ));
                    }

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
                    var detailhotel = new DetailsHotel
                    {
                        TransactionId = transaction.Id,
                        HotelId = itemhotel.HotelId,
                        Quantity = itemhotel.Quantity,
                        Created = DateTime.Now
                    };
                    var hotel = context.Hotels.Where(h => h.Id == detailhotel.HotelId).FirstOrDefault();
                    if(hotel.Room < detailhotel.Quantity)
                    {
                        return await Task.FromResult(new TransactionStatus
                        (
                            false, "Jumlah Kamar Tidak Mencukupi"
                        ));
                    }
                    transaction.DetailsHotels.Add(detailhotel);
                    hotel.Room -= detailhotel.Quantity;
                    TotalHotelPrice += hotel.Price * detailhotel.Quantity;

                    int TotalTicketPrice = 0;
                    var itemticket = input.DetailTicketings;
                    var detailTicket = new DetailsTicketing
                    {
                        TranscationId = transaction.Id,
                        TicketingId = itemticket.TicketingId,
                        Quantity = itemticket.Quantity,
                        Created = DateTime.Now
                    };
                    var ticket = context.Ticketings.Where(t => t.Id == detailTicket.TicketingId).FirstOrDefault();
                    if (ticket.Seat < detailTicket.Quantity)
                    {
                        return await Task.FromResult(new TransactionStatus
                        (
                            false, "Jumlah Kursi Tidak Mencukupi"
                        ));
                    }
                    ticket.Seat -= detailTicket.Quantity;
                    transaction.DetailsTicketings.Add(detailTicket);
                    TotalTicketPrice += ticket.Price * detailTicket.Quantity;

                    transaction.TotalBill = TotalHotelPrice + TotalTicketPrice;
                    context.Transactions.Add(transaction);
                    context.SaveChanges();
                    await begintransaction.CommitAsync();


                    //SendDataOrder dengan Kafka
                    SendDataOrder virtualAccount = new SendDataOrder
                    {
                        TransactionId = transaction.Id,
                        Virtualaccount = transaction.VirtualAccount, //0778 + phone
                        Bills = Convert.ToString(transaction.TotalBill), //total cost
                        PaymentStatus = transaction.PaymentStatus
                    };

                    var key = DateTime.Now.ToString();
                    var val = JsonConvert.SerializeObject(virtualAccount);
                    if (transaction.PaymentId == 2)
                    {
                        bool result = KafkaHelper.SendMessage(settings.Value, "OPO", key, val).Result;
                        if (result)
                        {
                            Console.WriteLine("Sukses Kirim ke Kafka");
                        }
                        else
                        {
                            Console.WriteLine("Gagal Kirim ke Kafka");
                        }
                    }
                    else if (transaction.PaymentId == 1)
                    {
                        bool result = KafkaHelper.SendMessage(settings.Value, "Bank", key, val).Result;
                        if (result)
                        {
                            Console.WriteLine("Sukses Kirim ke Kafka");
                        }
                        else
                        {
                            Console.WriteLine("Gagal Kirim ke Kafka");
                        }
                    }
                    else
                    {
                        throw new Exception("Payment Not Available");
                    }
                    return await Task.FromResult(new TransactionStatus
                        (
                        true, "Berhasil Membuat Order"
                        ));
                }
                else
                {
                    throw new Exception("user was not found");
                }

            }
            catch (Exception ex)
            {
                begintransaction.Rollback();
                return await Task.FromResult(new TransactionStatus
                   (
                       false, ex.Message
                   ));
            }

            return await Task.FromResult(new TransactionStatus
                (
                true, "Berhasil "
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
                    true, "Transaction Updated"
                ));
            }
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
