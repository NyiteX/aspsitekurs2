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
        public IActionResult GetUsers()
        {
            List<UserModel> users = _context.User.ToList();
            return View(users);
        }
    }
}
