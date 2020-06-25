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
    public class OrderDetailsController : ControllerRef<OrdersDbContext, OrderDetails, int>
    {
        public OrderDetailsController(OrdersDbContext context) : base(context)
        {

        }
    }
}
