using System;
using System.Threading.Tasks;
using MassTransit.Core;

namespace MassTransit.Alpha
{
    class Program
    {
        static void Main(string[] args)
        {
            Send().Wait();
            //Publish();
            //Request().Wait();
        }

        static IBusControl ConfigureBus()
        {
            return Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host(new Uri("rabbitmq://192.168.227.131/host"), host =>
                {
                    host.Username("admin");
                    host.Password("Lunasea2019");
                });
            });
        }

        #region Send/Receive

        // Send will send messsage to exchange names $SendEndpoint (MassTransit.Send)
        static async Task Send()
        {
            var uri = new Uri("rabbitmq://192.168.227.131/host/MassTransit.Send");
            var bus = ConfigureBus();

            string message = string.Empty;
            while ((message = Console.ReadLine()) != "exit")
            {
                var endpoint = await bus.GetSendEndpoint(uri);

                await endpoint.Send(new MessageInfo { Id = Guid.NewGuid(), Message = message });
                Console.WriteLine($"Send Message: {message}");
            }
        }

        #endregion

        #region Publish/Subscribe

        // Publish will send messsage to exchange names $Class.Name  
        static void Publish()
        {
            var bus = ConfigureBus();

            string message = string.Empty;
            while ((message = Console.ReadLine()) != "exit")
            {
                // It will publish data to exchange [MassTransit.Core:MessageInfo]
                bus.Publish(new MessageInfo
                {
                    Id = Guid.NewGuid(),
                    Message = message
                });

                // It will publish data to exchange [MassTransit.Core:OtherInfo]
                bus.Publish(new OtherInfo
                {
                    Id = Guid.NewGuid(),
                    Other = message
                });

                Console.WriteLine($"Send Message: {message}");
            }
        }

        #endregion

        #region Request/Response

        static async Task Request()
        {
            var uri = new Uri("rabbitmq://192.168.227.131/host/MassTransit.Request");
            var bus = ConfigureBus();

            var client = bus.CreateRequestClient<IMessageRequest, IMessageResponse>(uri, TimeSpan.FromSeconds(10));

            bus.Start(); // request/response mode must start bus first.

            string message = string.Empty;
            while ((message = Console.ReadLine()) != "exit")
            {
                Console.WriteLine($"Request Message: {message}");
                var response = await client.Request<IMessageRequest, IMessageResponse>(new { Id = Guid.NewGuid(), Message = message });
                Console.WriteLine($"Response Message: {response.Result}");
            }

            bus.Stop();

            await Task.CompletedTask;
        }

        #endregion
    }

    class PublishObserver : IPublishObserver
    {
        public async Task PostPublish<T>(PublishContext<T> context) where T : class
        {
            await Console.Out.WriteLineAsync($"---------PostPublish---------");
        }

        public async Task PrePublish<T>(PublishContext<T> context) where T : class
        {
            await Console.Out.WriteLineAsync($"---------PrePublish---------");
        }

        public async Task PublishFault<T>(PublishContext<T> context, Exception exception) where T : class
        {
            await Console.Out.WriteLineAsync($"---------PublishFault---------");
        }
    }

    class SendObserver : ISendObserver
    {
        public async Task PostSend<T>(SendContext<T> context) where T : class
        {
            await Console.Out.WriteLineAsync($"---------PostSend---------");
        }

        public async Task PreSend<T>(SendContext<T> context) where T : class
        {
            await Console.Out.WriteLineAsync($"---------PreSend---------");
        }

        public async Task SendFault<T>(SendContext<T> context, Exception exception) where T : class
        {
            await Console.Out.WriteLineAsync($"---------SendFault---------");
        }
    }
}
