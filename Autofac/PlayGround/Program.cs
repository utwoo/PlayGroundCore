using System;
using System.Reflection;
using Autofac;
using PlayGround.Services;

namespace PlayGround
{
    class Program
    {
        static void Main(string[] args)
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterAssemblyModules(Assembly.GetExecutingAssembly());
            var container = containerBuilder.Build();

            var greetings = container.Resolve<IGreeting>();

            Console.ReadLine();
        }
    }
}
