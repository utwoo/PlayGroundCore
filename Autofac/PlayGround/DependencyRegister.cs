using Autofac;
using PlayGround.Services;

namespace PlayGround
{
    public class DependencyRegister : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            //builder.RegisterType<Meeting>().As<IMeeting>().InstancePerLifetimeScope();
            builder.RegisterType<EnglishGreeting>().As<IGreeting>();

            //builder.RegisterType<RuleManager>().SingleInstance();
            //builder.RegisterType<InstancePerDependencyRule>().As<IRule>();
            //builder.RegisterType<SingletonRule>().As<IRule>().SingleInstance();

            builder.RegisterDecorator<ColorDecorator, IGreeting>();
            builder.RegisterDecorator<BlackWhiteDecorator, IGreeting>();
        }
    }
}
