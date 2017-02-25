using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolfBag.ViewModels
{
    public class ConfirmEmailViewModel
    {
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string UserId { get; set; }
    }
}
