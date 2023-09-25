using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebAppForGame.Models;
using Microsoft.AspNetCore.Authorization;

namespace WebAppForGame.Controllers
{
    [AllowAnonymous]
    public class AccessController : Controller
    {
        [AllowAnonymous]
        public IActionResult Login()
        {
            ClaimsPrincipal claimUser = HttpContext.User;

            if (claimUser.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Admin");


            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(VmLogin modelLogin)
        {
            if (modelLogin.Email == "dmitriy@waznaw.ru" && modelLogin.Password == "test123")
            { 
                List<Claim> claims = new List<Claim>() {
                    new Claim(ClaimTypes.NameIdentifier, modelLogin.Email),
                    new Claim(ClaimTypes.Name, modelLogin.Email),
                    new Claim(ClaimTypes.Role, "Admin")
                };

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,
                    CookieAuthenticationDefaults.AuthenticationScheme);

                AuthenticationProperties properties = new AuthenticationProperties()
                {

                    AllowRefresh = true,
                    IsPersistent = modelLogin.KeepLoggedIn
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity), properties);

                return RedirectToAction("Index", "Admin");
            }



            ViewData["ValidateMessage"] = "Неправильные данные для входа";
            return View();
        }
    }
}
