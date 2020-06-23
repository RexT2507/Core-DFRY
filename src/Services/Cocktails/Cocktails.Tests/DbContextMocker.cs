using Cocktails.API.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cocktails.Tests
{
    public static class DbContextMocker
    {
        public static CocktailsDbContext GetCocktailsDbContext(string dbName)
        {
            // Création des options pour l'instance DbContext
            var options = new DbContextOptionsBuilder<CocktailsDbContext>().UseInMemoryDatabase(databaseName: dbName).Options;

            // Création de l'instance de DbContext
            var dbContext = new CocktailsDbContext(options);

            // On ajoute les entités en mémoire ;)
            dbContext.Seed();

            return dbContext;
        }
    }
}
