namespace StarterKit.Web.Bootstrapper.Modules
{
    using System;
    using Autofac;
    using MassTransit;
    using System.Configuration;
    using StarterKit.Contracts;
    using Microsoft.ServiceBus;

    public class RequestClientModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            Uri address = new Uri(ConfigurationManager.AppSettings["AzureQueueFullUri"]);
            TimeSpan requestTimeout = TimeSpan.FromSeconds(30);

            builder.Register(c => new MessageRequestClient<CheckOrderStatus, OrderStatusResult>(c.Resolve<IBus>(), address, requestTimeout))
                .As<IRequestClient<CheckOrderStatus, OrderStatusResult>>()
                .SingleInstance();
        }
    }
}
