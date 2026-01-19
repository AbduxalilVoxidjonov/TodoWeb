using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoWeb.Data;
using TodoWeb.Models;

namespace TodoWeb.Controllers // Namespace qo'shildi
{
    [Authorize(Roles = "Admin")] // Faqat Admin huquqi borlar kira oladi
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public AdminController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            // Statistika (Umumiy)
            ViewBag.TotalUsers = await _userManager.Users.CountAsync();
            ViewBag.TotalTodos = await _context.Todos.CountAsync();
            ViewBag.CompletedTodos = await _context.Todos.CountAsync(t => t.IsCompleted);

            // Foydalanuvchilar va ularning task statistikasi
            var usersWithStats = await _userManager.Users.Select(u => new
            {
                Id = u.Id,
                UserName = u.UserName,
                Email = u.Email,
                TotalTasks = _context.Todos.Count(t => t.UserId == u.Id),
                ActiveTasks = _context.Todos.Count(t => t.UserId == u.Id && !t.IsCompleted),
                CompletedTasks = _context.Todos.Count(t => t.UserId == u.Id && t.IsCompleted)
            }).ToListAsync();

            return View(usersWithStats);
        }

        // Qo'shimcha: Foydalanuvchini o'chirish funksiyasi (Admin uchun)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null && user.UserName.ToLower() != "admin") // Admin o'zini o'zi o'chira olmaydi
            {
                await _userManager.DeleteAsync(user);
            }
            return RedirectToAction(nameof(Index));
        }


        //User tasklarini ko'rish
        public async Task<IActionResult> UserTasks(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            // Foydalanuvchini topamiz
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Shu foydalanuvchiga tegishli vazifalarni bazadan olamiz
            var tasks = await _context.Todos
                .Where(t => t.UserId == id) // UserId orqali filtrlaymiz
                .ToListAsync();

            ViewBag.UserName = user.UserName; // Sahifada ismini ko'rsatish uchun
            return View(tasks);
        }


        // 1. Yangi task qo'shish (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTaskForUser(string userId, string title)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(title))
            {
                return RedirectToAction("UserTasks", new { id = userId });
            }

            var todo = new Todo
            {
                Title = title,
                UserId = userId, // Taskni aynan shu foydalanuvchiga biriktiramiz
                IsCompleted = false,
                CreatedDate = DateTime.Now
            };

            _context.Todos.Add(todo);
            await _context.SaveChangesAsync();

            return RedirectToAction("UserTasks", new { id = userId });
        }

        // 2. Taskni o'chirib tashlash (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTask(int taskId, string userId)
        {
            var todo = await _context.Todos.FindAsync(taskId);
            if (todo != null)
            {
                _context.Todos.Remove(todo);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("UserTasks", new { id = userId });
        }

        //update task
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateTaskStatus(int taskId, string userId, bool isCompleted)
        {
            var todo = await _context.Todos.FindAsync(taskId);
            if (todo != null)
            {
                todo.IsCompleted = isCompleted;
                _context.Todos.Update(todo);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("UserTasks", new { id = userId });
        }
    }
}