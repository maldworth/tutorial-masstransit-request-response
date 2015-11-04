namespace StarterKit.Service.Consumer
{
    using System;
    using System.Threading.Tasks;
    using MassTransit;
    using Contracts;

    public class CheckOrderStatusConsumer : IConsumer<CheckOrderStatus>
    {
        public async Task Consume(ConsumeContext<CheckOrderStatus> context)
        {
            Console.Out.WriteLine("Received OrderId: " + context.Message.OrderId);
            context.Respond(new SimpleOrderStatusResult { OrderMessage = string.Format("Echo the OrderId {0}", context.Message.OrderId) });
        }
    }

    public class SimpleOrderStatusResult : OrderStatusResult
    {

        public string OrderMessage { get; set; }
    }

}
