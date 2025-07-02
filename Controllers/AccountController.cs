using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Services;

namespace ToDoList.Controllers;

public class AccountController : Controller
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly GmailSender _gmailSender;

    public AccountController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, GmailSender gmailSender)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _gmailSender = gmailSender;
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
            return Content("Weak Password or already used Email");
        }
        var token = _userManager.GenerateEmailConfirmationTokenAsync(user).Result;
        var link = Url.Action("ConfirmEmail",
          "Account",
          new { userId = user.Id, token },
          protocol: Request.Scheme);
        _gmailSender.SendEmail(user.Email, "Confirm Your ToDoList Email",$"Click the link to confirm you Email: <a href='{link}'> Confirm </a>");
        //_signInManager.SignInAsync(user, false).Wait();

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
        var user = _userManager.FindByEmailAsync(email).Result;
        if (user == null)
        {
            return Content("No Account");
        }
        if (!_userManager.IsEmailConfirmedAsync(user).Result)
        {
            return Content("Please Confirm Your Email First");
        }
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
    [HttpGet]
    public IActionResult ConfirmEmail(string userId, string token)
    {
        var user = _userManager.FindByIdAsync(userId).Result;
        if (user == null)
        {
            return Content("Not Found");
        }
        var confirm = _userManager.ConfirmEmailAsync(user, token).Result;
        if (confirm.Succeeded)
        {
            _signInManager.SignInAsync(user,false).Wait();
            return RedirectToAction("Index", "Tasks");
        }
        else
        {
            return Content("Token issue");
        }
    }
}