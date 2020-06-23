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
            var controller = new CocktailsController(null, dbContext);

            /*var response = await controller.GetCocktailsAsync() as ObjectResult;*/
            /*var value = response.Value as IPagedResponse<Cocktail>;*/

            dbContext.Dispose();

            /*Assert.False(value.DidError);*/
        }

        [Fact]
        public async Task TestGetCocktailAsync()
        {
            var dbContext = DbContextMocker.GetCocktailsDbContext(nameof(TestGetCocktailAsync));
            var controller = new CocktailsController(null, dbContext);
            var id = 1;

            /*var response = await controller.GetCocktailAsync(id) as ObjectResult;
            var value = response.Value as ISingleResponse<Cocktail>;*/

            dbContext.Dispose();

            /*Assert.False(value.DidError);*/
        }

        [Fact]
        public async Task TestPostCocktailAsync()
        {
            var dbContext = DbContextMocker.GetCocktailsDbContext(nameof(TestPostCocktailAsync));
            var controller = new CocktailsController(null, dbContext);
            /*var request = new PostCocktailAsync
            {
                ID = 5,
                Nom = "CocoLoco",
                Origine = "",
                Alcool = true,
                Rating = 69
            };

            var response = await controller.PostCocktailAsync(request) as ObjectResult;
            var value = response.Value as ISingleResponse<Cocktail>;*/

            dbContext.Dispose();

            /*Assert.False(value.DidError);*/
        }

        [Fact]
        public async Task TestPutCocktailAsync()
        {
            var dbContext = DbContextMocker.GetCocktailsDbContext(nameof(TestPutCocktailAsync));
            var controller = new CocktailsController(null, dbContext);
            var id = 12;
            /*var request = new PutCocktailAsync
            {
                ID = 5,
                Nom = "CocoLoco",
                Origine = "",
                Alcool = true,
                Rating = 666
            };

            var response = await controller.PutCocktailAsync(id, request) as ObjectResult;
            var value = response.Value as IResponses;*/

            dbContext.Dispose();

            /*Assert.False(value.DidError);*/
        }

        [Fact]
        public async Task TestDeleteCocktailAsync()
        {
            var dbContext = DbContextMocker.GetCocktailsDbContext(nameof(TestDeleteCocktailAsync));
            var controller = new CocktailsController(null, dbContext);
            var id = 5;

            /*var response = await controller.DeleteCocktailAsync(id) as ObjectResult;
            var value = response.Value as IResponses;*/

            dbContext.Dispose();

            /*Assert.False(value.DidError);*/
        }
    }
}
