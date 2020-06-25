using Auth.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Tests
{
    public class AuthControllerUnitTest
    {

        // Erreurs sur ModelState à vérifier plus tard
        public async Task<IActionResult> TestLogin()
        {
            /*ModelState.AddModelError("error", "error");
            if (!User.IsValid)
            {

            }*/

            return null;

        }
    }
}
