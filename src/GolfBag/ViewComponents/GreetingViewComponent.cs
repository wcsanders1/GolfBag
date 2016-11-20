using GolfBag.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolfBag.ViewComponents
{
    public class GreetingViewComponent : ViewComponent
    {
        private IGreeter _greeter;

        public GreetingViewComponent(IGreeter greeter)
        {
            _greeter = greeter;
        }

        public IViewComponentResult Invoke()
        {
            var model = _greeter.GetGreeting();

            return View("Default", model);  //when the model is a simple string, must specify the view name or else C# will think the model is the view
        }
    }
}
