using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenUp.ViewModels.Authentication;
using OpenUpData.Helpers.Constants;
using OpenUpData.Models;

namespace OpenUp.Controllers;

public class AuthenticationController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    public AuthenticationController(UserManager<User> userManager,
        SignInManager<User> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }
    public async Task<IActionResult> Login()
    {
        return View();
    }

    public async Task<IActionResult> Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterVM registerVM)
    {
        var newUser = new User()
        {
            FullName = $"{registerVM.FirstName} {registerVM.LastName}",
            Email = registerVM.Email,
            UserName = registerVM.Email
        };

        var result = await _userManager.CreateAsync(newUser, registerVM.Password);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(newUser, AppRoles.User);

            await _signInManager.SignInAsync(newUser, isPersistent: false);
            return RedirectToAction("Index", "Home");
        }

        return View();
    }
}

