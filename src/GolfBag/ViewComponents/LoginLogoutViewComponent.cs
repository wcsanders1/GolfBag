using GolfBag.Entities;
using GolfBag.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolfBag.ViewComponents
{
    public class LoginLogoutViewComponent : ViewComponent
    {
        private UserManager<User> _userManager;
        public LoginLogoutViewComponent(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        public IViewComponentResult Invoke()
        {
            if (User.Identity.IsAuthenticated)
            {
                var loginLogoutViewComponentViewModel = new LoginLogoutViewComponentViewModel();
                var currentUser = GetCurrentUserAsync().Result;

                loginLogoutViewComponentViewModel.FirstName = currentUser.FirstName;
                loginLogoutViewComponentViewModel.LastName = currentUser.LastName;
                return View(loginLogoutViewComponentViewModel);
            }
            return View();
        }

        private async Task<User> GetCurrentUserAsync()
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            return currentUser;
        }
    }
}
