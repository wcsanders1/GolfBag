using GolfBag.Entities;
using GolfBag.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolfBag.Controllers
{
    public class StatisticsController : Controller
    {
        private IRoundOfGolf _roundOfGolf;
        private UserManager<User> _userManager;

        public StatisticsController(IRoundOfGolf roundOfGolf, UserManager<User> userManager)
        {
            _roundOfGolf = roundOfGolf;
            _userManager = userManager;
        }

        public JsonResult GetScores()
        {
            var rounds = _roundOfGolf.GetAllRounds(GetCurrentUserAsync().Result.Id);
               
            var scores = new List<int>();

            foreach (var round in rounds)
            {
                var score = round.Scores.Sum(x => x.HoleScore);
                scores.Add(score);
            }

            return Json(scores);
        }

        private async Task<User> GetCurrentUserAsync()
        {
            return await _userManager.GetUserAsync(User);
        }
    }
}
