using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using onlineExamApp.Data;
using onlineExamApp.Enums;
using onlineExamApp.Models;
using onlineExamApp.Services;
using onlineExamApp.ViewModel;
using OnlineExamSystem.ViewModels;

namespace onlineExamApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ApplicationDbContext _db;

        private readonly IEmailSender emailSender;

        public AccountController(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IEmailSender emailSender,
            ApplicationDbContext db)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.emailSender = emailSender;
            _db = db;
        }

  


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(model);
            }

            var result = await signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var roles = await userManager.GetRolesAsync(user);

                if (roles.Contains(UserRoles.Educator.ToString()))
                {
                    return RedirectToAction("EducatorPage", "Exams");
                }
                else if (roles.Contains(UserRoles.Admin.ToString()))
                {
                    return RedirectToAction("index", "Admin");
                }
                else if (roles.Contains(UserRoles.Student.ToString()))
                {
                    return RedirectToAction("Index", "Students");
                }


                return RedirectToAction("Welcome", "Account");
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(model);
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new ApplicationUser
            {
                DisplayName = model.Name,
                UserName = model.Email,
                NormalizedUserName = model.Email.ToUpper(),
                Email = model.Email,
                NormalizedEmail = model.Email.ToUpper()
            };

            var result = await userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                var emailDomain = model.Email.Split('@')[1].ToLower();

                var universityDomains = new Dictionary<string, string>
{
                        { "ksu.edu.sa", "King Saud University" },
                        { "kau.edu.sa", "King Abdulaziz University" },
                        { "kfu.edu.sa", "King Faisal University" },
                        { "uqu.edu.sa", "Um Al-Qura University" },
                        { "qu.edu.sa", "Qassim University" },
                        { "imamu.edu.sa", "Imam Muhammad bin Saud University" },
                        { "psu.edu.sa", "Prince Sultan University" },
                        { "twu.edu.sa", "Taif University" },
                        { "pnu.edu.sa", "Princess Nourah University" },
                        { "kku.edu.sa", "King Khalid University" },
                        { "saudigov.edu.sa", "Saudi Government Universities" },
                        { "najah.edu.sa", "Najran University" },
                        { "ju.edu.sa", "Jazan University" },
                        { "mu.edu.sa", "Makkah University" },
                        { "shms.edu.sa", "Shaqra University" },
                        { "u.edu.sa", "University of Dammam" },
                        { "huf.edu.sa", "Hail University" }
};


                string university = universityDomains
                    .FirstOrDefault(x => emailDomain.EndsWith(x.Key)).Value;

                if (university == null)
                {
                    university = "Unknown University";
                }

                user.University = university;

                var emailPrefix = model.Email.Split('@')[0];
                UserRoles assignedRole;

                if (emailPrefix.EndsWith(".Ad", StringComparison.OrdinalIgnoreCase))
                {
                    assignedRole = UserRoles.Admin;
                }
                else if (emailPrefix.EndsWith(".Ed", StringComparison.OrdinalIgnoreCase))
                {
                    assignedRole = UserRoles.Educator;
                }
                else if (emailPrefix.EndsWith(".Sa", StringComparison.OrdinalIgnoreCase))
                {
                    assignedRole = UserRoles.Student;
                }
                else
                {
                    ModelState.AddModelError("", "Email does not match any known role suffix.");
                    return View(model);
                }

                var roleName = assignedRole.ToString();
                var roleExists = await roleManager.RoleExistsAsync(roleName);
                if (!roleExists)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }

                await userManager.AddToRoleAsync(user, roleName);

                await signInManager.SignInAsync(user, isPersistent: false);

                return RedirectToAction("Login", "Account");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }


        [HttpGet]
        public IActionResult VerifyEmail()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendResetLink(VerifyEmailViewModel model)
        {
            if (!ModelState.IsValid)
                return View("VerifyEmail", model);

            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "User not found.");
                return View("VerifyEmail", model);
            }

            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var resetLink = Url.Action("ResetPassword", "Account", new { email = model.Email, token = token }, Request.Scheme);

            await emailSender.SendEmailAsync(model.Email, "Reset Your Password", $"Click this link to reset your password: <a href='{resetLink}'>Reset Password</a>");

            TempData["Message"] = "A reset link has been sent to your email.";
            return RedirectToAction("Login");
        }




        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string email, string token)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login");
            }

            var model = new ResetPasswordViewModel
            {
                Email = email,
                Token = token
            };

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var result = await userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("Login", "Account");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Welcome", "Account");
        }

        [HttpGet]
        public async Task<IActionResult> Welcome()
        {
            if (User.Identity?.IsAuthenticated ?? false)
            {
                var user = await userManager.GetUserAsync(User);
                if (user == null) return Challenge();

                var roles = await userManager.GetRolesAsync(user);

                if (roles.Contains(UserRoles.Admin.ToString()))
                    return RedirectToAction("index", "Admin");
                else if (roles.Contains(UserRoles.Educator.ToString()))
                    return RedirectToAction("EducatorPage", "Exams");
                else if (roles.Contains(UserRoles.Student.ToString()))
                    return RedirectToAction("Index", "Students");
                else
                    return RedirectToAction("Index", "Home");
            }

            return View();
        }
    }
}
