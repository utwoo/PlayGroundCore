using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;

namespace RabbitMQ.Beta
{
    class Program
    {
        public static void Main(string[] args)
        {
            int random = new Random().Next(1, 1000);

            string exchangeName = args.Length >= 1 ? args[0] : "DefaultExchange";
            string queueName = args.Length >= 2 ? args[1] : "DefaultQueue";
            string routeKey = args.Length >= 3 ? args[2] : string.Empty;

            // confirgure logger
            Log.Logger = new LoggerConfiguration()
                .WriteTo.ColoredConsole()
                .CreateLogger();

            // log start
            Log.Information("Receive Service Start...");

            // configure RabbitMQ connenction factory
            var connectionFactory = new ConnectionFactory
            {
                //HostName = "47.96.126.127", // Aliyun
                HostName = "192.168.227.134", // CentOS Beta
                Port = 5672,
                //VirtualHost = "host",
                UserName = "guest",
                Password = "guest"
            };

            // create RabbitMQ connection
            using (var connection = connectionFactory.CreateConnection())
            {
                // create RabbitMQ channel
                using (var channel = connection.CreateModel())
                {
                    // declare an exchange
                    channel.ExchangeDeclare(exchangeName, string.IsNullOrEmpty(routeKey) ? ExchangeType.Fanout : ExchangeType.Direct);
                    // declare a queue
                    channel.QueueDeclare(queueName, false, false, false, null);
                    // bind exchange and queue with route key
                    channel.QueueBind(queueName, exchangeName, routeKey);
                    // declare a consumer
                    var consumer = new EventingBasicConsumer(channel);

                    consumer.Received += (model, eventArgs) =>
                    {
                        byte[] body = eventArgs.Body;
                        Log.Information($"Received Message:{Encoding.UTF8.GetString(body)}");
                        channel.BasicAck(eventArgs.DeliveryTag, true);
                    };

                    // Use Rx.Net to Throttle
                    //var received =
                    //    Observable.FromEventPattern<EventHandler<BasicDeliverEventArgs>, BasicDeliverEventArgs>(
                    //        h => consumer.Received += h,
                    //        h => consumer.Received -= h)
                    //        .Select(p => p.EventArgs);

                    //received
                    //    .Throttle(TimeSpan.FromSeconds(5))
                    //    .Subscribe(message =>
                    //    {
                    //        Log.Information($"Received Message:{Encoding.UTF8.GetString(message.Body)}");
                    //        channel.BasicAck(message.DeliveryTag, true);
                    //    });

                    // bind consumer for queue 
                    channel.BasicConsume(queueName, false, consumer);

                    Console.ReadLine();
                }
            }
        }
    }
}
