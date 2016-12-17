using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolfBag.ViewModels
{
    public class ViewTeeBox
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<ViewTee> Tees { get; set; }

        public ViewTeeBox()
        {
            Tees = new List<ViewTee>();
        }
    }
}
