using GolfBag.Entities;
using GolfBag.Services;
using GolfBag.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolfBag.Controllers
{
    public class AccountController : Controller
    {
        private SignInManager<User> _signInManager;
        private UserManager<User> _userManager;
        private IRoundOfGolf _roundOfGolf;

        public AccountController(UserManager<User> userManager,
            SignInManager<User> signInManager,
            IRoundOfGolf roundOfGolf)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roundOfGolf = roundOfGolf;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost, AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Register(RegisterUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    UserName = model.Username,
                    FirstName = model.FirstName,
                    LastName = model.LastName
                };

                var createResult = await _userManager.CreateAsync(user, model.Password);
                if (createResult.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in createResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);  //these errors will appear in validation summary because they're not associated with a specific model
                    }
                }
            }
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout ()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Login (LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var loginResult = await _signInManager.PasswordSignInAsync(
                    model.Username,
                    model.Password,
                    model.RememberMe,
                    false);

                if (loginResult.Succeeded)
                {
                    if (Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            ModelState.AddModelError("", "Could not log in");
            return View(model);
        }
        
        //[HttpGet, ValidateAntiForgeryToken]
        public IActionResult ManageAccount()
        {
            var currentUser = GetCurrentUserAsync().Result;
            var manageAccountViewModel = new ManageAccountViewModel {
                FirstName = currentUser.FirstName,
                LastName = currentUser.LastName
            };
            return View(manageAccountViewModel);
        }

        public IActionResult SaveChanges(ManageAccountViewModel model)
        {
            if (ModelState.IsValid)
            {               
                var currentUser = GetCurrentUserAsync().Result;
                currentUser.FirstName = model.FirstName;
                currentUser.LastName = model.LastName;
                _userManager.UpdateAsync(currentUser);
                _roundOfGolf.Commit();
            }
            return RedirectToAction("Index", "Home");
        }

        private async Task<User> GetCurrentUserAsync()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            return currentUser;
        }
    }
}
