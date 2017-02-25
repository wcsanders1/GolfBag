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
                    SendConfirmationEmail(user);

                    // take user to ConfirmEmail view
                    var confirmEmail = new ConfirmEmailViewModel
                    {
                        Email = user.Email,
                        EmailConfirmed = user.EmailConfirmed,
                        UserId = user.Id
                    };

                    ViewBag.Message = $"A confirmation link has been sent to {user.Email}. Please click that link to enable login to Golf Bag.";
                    return View("ConfirmEmail", confirmEmail);
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

        // This action is called when the user clicks his email confirmation link.
        public IActionResult ConfirmEmail(string userid, string token)
        {
            User user = _userManager.FindByIdAsync(userid).Result;
            IdentityResult result = _userManager.ConfirmEmailAsync(user, token).Result;

            if (result.Succeeded)
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.Message = "Your email could not be confirmed.";
            return View("ConfirmEmail");
        }

        public IActionResult ChangeEmail(ConfirmEmailViewModel confirmEmailViewModel)
        {
            if (ModelState.IsValid)
            {
                var currentUser = _userManager.FindByIdAsync(confirmEmailViewModel.UserId).Result;
                currentUser.Email = confirmEmailViewModel.Email;
                _userManager.UpdateAsync(currentUser).Wait();
                _roundOfGolf.Commit();

                SendConfirmationEmail(currentUser);

                var confirmEmail = new ConfirmEmailViewModel
                {
                    Email = currentUser.Email,
                    EmailConfirmed = currentUser.EmailConfirmed,
                    UserId = currentUser.Id
                };

                ViewBag.Message = $"A confirmation link has been sent to {currentUser.Email}. Please click that link to enable login to Golf Bag.";
                return View("ConfirmEmail", confirmEmail);
            }
            ViewBag.Message = "Please enter a valid email address.";
            return View(confirmEmailViewModel);
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
                var user = _userManager.FindByNameAsync(model.Username).Result;
                if (user != null)
                {
                    if (!_userManager.IsEmailConfirmedAsync(user).Result)
                    {
                        ModelState.AddModelError("", "Email not confirmed");
                        var confirmEmailViewModel = new ConfirmEmailViewModel
                        {
                            Email = user.Email,
                            UserId = user.Id
                        };
                        ViewBag.Message = "You cannot login because your email has not been confirmed.";
                        return View("ConfirmEmail", confirmEmailViewModel);
                    }
                }
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

        private void SendConfirmationEmail(User user)
        {
            var confirmationLink = CreateConfirmationLink(user);

            MimeMessage emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_config["email:address"]));
            emailMessage.To.Add(new MailboxAddress(user.Email));
            emailMessage.Subject = _config["email:subject"];
            emailMessage.Body = new TextPart("plain") { Text = _config["email:body"] + Environment.NewLine + confirmationLink };

            using (SmtpClient client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Authenticate(_config["email:address"], _config["email:password"]);
                client.Send(emailMessage);
                client.Disconnect(true);
            }                        
        }

        private string CreateConfirmationLink(User user)
        {
            var confirmationToken = _userManager.GenerateEmailConfirmationTokenAsync(user).Result;
            return Url.Action("ConfirmEmail",
                        "Account", new
                        {
                            userid = user.Id,
                            token = confirmationToken
                        },
                        protocol: HttpContext.Request.Scheme);
        }
    }
}
