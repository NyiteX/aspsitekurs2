using aspsitekurs2.Classes;
using aspsitekurs2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace aspsitekurs2.Controllers
{
    public class UserController : Controller
    {
        readonly ILogger<UserController> _logger;
        readonly ApplicationContext _context;

        public UserController(ILogger<UserController> logger, ApplicationContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Users()
        {
            List<UserModel> users = await _context.User.ToListAsync();
            return View(users);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AccountInfo(string username)
        {
            if(!User.Identities.Any(u => u.Name == username)) 
            {
                return RedirectToAction("Login","Auth");
            }
            var user = await _context.User.FirstOrDefaultAsync(u => u.Name == username);

            return View(user);
        }
    }
}
