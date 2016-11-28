using GolfBag.ViewModels;
using GolfBag.Services;
using GolfBag.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace GolfBag.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {

        public HomeController()
        {

        }

        [AllowAnonymous]
        public ViewResult Index()
        {
            return View();
        }
    }
}
