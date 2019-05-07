using System;
using System.Text;
using RabbitMQ.Client;
using Serilog;

namespace RabbitMQ.Alpha
{
    class Program
    {
        static void Main(string[] args)
        {
            // console commands 
            string command = string.Empty;
            string[] commands;

            // confirgure logger
            Log.Logger = new LoggerConfiguration()
                .WriteTo.ColoredConsole()
                .CreateLogger();

            // log start
            Log.Information("Send Service Start...  Please press Message [Exchange] [RouteKey]");

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
                    // if command is 'exit' then exit
                    while ((command = Console.ReadLine()) != "exit")
                    {
                        string
                            exchangeName = "DefaultExchange",
                            routingKey = "",
                            message = string.Empty;

                        // split commands with space
                        commands = command.Split(' ');

                        switch (commands.Length)
                        {
                            case 1:
                                message = commands[0];
                                break;
                            case 2:
                                message = commands[0];
                                exchangeName = commands[1];
                                break;
                            case 3:
                                message = commands[0];
                                exchangeName = commands[1];
                                routingKey = commands[2];
                                break;
                            default:
                                Log.Information("Input Parameters Invalid.");
                                break;
                        }

                        // declare exchange
                        if (!string.IsNullOrWhiteSpace(exchangeName))
                            channel.ExchangeDeclare(exchangeName, ExchangeType.Fanout);

                        byte[] body = Encoding.UTF8.GetBytes(message);

                        // publish message witn exchange name, routeKey and message
                        channel.BasicPublish(exchangeName, routingKey, null, body);

                        Log.Information($"Send Message Successfully: {message}");
                        Log.Information("Send Service Start...  Please press Message [Exchange] [RouteKey]");
                    }
                }
            }
        }
    }
}
