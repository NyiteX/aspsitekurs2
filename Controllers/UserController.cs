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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Users()
        {
            List<UserModel> users = await _context.User.ToListAsync();
            return View(users);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AccountInfo(int id)
        {
            await Console.Out.WriteLineAsync(id.ToString());
            await Console.Out.WriteLineAsync(User.Claims.ElementAtOrDefault(0).Value);
            if (!User.Identities.Any(u => u.Claims.ElementAtOrDefault(0).Value == id.ToString())) 
            {
                return RedirectToAction("Login","Auth");
            }
            var user = await _context.User.FirstOrDefaultAsync(u => u.ID == id);

            return View(user);
        }
    }
}
