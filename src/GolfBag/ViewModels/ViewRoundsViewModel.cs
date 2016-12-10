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
    }

    public class ViewRound
    {
        public string CourseName { get; set; }

        public int RoundId { get; set; }

        public DateTime RoundDate { get; set; }
    }
}
