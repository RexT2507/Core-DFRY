using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cocktails.API.Data;
using Cocktails.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Cocktails.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CocktailsController : ControllerBase
    {
        protected readonly ILogger Logger;
        protected readonly CocktailsDbContext DbContext;

        public CocktailsController(ILogger<CocktailsController> logger, CocktailsDbContext dbContext)
        {
            Logger = logger;
            DbContext = dbContext;
        }

        [HttpGet("Cocktails")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<Cocktail> GetCocktailsAsync()
        {
            return null;
        }
    }


}
