using aspsitekurs2.Classes;
using aspsitekurs2.Models;
using aspsitekurs2.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Data;

namespace aspsitekurs2.Controllers
{
    public class AdminPanelController : Controller
    {
        readonly ILogger<AdminPanelController> _logger;
        readonly ApplicationContext _context;

        public AdminPanelController(ILogger<AdminPanelController> logger, ApplicationContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            int userid = Convert.ToInt32(User.Claims.FirstOrDefault().Value);
            var user = _context.User.FirstOrDefault(u => u.ID == userid);

            return View(user);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Users()
        {
            int userid = Convert.ToInt32(User.Claims.FirstOrDefault().Value);

            List<UserModel> users = await _context.User.ToListAsync();
            var userVM = new UserViewModel();
            userVM.UserList = users;
            userVM.UserInfo = await _context.User.FirstOrDefaultAsync(u => u.ID == userid);

            return View(userVM);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Products()
        {
            int userid = Convert.ToInt32(User.Claims.FirstOrDefault().Value);

            var productVM = new ProductViewModel();
            List<ProductModel> products = await _context.Product.ToListAsync();

            productVM.UserInfo = await _context.User.FirstOrDefaultAsync(p => p.ID == userid);
            productVM.ProductList = products;

            return View(productVM);
        }

        public async Task<IActionResult> AddProduct(ProductEditModel product)
        {
            if (!ModelState.IsValid)
            {
                int userid = Convert.ToInt32(User.Claims.FirstOrDefault().Value);

                var productVM = new ProductViewModel();
                List<ProductModel> products = await _context.Product.ToListAsync();

                productVM.UserInfo = await _context.User.FirstOrDefaultAsync(p => p.ID == userid);
                productVM.ProductList = products;
                productVM.ProductEdit = product;
                productVM.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                return View("Products", productVM);
            }

            var isUniqueName = await _context.Product.AllAsync(u => u.Name != product.Name);

            if (!isUniqueName)
            {
                int userid = Convert.ToInt32(User.Claims.FirstOrDefault().Value);

                var productVM = new ProductViewModel();
                List<ProductModel> products = await _context.Product.ToListAsync();

                productVM.UserInfo = await _context.User.FirstOrDefaultAsync(p => p.ID == userid);
                productVM.ProductList = products;
                productVM.ProductEdit = product;

                ModelState.AddModelError(nameof(product.Name), "This name is already in use.");
                productVM.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return View("Products", productVM);
            }

            var newProduct = new ProductModel();
            newProduct.Name = product.Name;
            newProduct.Description = product.Description;               
            newProduct.Category = product.Category;
            newProduct.Count = product.Count;
            newProduct.Price = product.Price;

            if (product.NewPic != null && product.NewPic.Length > 0)
            {
                string fileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(product.NewPic.FileName);

                string folderPath = Path.Combine("Pictures", "Products");
                string filePath = Path.Combine(folderPath, fileName);

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await product.NewPic.CopyToAsync(fileStream);
                }

                newProduct.Pic = fileName;  
            }

            _context.Product.Add(newProduct);
            await _context.SaveChangesAsync();

            return RedirectToAction("Products");
        }

        [HttpPost]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> DeleteProduct(int ID)
        {
            var product = await _context.Product.FirstOrDefaultAsync(v => v.ID == ID);
            if (product != null)
            {
                string folderPath = Path.Combine("Pictures", "Products");
                var oldPic = Path.Combine(folderPath, product.Pic);
                if (System.IO.File.Exists(oldPic))
                {
                    if (product.Pic != "2.ico")
                        System.IO.File.Delete(oldPic);
                }

                _context.Product.Remove(product);
                _context.SaveChanges();
            }
            return RedirectToAction("Products");
        }

    }
}
