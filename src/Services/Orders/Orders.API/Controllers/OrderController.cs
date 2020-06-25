using ApiLibrary.Core.Base;
using ApiLibrary.Core.Controllers;
using Orders.API.Data;
using Orders.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orders.API.Controllers
{
    public class OrderController : ControllerRef<OrdersDbContext, Order,int>
    {
        public OrderController(OrdersDbContext context): base(context) { }

    }
}
