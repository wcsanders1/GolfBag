using GolfBag.ViewModels;
using GolfBag.Services;
using GolfBag.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolfBag.Controllers
{
    public class HomeController : Controller
    {
        private IGreeter _greeter;
        private IScoreCardData _scoreCardData;

        public HomeController(
            IScoreCardData scoreCardData,
            IGreeter greeter)
        {
            _scoreCardData = scoreCardData;
            _greeter = greeter;
        }
        public ViewResult Index()
        {
            var model = new HomePageViewModel();
            model.ScoreCards = _scoreCardData.GetAll();
            model.CurrentGreeting = _greeter.GetGreeting();
            return View(model);
        }

        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(ScoreCardEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var scoreCard = new ScoreCard();
                scoreCard.PlayerName = model.PlayerName;
                scoreCard.CourseName = model.CourseName;
                scoreCard.Course = model.Course;

                _scoreCardData.Add(scoreCard);

                return RedirectToAction("Details", new { id = scoreCard.Id });
            }
            return View();
        }

        public IActionResult Details(int id)
        {
            var model = _scoreCardData.Get(id);
            if (model == null)
            {
                return RedirectToAction("Index");
            }
            return View(model);
        }
    }
}
