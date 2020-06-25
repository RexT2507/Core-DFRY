using ApiLibrary.Core.Base;
using ApiLibrary.Core.Controllers;
using Microsoft.Extensions.Logging;
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
        protected readonly ILogger Logger;
        protected readonly OrdersDbContext DbContext;

        public OrderController(ILogger<OrderController> logger, OrdersDbContext context): base(context) 
        {
            Logger = logger;
            DbContext = context;
        }

    }
}
