using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using ToDoList.Data;
using ToDoList.Models;
namespace ToDoList.Controllers
{
    [Authorize]
    public class TasksController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public TasksController(AppDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [HttpGet]
        public IActionResult Index()
        {
            string userId = _userManager.GetUserId(User)!;
            var tasks = _context.ToDoTask.Where(task=>task.UserId==userId).ToList();
            return View(tasks);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(ToDoTask data)
        {
            data.UserId = _userManager.GetUserId(User)!;
            _context.ToDoTask.Add(data);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var task = _context.ToDoTask.Find(id);
            if (task == null)
            {
                return RedirectToAction("Index");
            }
            _context.ToDoTask.Remove(task);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Update(int id)
        {
            var task = _context.ToDoTask.Find(id);
            if (task == null)
            {
                return RedirectToAction("Index");
            }
            return View(task);
        }
        [HttpPost]
        public IActionResult Update(ToDoTask EditedTask)
        {
            var task = _context.ToDoTask.Find(EditedTask.Id);
            if (task == null)
            {
                return RedirectToAction("Index");
            }
            task.TaskName = EditedTask.TaskName;
            task.IsDone = EditedTask.IsDone;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SetDoneStatus([FromBody] DoneStatusDto doneStatus)
        {
            var task = _context.ToDoTask.Find(doneStatus.Id);
            if (task == null)
            {
                return RedirectToAction("Index");
            }
            task.IsDone = doneStatus.IsDone;
            _context.SaveChanges();
            return Ok();
        }
    }  
}