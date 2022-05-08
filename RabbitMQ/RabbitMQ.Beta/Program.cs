using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;

namespace RabbitMQ.Beta
{
    class Program
    {
        public static void Main(string[] args)
        {
            var exchangeName = args.Length >= 1 ? args[0] : "DefaultExchange";
            var queueName = args.Length >= 2 ? args[1] : "DefaultQueue";
            var routeKey = args.Length >= 3 ? args[2] : string.Empty;

            // configure logger
            Log.Logger = new LoggerConfiguration()
                .WriteTo.ColoredConsole()
                .CreateLogger();

            // log start
            Log.Information("Receive Service Start...");

            // configure RabbitMQ connection factory
            var connectionFactory = new ConnectionFactory
            {
                HostName = "192.168.20.151",
                Port = 5672,
                VirtualHost = "host",
                UserName = "admin",
                Password = "admin"
            };

            // create RabbitMQ connection
            using (var connection = connectionFactory.CreateConnection())
            {
                // create RabbitMQ channel
                using (var channel = connection.CreateModel())
                {
                    // Set Prefetch
                    channel.BasicQos(0, 1, false);
                    // declare an exchange
                    channel.ExchangeDeclare(exchangeName, string.IsNullOrEmpty(routeKey) ? ExchangeType.Fanout : ExchangeType.Direct);
                    // declare a queue
                    channel.QueueDeclare(queueName, false, false, false, null);
                    // bind exchange and queue with route key
                    channel.QueueBind(queueName, exchangeName, routeKey);
                    // declare a consumer
                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Shutdown += (sender, eventArgs) => { Log.Information($"Consumer Shutdown:{eventArgs.Cause}"); };
                    consumer.Received += (model, eventArgs) =>
                    {
                        try
                        {
                            var body = eventArgs.Body;
                            Log.Information($"Received Message:{Encoding.UTF8.GetString(body)}");

                            Thread.Sleep(90 * 1000);
                            channel.BasicAck(eventArgs.DeliveryTag, false);

                            Log.Information($"Successfully Message:{Encoding.UTF8.GetString(body)}");
                        }
                        catch (Exception ex)
                        {
                            Log.Error($"Consumer Receive Error:{ex.Message}");
                        }
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