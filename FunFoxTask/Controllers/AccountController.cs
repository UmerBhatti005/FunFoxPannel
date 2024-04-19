using FunFoxTask.FunFoxDbContext;
using FunFoxTask.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FunFoxTask.Controllers
{
    public class AccountController : Controller
    {
        public ApplicationDbContext _dbcontext { get; }

        private UserManager<IdentityAspNetUser> _userManager;
        private SignInManager<IdentityAspNetUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<IdentityAspNetUser> userManager, SignInManager<IdentityAspNetUser> signInManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext dbcontext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _dbcontext = dbcontext;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> SignUp()
        {
            var student = _userManager.GetUsersInRoleAsync("Student").Result.Count();
            ViewData["courses"] = await _dbcontext.courses.Select(x => new SelectListItem { Text = x.CourseName, Value = x.Id.ToString() }).ToListAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel model)
        {
            IdentityAspNetUser identityUser = new IdentityAspNetUser
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                UserName = model.Username,
                EnrolledCourse = model.EnrolledCourse
            };
            var res = await _userManager.CreateAsync(identityUser, model.Password);
            if (res.Succeeded)
            {
                await _userManager.AddToRoleAsync(identityUser, "Student");
                var course = await _dbcontext.courses.FirstOrDefaultAsync(x => x.Id == model.EnrolledCourse);
                course.StudentsEnrolled += 1;
                _dbcontext.courses.Update(course);
                await _dbcontext.SaveChangesAsync();
                TempData["Notifications"] = Notification.SerializeNotifications(new List<Notification>() { new Notification("success", $"SignUp Successfully") });
            }
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                var res = await _signInManager.PasswordSignInAsync(user, model.Password, true, false);
                if (res.Succeeded)
                {
                    TempData["Notifications"] = Notification.SerializeNotifications(new List<Notification>() { new Notification("success", $"{model.Email} Login Successfully") });
                    return RedirectToAction("Index", "Home");
                }
                TempData["Notifications"] = Notification.SerializeNotifications(new List<Notification>() { new Notification("error", "Invalid Email or Password.") });
            }
            TempData["Notifications"] = Notification.SerializeNotifications(new List<Notification>() { new Notification("error", "Invalid Email.") });
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            TempData["Notifications"] = Notification.SerializeNotifications(new List<Notification>() { new Notification("success", $"SignOut Successfully") });
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<JsonResult> AddRole(string roleName)
        {
            IdentityRole identityRole = new IdentityRole
            {
                Name = roleName,
                ConcurrencyStamp = Guid.NewGuid().ToString()
            };
            var res = await _roleManager.CreateAsync(identityRole);
            return Json(res.Succeeded);
        }
    }
}
