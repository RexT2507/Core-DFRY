using Microsoft.AspNetCore.Mvc;
using Orders.API.Controllers;
using Orders.API.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Orders.Tests
{
    public class OrdersControllerUnitTest
    {

        [Fact]
        public async Task TestGetOrdersAsync()
        {
            var dbContext = DbContextMocker.GetOrdersDbContext(nameof(TestGetOrdersAsync));
            var controller = new OrderController(null, dbContext);

            var response = await controller.GetElements(null, null, null);
            var value = response.Value as IPagedResponse<Order>;

            dbContext.Dispose();

            Assert.False(value.DidError);
        }

        [Fact]
        public async Task TestGetOrderAsync()
        {
            var dbContext = DbContextMocker.GetOrdersDbContext(nameof(TestGetOrderAsync));
            var controller = new OrderController(null, dbContext);
            var id = 1;

            var response = await controller.GetElementById(id);
            var value = response.Value as ISingleResponse<Order>;

            dbContext.Dispose();

            Assert.False(value.DidError);
        }

        [Fact]
        public async Task TestPostOrderAsync()
        {
            var dbContext = DbContextMocker.GetOrdersDbContext(nameof(TestPostOrderAsync));
            var controller = new OrderController(null, dbContext);
            var request = new OrderDetails
            {
                ID_Order = 1,
                order = null,
                ID_Cocktail = 1,
                cocktail = { },
                Quantite = 2
            };

            /*var response = await controller.CreateElement(request);
            var value = response.Value as ISingleResponse<Order>;

            dbContext.Dispose();

            Assert.False(value.DidError);*/
        }

        [Fact]
        public async Task TestPutOrderAsync()
        {
            var dbContext = DbContextMocker.GetOrdersDbContext(nameof(TestPutOrderAsync));
            var controller = new OrderController(null, dbContext);
            var id = 12;
            var request = new OrderDetails
            {
                ID_Order = 1,
                order = null,
                ID_Cocktail = 1,
                cocktail = { },
                Quantite = 2
            };

            /*var response = await controller.UpdateElement(id, request);
            var value = response.Value as IResponses;

            dbContext.Dispose();

            Assert.False(value.DidError);*/
        }

        [Fact]
        public async Task TestDeleteOrderAsync()
        {
            var dbContext = DbContextMocker.GetOrdersDbContext(nameof(TestDeleteOrderAsync));
            var controller = new OrderController(null, dbContext);
            var id = 5;

            var response = await controller.DeleteElement(id) as ObjectResult;
            var value = response.Value as IResponses;

            dbContext.Dispose();

            Assert.False(value.DidError);
        }
    }

}
