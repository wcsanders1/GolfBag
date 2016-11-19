using GolfBag.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolfBag.ViewModels
{
    public class HomePageViewModel
    {
        public IEnumerable<ScoreCard> ScoreCards { get; set; }
        public string CurrentGreeting { get; set; }
    }
}
