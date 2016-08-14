namespace StarterKit.Web.Bootstrapper.Modules
{
    using System;
    using Autofac;
    using MassTransit;
    using MassTransit.AzureServiceBusTransport;
    using System.Configuration;
    using Microsoft.ServiceBus;

    public class AzureServiceBusModule : Module
    {
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
            }))
                .SingleInstance()
                .As<IBusControl>()
                .As<IBus>();
        }
    }
}
