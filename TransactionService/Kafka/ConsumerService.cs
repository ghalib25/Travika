using Confluent.Kafka;
using Model.Model;
using Newtonsoft.Json;
using TransactionService.GraphQL;

namespace TransactionService.Kafka
{
    public class ConsumerService : BackgroundService
    {
        private readonly IConfiguration config;
        private readonly ConsumerConfig _consumerConfig;

        public ConsumerService(IConfiguration configuration)
        {
            config = configuration;
            _consumerConfig = new ConsumerConfig
            {
                BootstrapServers = config.GetSection("KafkaSettings").GetValue<string>("Server"),
                GroupId = "tester",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("!!! CONSUMER STARTED !!!\n");

            var task = Task.Run(() => ProcessQueue(stoppingToken), stoppingToken);

            return task;
        }

        private void ProcessQueue(CancellationToken stoppingToken)
        {
            var topic = "TRAVIKA";

            using (var consumer = new ConsumerBuilder<string, string>(_consumerConfig).Build())
            {
                Console.WriteLine("Connected");
                consumer.Subscribe(topic);
                try
                {
                    while (!stoppingToken.IsCancellationRequested)
                    {
                        var cr = consumer.Consume(stoppingToken);
                        Console.WriteLine($"Consumed record with key: {cr.Message.Key} and value: {cr.Message.Value}");

                        using (var context = new TravikaContext())
                        {
                            var val2 = JsonConvert.DeserializeObject<SendDataOrder>(cr.Message.Value);

                            var transaction = context.Transactions.Where(o => o.Id == val2.TransactionId).FirstOrDefault();
                            //var order = context.Orders.Include(o=>o.OrderDetails).Where(o=>o.Id==val2.TransactionId).FirstOrDefault();

                            transaction.PaymentStatus = val2.PaymentStatus;

                            context.Transactions.Update(transaction);
                            context.SaveChanges();
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    consumer.Close();
                    // Ctrl-C was pressed.
                }
                finally
                {
                    consumer.Close();
                }
            }

        }
    }
}
