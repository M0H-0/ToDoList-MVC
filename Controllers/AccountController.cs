using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ToDoList.Controllers;

public class AccountController : Controller
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;

    public AccountController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    [HttpGet]
    public IActionResult SignUp()
    {
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult SignUp(string email, string password)
    {
        var user = new IdentityUser { UserName = email, Email = email };
        var result = _userManager.CreateAsync(user, password).Result;
        if (!result.Succeeded)
        {
            return Content("Error");
        }
        _signInManager.SignInAsync(user, false).Wait();

        return RedirectToAction("Index", "Tasks");
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult SignIn(string email, string password)
    {
        var result = _signInManager.PasswordSignInAsync(email, password, false, false).Result;
        if (!result.Succeeded)
        {
            return Content("error");
        }
        return RedirectToAction("Index", "Tasks");
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult LogOut()
    {
        _signInManager.SignOutAsync().Wait();
        return RedirectToAction("Index");
    }
}