using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TodoWeb.Data;
using TodoWeb.Models;

namespace TodoWeb.Controllers
{
    [Authorize]
    public class TodoController : Controller
    {
        private readonly ApplicationDbContext _context;

        // UserManager ni qo'shish kerak
        private readonly UserManager<IdentityUser> _userManager;

        // Constructor orqali ikkala servisni ham qabul qilamiz
        public TodoController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Todo
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var todos = await _context.Todos
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.CreatedDate)
                .ToListAsync();

            return View(todos);
        }


       

        // GET: Todo/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Todo/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description")] Todo todo)
        {
            // 1. Foydalanuvchi ID-sini olish
            var userId = _userManager.GetUserId(User);

            if (string.IsNullOrEmpty(userId))
            {
                return Challenge(); // Agar foydalanuvchi aniqlanmasa, login sahifasiga yuboradi
            }

            // 2. UserId ni modelga biriktirish
            todo.UserId = userId;
            todo.CreatedDate = DateTime.Now; // Vaqtni ham aniqlashtirib ketamiz

            // 3. ModelState'dan UserId ni olib tashlash (chunki u [Required] lekin View'dan kelmaydi)
            ModelState.Remove("UserId");

            if (ModelState.IsValid)
            {
                _context.Add(todo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(todo);
        }

        // POST: Todo/Complete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Complete(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var todo = await _context.Todos
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (todo != null)
            {
                todo.IsCompleted = !todo.IsCompleted;
                todo.CompletedDate = todo.IsCompleted ? DateTime.Now : null;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Todo/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var todo = await _context.Todos
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (todo != null)
            {
                _context.Todos.Remove(todo);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}