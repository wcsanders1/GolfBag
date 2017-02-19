using GolfBag.Entities;
using GolfBag.Services;
using GolfBag.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MailKit.Net.Smtp;
using MimeKit;
using MailKit.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using Google.Apis.Auth.OAuth2;
using System.Threading;
using Microsoft.Extensions.Configuration;

namespace GolfBag.Controllers
{
    public class AccountController : Controller
    {
        private SignInManager<User> _signInManager;
        private UserManager<User> _userManager;
        private IRoundOfGolf _roundOfGolf;
        private IConfigurationRoot _config;

        public AccountController(UserManager<User> userManager,
            SignInManager<User> signInManager,
            IRoundOfGolf roundOfGolf,
            IConfigurationRoot config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roundOfGolf = roundOfGolf;
            _config = config;
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
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName
                };

                var createResult = await _userManager.CreateAsync(user, model.Password);
                if (createResult.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);

                    // send confirmation email
                    string confirmationToken = _userManager
                        .GenerateEmailConfirmationTokenAsync(user).Result;

                    string confirmationLink = Url.Action("ConfirmEmail",
                        "Account", new
                        {
                            userid = user.Id,
                            token = confirmationToken
                        },
                        protocol: HttpContext.Request.Scheme);

                    SendConfirmationEmail(user.Email,
                        _config["email:subject"],
                        _config["email:body"] + " " + confirmationLink,
                        confirmationToken);

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

        public bool ConfirmEmail(string userid, string token)   // MAKE THIS ACTUALLY FORCE THE USER TO CONFIRM EMAIL
        {
            User user = _userManager.FindByIdAsync(userid).Result;
            IdentityResult result = _userManager.ConfirmEmailAsync(user, token).Result;
            return result.Succeeded;
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
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
        public async Task<IActionResult> Login(LoginViewModel model)
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
            var manageAccountViewModel = new ManageAccountViewModel
            {
                FirstName = currentUser.FirstName,
                LastName = currentUser.LastName,
                UserName = currentUser.UserName
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
                currentUser.UserName = model.UserName;
                _userManager.UpdateAsync(currentUser).Wait();
                _roundOfGolf.Commit();
            }
            return RedirectToAction("Index", "Home");
        }

        private async Task<User> GetCurrentUserAsync()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            return currentUser;
        }

        private void SendConfirmationEmail(string email,
            string subject, 
            string message,
            string confirmationToken)
        {
            MimeMessage emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_config["email:address"]));
            emailMessage.To.Add(new MailboxAddress(email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart("plain") { Text = message };

            using (SmtpClient client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Authenticate(_config["email:address"], _config["email:password"]);
                client.Send(emailMessage);
                client.Disconnect(true);
            }
        }
    }
}
