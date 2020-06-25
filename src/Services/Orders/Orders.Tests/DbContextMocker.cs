using Microsoft.EntityFrameworkCore;
using Orders.API.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orders.Tests
{
    public static class DbContextMocker
    {
        public static OrdersDbContext GetOrdersDbContext(string dbName)
        {
            // Création des options pour l'instance DbContext
            var options = new DbContextOptionsBuilder<OrdersDbContext>().UseInMemoryDatabase(databaseName: dbName).Options;

            // Création de l'instance de DbContext
            var dbContext = new OrdersDbContext(options);

            // On ajoute les entités en mémoire ;)
            dbContext.Seed();

            return dbContext;
        }
    }
}
