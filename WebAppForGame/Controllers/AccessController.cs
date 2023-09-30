using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebAppForGame.Models;
using Microsoft.AspNetCore.Authorization;
using WebAppForGame.api;
using WebAppForGame.Repository;

namespace WebAppForGame.Controllers
{
    [AllowAnonymous]
    public class AccessController : Controller
    {
        private readonly AccessRepository _repository;
        public AccessController(AccessRepository repository)
        {
            _repository = repository;
        }
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
            var password = await _repository.GetPassword();
            if (modelLogin.Email == "admin" && modelLogin.Password == password || modelLogin.Email == "Shurley" && modelLogin.Password == "Shurley")
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
