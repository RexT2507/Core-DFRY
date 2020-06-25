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
    public class PreparationController : ControllerRef<CocktailsDbContext, Preparation, int>
    {
        public PreparationController(CocktailsDbContext context) : base(context)
        {

        }
    }
}
