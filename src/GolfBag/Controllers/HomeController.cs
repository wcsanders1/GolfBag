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
        private IScoreCardData _scoreCardData;

        public HomeController(
            IScoreCardData scoreCardData)
        {
            _scoreCardData = scoreCardData;
        }

        [AllowAnonymous]
        public ViewResult Index()
        {
            var model = new HomePageViewModel();
            model.ScoreCards = _scoreCardData.GetAll();
            return View(model);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var model = _scoreCardData.Get(id);
            if (model == null)
            {
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(int id, ScoreCardEditViewModel input)
        {
            var scoreCard = _scoreCardData.Get(id);
            if (scoreCard != null && ModelState.IsValid)
            {
                scoreCard.PlayerName = input.PlayerName;
                scoreCard.CourseName = input.CourseName;
                scoreCard.Course = input.Course;

                _scoreCardData.Commit();

                return RedirectToAction("Details", new { id = scoreCard.Id });
            }
            return View(scoreCard);
        }

        [HttpGet]
        public IActionResult Create()
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
                _scoreCardData.Commit();

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
