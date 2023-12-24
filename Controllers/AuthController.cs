using aspsitekurs2.Classes;
using aspsitekurs2.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
        public async Task<IActionResult> Login(string name, string password)
        {
            var loginSuccess = await Login_method(name, password);

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
            return View("Registration");
        }

        [HttpPost]
        public async Task<IActionResult> Registration(string name, string password, string email)
        {
            byte[] pic;
            string imagePath = "Pictures/2.ico";
            using (FileStream fs = new FileStream(imagePath, FileMode.Open, FileAccess.Read))
            {
                pic = new byte[fs.Length];
                await fs.ReadAsync(pic, 0, pic.Length);
            }

            UserModel user = new UserModel(Id: null, Login: name, Password: HashClass.ToSHA256(password),
                                            Email: email, Pic: pic);

            _context.User.Add(user);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }


        //
        // My functions
        //
        async Task<bool> Login_method(string name, string password)
        {
            bool f = _context.User.Any(user => user.Login == name && user.Password == HashClass.ToSHA256(password));
            if (!f)
            {
                ModelState.AddModelError("", "Wrong login or password.");
                return false;
            }

            /* int? userId = _context.Users
                         .Where(user => user.Login == name && user.Password == HashClass.ToSHA256(password))
                         .Select(user => user.Id)
                         .FirstOrDefault();*/

            var claims = new List<Claim>
            {
                /*new Claim(ClaimTypes.NameIdentifier, userId.ToString()),*/
                new Claim(ClaimTypes.Name, name),
                new Claim(ClaimTypes.Email, HashClass.ToSHA256(password))
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return true;
        }

    }
}
