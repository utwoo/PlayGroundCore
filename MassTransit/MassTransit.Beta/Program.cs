using System;
using System.Threading.Tasks;
using MassTransit.Core;

namespace MassTransit.Beta
{
    class Program
    {
        static void Main(string[] args)
        {
            Receive();
            //Subscribe();
            //Response();
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

            // AOP
            bus.ConnectReceiveObserver(new ReceiveObserver());

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

                // It will create queue "MessageInfo.Subscribe" and bind it to exchange "MessageInfo.Subscribe" 
                // And exchange "MessageInfo.Subscribe" will be bond by the exchange $Consumer.Classes
                cfg.ReceiveEndpoint(host, "MessageInfo.Subscribe", e =>
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

        #region Request/Response

        static void Response()
        {
            var bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var host = cfg.Host(new Uri("rabbitmq://192.168.227.131/host"), h =>
                {
                    h.Username("admin");
                    h.Password("Lunasea2019");
                });

                // it will create queue [MassTransit.Request] and bind it to exchange [MassTransit.Request] 
                cfg.ReceiveEndpoint(host, "MassTransit.Request", e =>
                {
                    e.Consumer<MessageInfoRequestConsumer>();
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

        class MessageInfoRequestConsumer : IConsumer<IMessageRequest>
        {
            public async Task Consume(ConsumeContext<IMessageRequest> context)
            {
                await Console.Out.WriteLineAsync($"Message Alpha Received Message: {context.Message.Message}");
                await context.RespondAsync<IMessageResponse>(new { Result = $"Success For {context.Message.Message}" });
                Console.WriteLine("Response Successfully.");
            }
        }

        #endregion
    }

    class ReceiveEndpointObserver : IReceiveEndpointObserver
    {
        public async Task Completed(ReceiveEndpointCompleted completed)
        {
            await Console.Out.WriteLineAsync($"---------Receive Completed---------");
        }

        public async Task Faulted(ReceiveEndpointFaulted faulted)
        {
            await Console.Out.WriteLineAsync($"---------Receive Faulted---------");
        }

        public async Task Ready(ReceiveEndpointReady ready)
        {
            await Console.Out.WriteLineAsync($"---------Receive Ready---------");
        }
    }

    class ReceiveObserver : IReceiveObserver
    {
        public async Task ConsumeFault<T>(ConsumeContext<T> context, TimeSpan duration, string consumerType, Exception exception) where T : class
        {
            await Console.Out.WriteLineAsync($"---------ConsumeFault---------");
        }

        public async Task PostConsume<T>(ConsumeContext<T> context, TimeSpan duration, string consumerType) where T : class
        {
            await Console.Out.WriteLineAsync($"---------PostConsume---------");
        }

        public async Task PostReceive(ReceiveContext context)
        {
            await Console.Out.WriteLineAsync($"---------PostReceive---------");
        }

        public async Task PreReceive(ReceiveContext context)
        {
            await Console.Out.WriteLineAsync($"---------PreReceive---------");
        }

        public async Task ReceiveFault(ReceiveContext context, Exception exception)
        {
            await Console.Out.WriteLineAsync($"---------ReceiveFault---------");
        }
    }
}
