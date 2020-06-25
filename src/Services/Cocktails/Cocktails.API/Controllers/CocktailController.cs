using ApiLibrary.Core.Base;
using ApiLibrary.Core.Controllers;
using Cocktails.API.Data;
using Cocktails.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cocktails.API.Controllers
{
    public class CocktailController : ControllerRef<CocktailsDbContext,Cocktail,int>
    {
        protected readonly ILogger Logger;
        protected readonly CocktailsDbContext DbContext;

        public CocktailController(ILogger<CocktailController> logger, CocktailsDbContext context) : base(context)
        {
            Logger = logger;
            DbContext = context;
        }

    }
}
