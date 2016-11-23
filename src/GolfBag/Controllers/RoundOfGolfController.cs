using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GolfBag.ViewModels;
using GolfBag.Entities;
using GolfBag.Services;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace GolfBag.Controllers
{
    public class RoundOfGolfController : Controller
    {
        private IRoundOfGolf _roundOfGolf;

        public RoundOfGolfController(IRoundOfGolf roundOfGolf)
        {
            _roundOfGolf = roundOfGolf;
        }
        public IActionResult EnterScore(RoundOfGolfViewModel model)
        {
            if (ModelState.IsValid)
            {
                var roundOfGolf = new RoundOfGolf();
                List<Score> scores = new List<Score>();

                roundOfGolf.PlayerName = User.Identity.Name;

                for (int i = 0; i < model.Scores.Count; i++)
                {
                    Score score = new Score();
                    score.HoleScore = model.Scores[i];
                    scores.Add(score);
                }

                roundOfGolf.Scores = scores;

                _roundOfGolf.Add(roundOfGolf);
                return RedirectToAction("Index", "Home", "");
            }
            return View();
        }
    }
}
