using Auth.API.Data;
using Auth.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Auth.API.Controllers
{
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserDbContext _db;
        public AuthController(UserDbContext db)
        {
            _db = db;
        }

        [Route("/api/v{version:apiVersion}/login")]
        [HttpPost]
        [ApiVersion("1")]
        public async Task<ActionResult> Login([FromForm] string username, [FromForm] string password)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("formation13@cSDJKHISFGZDBHJKFDGFKDGFhkqgfzdjgfdhjsfzdqoyufqsguif");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, username)
                }),
                Expires = DateTime.Now.AddHours(4),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var user = new ExpandoObject();
            var userMap = (IDictionary<string, object>)user;
            userMap.Add("user", username);
            userMap.Add("token", tokenHandler.WriteToken(token));

            User myUser = new User {
                Login = username,
                Password = password,
                Token = tokenHandler.WriteToken(token),
                createdAt = DateTime.Now
            };

            if(ModelState.IsValid)
            {
                try
                {
                    _db.Users.Add(myUser);
                    await _db.SaveChangesAsync();
                }catch(Exception e)
                {
                    return BadRequest(e.Message);
                }
            }


            return Ok(userMap);
        }
    }
}
