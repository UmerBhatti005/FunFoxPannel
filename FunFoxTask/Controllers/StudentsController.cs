using FunFoxTask.FunFoxDbContext;
using FunFoxTask.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FunFoxTask.Controllers
{
    public class StudentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityAspNetUser> _userManager;

        public StudentsController(ApplicationDbContext context, UserManager<IdentityAspNetUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Students
        public async Task<IActionResult> Index()
        {
            var courses = _context.courses.ToList();
            var student = await _userManager.GetUsersInRoleAsync("Student");
            var students = student.Select(x => new Student
            {
                Id = x.Id,
                FirstName = x.FirstName,
                lastName = x.LastName,
                Username = x.UserName,
                Email = x.Email,
                CourseName = courses?.FirstOrDefault(y => y.Id == x.EnrolledCourse.GetValueOrDefault())?.CourseName
            }).ToList();
            return View(students);
        }

        public async Task<IActionResult> EnrolledCourse()
        {
            var user = await _userManager.GetUserAsync(User);
            ViewData["CourseName"] = _context.courses.FirstOrDefault(x => x.Id == user.EnrolledCourse)?.CourseName;
            return View(user);
        }

        public async Task<IActionResult> UnEnroll(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                var course = await _context.courses.FirstOrDefaultAsync(x => x.Id == user.EnrolledCourse);
                if (course?.StudentsEnrolled != null)
                {
                    course.StudentsEnrolled -= 1;
                    _context.courses.Update(course);
                }
                user.EnrolledCourse = null;
                await _userManager.UpdateAsync(user);
                _context.SaveChangesAsync();
                TempData["Notifications"] = Notification.SerializeNotifications(new List<Notification>() { new Notification("success", $"UnEnroll Successfully") });
            }

            return RedirectToAction("Index");
        }
        
    }
}
