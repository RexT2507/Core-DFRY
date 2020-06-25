using Cocktails.API.Controllers;
using Cocktails.API.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Cocktails.Tests
{
    public class CocktailsControllerUnitTest
    {
        [Fact]
        public async Task TestGetCocktailsAsync()
        {
            var dbContext = DbContextMocker.GetCocktailsDbContext(nameof(TestGetCocktailsAsync));
            var controller = new CocktailController(null, dbContext);

            var response = await controller.GetElements(null, null, null);
            var value = response.Value as IPagedResponse<Cocktail>;

            dbContext.Dispose();

            Assert.False(value.DidError);
        }

        [Fact]
        public async Task TestGetCocktailAsync()
        {
            var dbContext = DbContextMocker.GetCocktailsDbContext(nameof(TestGetCocktailAsync));
            var controller = new CocktailController(null, dbContext);
            var id = 1;

            var response = await controller.GetElementById(id);
            var value = response.Value as ISingleResponse<Cocktail>;

            dbContext.Dispose();

            Assert.False(value.DidError);
        }

        [Fact]
        public async Task TestPostCocktailAsync()
        {
            var dbContext = DbContextMocker.GetCocktailsDbContext(nameof(TestPostCocktailAsync));
            var controller = new CocktailController(null, dbContext);
            var request = new Cocktail
            {
                ID = 5,
                Nom = "CocoLoco",
                Origine = "",
                Alcool = true,
                Rating = 69
            };

            var response = await controller.CreateElement(request) as ObjectResult;
            var value = response.Value as ISingleResponse<Cocktail>;

            dbContext.Dispose();

            Assert.False(value.DidError);
        }

        [Fact]
        public async Task TestPutCocktailAsync()
        {
            var dbContext = DbContextMocker.GetCocktailsDbContext(nameof(TestPutCocktailAsync));
            var controller = new CocktailController(null, dbContext);
            var id = 12;
            var request = new Cocktail
            {
                ID = 5,
                Nom = "CocoLoco",
                Origine = "",
                Alcool = true,
                Rating = 42
            };

            var response = await controller.UpdateElement(id, request) as ObjectResult;
            var value = response.Value as IResponses;

            dbContext.Dispose();

            Assert.False(value.DidError);
        }

        [Fact]
        public async Task TestDeleteCocktailAsync()
        {
            var dbContext = DbContextMocker.GetCocktailsDbContext(nameof(TestDeleteCocktailAsync));
            var controller = new CocktailController(null, dbContext);
            var id = 5;

            var response = await controller.DeleteElement(id) as ObjectResult;
            var value = response.Value as IResponses;

            dbContext.Dispose();

            Assert.False(value.DidError);
        }
    }
}
