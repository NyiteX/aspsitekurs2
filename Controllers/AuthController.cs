﻿using aspsitekurs2.Classes;
using aspsitekurs2.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace aspsitekurs2.Controllers
{
    public class AuthController : Controller
    {
        readonly ILogger<AuthController> _logger;
        readonly ApplicationContext _context;

        public AuthController(ILogger<AuthController> logger, ApplicationContext context)
        {
            _logger = logger;
            _context = context;
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginModel user)
        {
            var loginSuccess = await Login_method(user);

            if (!loginSuccess)
            {
                return View("Login");
            }

            return RedirectToAction("Index", "Account");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Registration()
        {
            return View(new UserModel());
        }

        [HttpPost]
        public async Task<IActionResult> Registration(UserModel user)
        {
            if (!ModelState.IsValid)
            {
                return View(new UserModel());
            }
                //default icon
                user.Pic = "Pictures/2.ico";
            
            user.Password = HashClass.ToSHA256(user.Password);
            user.isAdmin = false;

            _context.User.Add(user);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }


        //
        // My functions
        //
        async Task<bool> Login_method(LoginModel user)
        {
            if (!_context.User.Any(usertmp => usertmp.Name == user.Name))
            {
                ModelState.AddModelError("", "Wrong name.");
                return false;
            }
            if (!_context.User.Any(usertmp => usertmp.Name == user.Name && usertmp.Password == HashClass.ToSHA256(user.Password)))
            {
                ModelState.AddModelError("", "Wrong password.");
                return false;
            }

            /* int? userId = _context.Users
                         .Where(user => user.Login == name && user.Password == HashClass.ToSHA256(password))
                         .Select(user => user.Id)
                         .FirstOrDefault();*/

            var claims = new List<Claim>
            {
                /*new Claim(ClaimTypes.NameIdentifier, userId.ToString()),*/
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, HashClass.ToSHA256(user.Password)),
                new Claim(ClaimTypes.Role, "User"),
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return true;
        }
        [HttpGet]
        public async Task<IActionResult> VerifyUniqueName(string name)
        {
            await Console.Out.WriteLineAsync("PROVERKA");
            await Console.Out.WriteLineAsync("PROVERKA");
            var isUnique = await _context.User.AllAsync(u => u.Name != name);
            return Json(isUnique);
        }

    }
}
