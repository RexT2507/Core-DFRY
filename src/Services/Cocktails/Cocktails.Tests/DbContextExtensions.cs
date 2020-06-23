using Cocktails.API.Data;
using Cocktails.API.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cocktails.Tests
{
    public static class DbContextExtensions
    {
        public static void Seed(this CocktailsDbContext dbContext)
        {
            dbContext.Cocktails.Add(new Cocktail
            {
                ID = 1,
                Nom = "Mojito",
                Origine = "",
                Alcool = true,
                Rating = 10
            });

            dbContext.Cocktails.Add(new Cocktail
            {
                ID = 2,
                Nom = "Punch",
                Origine = "",
                Alcool = true,
                Rating = 18
            });

            dbContext.Cocktails.Add(new Cocktail
            {
                ID = 3,
                Nom = "Diabolo",
                Origine = "",
                Alcool = false,
                Rating = 0
            });

            dbContext.SaveChanges();
        }
    }
}
