using ApiLibrary.Core.Base;
using ApiLibrary.Core.Controllers;
using Cocktails.API.Data;
using Cocktails.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cocktails.API.Controllers
{
    public class CocktailController : ControllerRef<CocktailsDbContext,Cocktail,int>
    {
        public CocktailController(CocktailsDbContext context): base(context) { }

    }
}
