using System.Net;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Collections.Generic;
using System.Security.Claims;

namespace WebApp
{
    [Route("api")]
    public class LoginController : Controller
    {
        private readonly IAccountDatabase _db;

        public LoginController(IAccountDatabase db)
        {
            _db = db;
        }

        [HttpPost("sign-in")]
        public async Task<IActionResult> Login(string userName)
        {
            //TODO 1: Generate auth cookie for user 'userName' with external id ?
            var account = await _db.FindByUserNameAsync(userName);
            if (account != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, account.ExternalId),
                    new Claim(ClaimTypes.Role, account.Role)
                };
                var userId = new ClaimsIdentity(claims, "Cookie");
                await HttpContext.SignInAsync("Cookie", new ClaimsPrincipal(userId));
                return Ok();
            }
            // TODO 2: Return 404 if user not found ?
            return NotFound();
        }
    }
}