using aspsitekurs2.Classes;
using aspsitekurs2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace aspsitekurs2.Controllers
{
    public class ProductController : Controller
    {
        readonly ILogger<ProductController> _logger;
        readonly ApplicationContext _context;

        public ProductController(ILogger<ProductController> logger, ApplicationContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetProducts()
        {
            List<ProductModel> products = _context.Product.ToList();

            return View(products);
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Search(string? search)
        {
            var products = _context.Product.Where(u => u.Name.Contains(search)).ToList();

            if (products.Count < 1)
            {
                ViewData["Notfound"] = search;
                return View(products);
            }
            return View(products);
        }
    }
}
