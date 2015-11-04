namespace StarterKit.Service.Modules
{
    using System;
    using System.Configuration;
    using Autofac;
    using MassTransit;
    using MassTransit.Log4NetIntegration;
    using StarterKit.Service.Consumer;

    public class BusModule : Module
    {
        private readonly System.Reflection.Assembly[] _assembliesToScan;

        public BusModule(params System.Reflection.Assembly[] assembliesToScan)
        {
            _assembliesToScan = assembliesToScan;
        }

        protected override void Load(ContainerBuilder builder)
        {
            // Creates our bus from the factory and registers it as a singleton against two interfaces
            builder.Register(c => Bus.Factory.CreateUsingRabbitMq(sbc =>
            {
                var host = sbc.Host(new Uri(ConfigurationManager.AppSettings["RabbitMQHost"]), h =>
                {
                    // Configure your host
                    h.Username(ConfigurationManager.AppSettings["RabbitMQUsername"]);
                    h.Password(ConfigurationManager.AppSettings["RabbitMQPassword"]);
                });

                sbc.ReceiveEndpoint(host, ConfigurationManager.AppSettings["ServiceQueueName"], e =>
                {
                    // Configure your consumer(s)
                    e.Consumer<CheckOrderStatusConsumer>();
                });
            }))
                .SingleInstance()
                .As<IBusControl>()
                .As<IBus>();
        }
    }
}
