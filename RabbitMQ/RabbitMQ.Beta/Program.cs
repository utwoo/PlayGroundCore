using System;
using System.Text;
using System.Threading;
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
                HostName = "192.168.227.131", // CentOS Beta
                Port = 5672,
                VirtualHost = "host",
                UserName = "admin",
                Password = "Lunasea2019"
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
                    channel.QueueDeclare(queueName, false, false, true, null);
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
                    // bind consumer for queue 
                    channel.BasicConsume(queueName, false, consumer);

                    Console.ReadLine();
                }
            }
        }
    }
}
