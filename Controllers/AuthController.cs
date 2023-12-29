using aspsitekurs2.Classes;
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
                return View(user);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
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

            var isUniqueName = _context.User.All(u => u.Name != user.Name);
            var isUniqueEmail = _context.User.All(u => u.Email != user.Email);

            if (!isUniqueName)
            {
                ModelState.AddModelError(nameof(user.Name), "This name is already in use.");
                return View(user);
            }
            else if(!isUniqueEmail)
            {
                ModelState.AddModelError(nameof(user.Email), "This email is already in use.");
                return View(user);
            }

            //default icon
            user.Pic = "2.ico";
            
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
                ModelState.AddModelError(nameof(user.Name), "Wrong name.");
                return false;
            }
            if (!_context.User.Any(usertmp => usertmp.Name == user.Name && usertmp.Password == HashClass.ToSHA256(user.Password)))
            {
                ModelState.AddModelError(nameof(user.Password), "Wrong password.");
                return false;
            }

            int? userId = _context.User
                        .Where(u => u.Name == user.Name && user.Password == HashClass.ToSHA256(user.Password))
                        .Select(user => user.ID)
                        .FirstOrDefault();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, HashClass.ToSHA256(user.Password)),
                new Claim(ClaimTypes.Role, "User"),
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return true;
        }
        public async Task<bool> VerifyUniqueName(string name)
        {
            var isUnique = await _context.User.AllAsync(u => u.Name != name);
            return isUnique;
        }

    }
}
