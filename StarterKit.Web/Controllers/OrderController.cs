using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MassTransit;

namespace StarterKit.Web.Controllers
{
    using System.Threading.Tasks;
    using Contracts;
    using ViewModels;


    public class OrderController : Controller
    {
        private readonly IRequestClient<CheckOrderStatus, OrderStatusResult> _client;

        public OrderController(IRequestClient<CheckOrderStatus, OrderStatusResult> client)
        {
            _client = client;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Status(CheckOrderStatusViewModel model)
        {
            var result = await _client.Request(new SimpleCheckOrderStatus
            {
                OrderId = model.OrderId
            });

            return View(result);
        }
    }

    public class SimpleCheckOrderStatus : CheckOrderStatus
    {

        public string OrderId { get; set; }
    }
}