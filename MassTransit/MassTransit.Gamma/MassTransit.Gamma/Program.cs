using System;
using System.Threading.Tasks;
using MassTransit.Core;

namespace MassTransit.Gamma
{
    class Program
    {
        static void Main(string[] args)
        {
            //Receive();
            Subscribe();
        }

        #region Send/Receive

        static void Receive()
        {
            var bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var host = cfg.Host(new Uri("rabbitmq://192.168.227.131/host"), h =>
                {
                    h.Username("admin");
                    h.Password("Lunasea2019");
                });

                // it will create queue [MassTransit.Send] and bind it to exchange [MassTransit.Send] 
                cfg.ReceiveEndpoint(host, "MassTransit.Send", e =>
                {
                    e.Consumer<MessageInfoConsumerAlpha>();
                    e.Consumer<MessageInfoConsumerBeta>();
                });

            });

            bus.Start();
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
            bus.Stop();
        }

        #endregion

        #region Publish/Subscribe

        static void Subscribe()
        {
            var bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var host = cfg.Host(new Uri("rabbitmq://192.168.227.131/host"), h =>
                {
                    h.Username("admin");
                    h.Password("Lunasea2019");
                });

                // It will create queue "OtherInfo.Subscribe" and bind it to exchange "OtherInfo.Subscribe" 
                // And exchange "OtherInfo.Subscribe" will be bond by the exchange $Consumer.Classes
                cfg.ReceiveEndpoint(host, "OtherInfo.Subscribe", e =>
                {
                    e.Consumer<MessageInfoConsumerAlpha>();
                    e.Consumer<OtherInfoConsumerAlpha>();
                    e.Consumer<OtherInfoConsumerBeta>();
                });
            });

            bus.Start();
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
            bus.Stop();
        }

        #endregion

        #region Consumers

        class MessageInfoConsumerAlpha : IConsumer<MessageInfo>
        {
            public async Task Consume(ConsumeContext<MessageInfo> context)
            {
                await Console.Out.WriteLineAsync($"Message Alpha Received Message: {context.Message.Message}");
            }
        }

        class MessageInfoConsumerBeta : IConsumer<MessageInfo>
        {
            public async Task Consume(ConsumeContext<MessageInfo> context)
            {
                await Console.Out.WriteLineAsync($"Message Beta Received Message: {context.Message.Message}");
            }
        }

        class OtherInfoConsumerAlpha : IConsumer<OtherInfo>
        {
            public async Task Consume(ConsumeContext<OtherInfo> context)
            {
                await Console.Out.WriteLineAsync($"Other Alpha Received Message: {context.Message.Other}");
            }
        }

        class OtherInfoConsumerBeta : IConsumer<OtherInfo>
        {
            public async Task Consume(ConsumeContext<OtherInfo> context)
            {
                await Console.Out.WriteLineAsync($"Other Beta Received Message: {context.Message.Other}");
            }
        }

        #endregion
    }
}
