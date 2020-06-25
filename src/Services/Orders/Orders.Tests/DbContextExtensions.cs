using Orders.API.Data;
using Orders.API.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orders.Tests
{
    public static class DbContextExtensions
    {
        public static void Seed(this OrdersDbContext dbContext)
        {
            /*dbContext.Orders.Add(new OrderDetails
            {
                ID_Order = 1,
                order = null,
                ID_Cocktail = 1,
                cocktail = { },
                Quantite = 2
            });*/

            dbContext.SaveChanges();
        }
    }
}
