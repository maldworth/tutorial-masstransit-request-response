namespace StarterKit.Service.Modules
{
    using System;
    using System.Configuration;
    using Autofac;
    using MassTransit;
    using MassTransit.Log4NetIntegration;
    using MassTransit.AzureServiceBusTransport;
    using StarterKit.Service.Consumer;
    using Microsoft.ServiceBus;

    public class AzureServiceBusModule : Module
    {
        private readonly System.Reflection.Assembly[] _assembliesToScan;

        public AzureServiceBusModule(params System.Reflection.Assembly[] assembliesToScan)
        {
            _assembliesToScan = assembliesToScan;
        }

        protected override void Load(ContainerBuilder builder)
        {
            // Creates our bus from the factory and registers it as a singleton against two interfaces
            builder.Register(c => Bus.Factory.CreateUsingAzureServiceBus(sbc =>
            {
                var serviceUri = ServiceBusEnvironment.CreateServiceUri("sb", ConfigurationManager.AppSettings["AzureSbNamespace"], ConfigurationManager.AppSettings["AzureSbPath"]);

                var host = sbc.Host(serviceUri, h =>
                {
                    h.TokenProvider = TokenProvider.CreateSharedAccessSignatureTokenProvider(ConfigurationManager.AppSettings["AzureSbKeyName"], ConfigurationManager.AppSettings["AzureSbSharedAccessKey"], TimeSpan.FromDays(1), TokenScope.Namespace);
                });

                sbc.ReceiveEndpoint(host, ConfigurationManager.AppSettings["ServiceQueueName"], e =>
                {
                    // Configure your consumer(s)
                    e.Consumer<CheckOrderStatusConsumer>();
                    e.DefaultMessageTimeToLive = TimeSpan.FromMinutes(1);
                    e.EnableDeadLetteringOnMessageExpiration = false;
                });
            }))
                .SingleInstance()
                .As<IBusControl>()
                .As<IBus>();
        }
    }
}
