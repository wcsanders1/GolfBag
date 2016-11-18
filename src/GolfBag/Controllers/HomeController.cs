using GolfBag.Models;
using GolfBag.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolfBag.Controllers
{
    public class HomeController : Controller
    {
        private IScoreCardData _scoreCardData;

        public HomeController(IScoreCardData scoreCardData)
        {
            _scoreCardData = scoreCardData;
        }
        public ViewResult Index()
        {
            var model = _scoreCardData.GetAll();
            return View(model);
        }
    }
}
