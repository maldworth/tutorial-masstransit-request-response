namespace StarterKit.Service
{
    using System.Reflection;
    using Autofac;
    using Modules;

    public class IocConfig
    {
        public static IContainer RegisterDependencies()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<MyService>();

            builder.RegisterModule(new AzureServiceBusModule(Assembly.GetExecutingAssembly()));

            return builder.Build();
        }
    }
}
