using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using onlineExamApp.Data;
using onlineExamApp.Enums;
using onlineExamApp.Models;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace onlineExamApp.Controllers
{
    public class ProfileController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<ProfileController> _logger;

        private readonly string[] allowedExt = new[] { ".jpg", ".jpeg", ".png" };
        private const long MAX_BYTES = 2 * 1024 * 1024;

        public ProfileController(ApplicationDbContext db, UserManager<ApplicationUser> um, IWebHostEnvironment env, ILogger<ProfileController> logger)
        {
            _db = db;
            _userManager = um;
            _env = env;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains(UserRoles.Admin.ToString()))
                ViewBag.ReturnUrl = Url.Action("index", "Admin");
            else if (roles.Contains(UserRoles.Educator.ToString()))
                ViewBag.ReturnUrl = Url.Action("EducatorPage", "Exams");
            else if (roles.Contains(UserRoles.Student.ToString()))
                ViewBag.ReturnUrl = Url.Action("Index", "Students");

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(ApplicationUser model, string? NewPassword, string? ConfirmPassword)
        {
            if (!ModelState.IsValid)
                return View("Edit", model);

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            if (string.IsNullOrWhiteSpace(model.DisplayName))
            {
                user.DisplayName = user.Email.Split('.')[0];
            }
            else
            {
                user.DisplayName = model.DisplayName;
            }

            if (!string.IsNullOrEmpty(NewPassword))
            {
                if (NewPassword != ConfirmPassword)
                {
                    TempData["Error"] = "Passwords do not match.";
                    return View("Edit", model);
                }

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var passwordResult = await _userManager.ResetPasswordAsync(user, token, NewPassword);

                if (!passwordResult.Succeeded)
                {
                    TempData["Error"] = "Failed to update password.";
                    return View("Edit", model);
                }
            }

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
                TempData["Success"] = "Profile updated successfully.";
            else
                TempData["Error"] = "Failed to update profile.";

            return RedirectToAction("Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadImage(IFormFile profileImage)
        {
            if (profileImage == null || profileImage.Length == 0)
            {
                TempData["Error"] = "No file selected";
                return RedirectToAction("Edit");
            }

            if (profileImage.Length > MAX_BYTES)
            {
                TempData["Error"] = "File too large (max 2MB)";
                return RedirectToAction("Edit");
            }

            var ext = Path.GetExtension(profileImage.FileName).ToLowerInvariant();
            if (!allowedExt.Contains(ext))
            {
                TempData["Error"] = "Invalid file type. Only jpg/png allowed.";
                return RedirectToAction("Edit");
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var fileName = $"{Guid.NewGuid()}{ext}";
            var savePath = Path.Combine(_env.WebRootPath, "images", "profiles");

            if (!Directory.Exists(savePath))
                Directory.CreateDirectory(savePath);

            var fullPath = Path.Combine(savePath, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await profileImage.CopyToAsync(stream);
            }

            if (!string.IsNullOrEmpty(user.ProfileImagePath) && user.ProfileImagePath.StartsWith("/images/profiles/"))
            {
                var oldImagePath = Path.Combine(_env.WebRootPath, user.ProfileImagePath.TrimStart('/'));
                if (System.IO.File.Exists(oldImagePath))
                {
                    try { System.IO.File.Delete(oldImagePath); } catch { }
                }
            }

            user.ProfileImagePath = $"/images/profiles/{fileName}";
            await _userManager.UpdateAsync(user);

            TempData["Success"] = "Profile image uploaded.";
            return RedirectToAction("Edit");
        }
    }
}


