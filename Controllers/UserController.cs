using aspsitekurs2.Classes;
using aspsitekurs2.Models;
using aspsitekurs2.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

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
            if (!User.Identities.Any(u => u.Claims.ElementAtOrDefault(0).Value == id.ToString()))
            {
                return RedirectToAction("Login", "Auth");
            }
            var user = await _context.User.FirstOrDefaultAsync(u => u.ID == id);

            var userVM = new UserViewModel();
            userVM.CopyUserInfo(user);

            return View(userVM);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Update(UserEditModel user)
        {
            var updatedUser = await _context.User.FirstOrDefaultAsync(u => u.ID == user.ID);
            var userVM = new UserViewModel();
            userVM.CopyUserInfo(updatedUser);

            

            if (!ModelState.IsValid)
            {
                await Console.Out.WriteLineAsync("Model Error");
                return View("AccountInfo", userVM);
            }

            var isUniqueName = await _context.User.AllAsync(u => u.Name != user.Name);
            var isUniqueEmail = await _context.User.AllAsync(u => u.Email != user.Email);

            if (!isUniqueName)
            {
                ModelState.AddModelError(nameof(user.Name), "This name is already in use.");
                return View("AccountInfo", userVM);
            }
            if (!isUniqueEmail)
            {
                ModelState.AddModelError(nameof(user.Email), "This email is already in use.");
                return View("AccountInfo", userVM);
            }


            if (updatedUser != null)
            {
                if(user.Name != null)
                {
                    updatedUser.Name = user.Name;
                }
                if (user.Email != null)
                {
                    updatedUser.Email = user.Email;
                }
                if (user.Password != null)
                {
                    updatedUser.Password = user.Password;
                }
                if (user.NewPic != null && user.NewPic.Length > 0)
                {
                    string fileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(user.NewPic.FileName);

                    string folderPath = Path.Combine("Pictures", "Users");
                    string filePath = Path.Combine(folderPath, fileName);

                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await user.NewPic.CopyToAsync(fileStream);
                    }

                    var oldPic = Path.Combine(folderPath, updatedUser.Pic);
                    if (System.IO.File.Exists(oldPic))
                    {
                        if(updatedUser.Pic != "2.ico")
                        System.IO.File.Delete(oldPic);
                    }

                    updatedUser.Pic = fileName;
                }


                _context.User.Update(updatedUser);
                await _context.SaveChangesAsync();
            }

            userVM.CopyUserInfo(updatedUser);

            return View("AccountInfo", userVM);
        }


    }
}
