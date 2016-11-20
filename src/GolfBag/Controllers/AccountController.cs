using GolfBag.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolfBag.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost, AutoValidateAntiforgeryToken]
        public IActionResult Register(RegisterUserViewModel model)
        {
            return View();
        }
    }
}
