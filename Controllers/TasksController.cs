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


    }  
}