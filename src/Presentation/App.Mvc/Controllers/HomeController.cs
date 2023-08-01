using App.Mvc.ViewComponents.Alert;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace App.Mvc.Controllers
{
    public class HomeController : Controller
    {       
        readonly ILogger<HomeController> _logger;

        readonly AlertViewComponent _alert;

        public HomeController(          
            ILogger<HomeController> logger)
        {
            _logger = logger;

            _alert = AlertViewComponent.Initialize(this);
        }

        [HttpGet]
        public IActionResult Index(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
            }

            return View();
        }
    }
}
