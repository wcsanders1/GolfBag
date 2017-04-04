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
            var scores = _roundOfGolf.GetAllRounds(GetCurrentUserAsync().Result.Id)
                                        .SelectMany(x => x.Scores);
               
            var holeScores = new List<int>();

            foreach (var score in scores)
            {
                holeScores.Add(score.HoleScore);
            }

            return Json(holeScores);
        }

        private async Task<User> GetCurrentUserAsync()
        {
            return await _userManager.GetUserAsync(User);
        }
    }
}
