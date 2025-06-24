using Microsoft.AspNetCore.Mvc;
using System.Linq;
using ToDoList.Data;
using ToDoList.Models;
namespace ToDoList.Controllers
{
    public class TasksController : Controller
    {
        private readonly AppDbContext _context;
        public TasksController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var task = _context.ToDoTask.ToList();
            return View(task);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(ToDoTask data)
        {
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
    }  
}