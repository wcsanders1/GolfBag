using GolfBag.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolfBag.ViewModels
{
    public class ViewRoundsViewModel
    {
        public int SelectedRound { get; set; }

        public List<ViewRound> ViewRounds { get; set; }

        public ViewRoundsViewModel()
        {
            ViewRounds = new List<ViewRound>();
        }

        public void MapViewRounds(RoundOfGolf round, Course course)
        {
            var viewRound = new ViewRound();

            viewRound.CourseName = course.CourseName;
            viewRound.RoundId = round.Id;
            viewRound.RoundDate = round.Date;
            ViewRounds.Add(viewRound);         
        }
    }
}
