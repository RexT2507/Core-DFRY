using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Orders.API.Data;
using Orders.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Orders.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        protected readonly ILogger Logger;
        protected readonly OrdersDbContext DbContext;

        public OrdersController(ILogger<OrdersController> logger, OrdersDbContext dbContext)
        {
            Logger = logger;
            DbContext = dbContext;
        }

        [HttpGet("Orders")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<Order> GetOrdersAsync()
        {
            return null;
        }
    }


}
