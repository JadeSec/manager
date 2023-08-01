using App.Application.Poc;
using App.Mvc.ViewComponents.Alert;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace App.Mvc.Controllers
{
    public class HomeController : Controller
    {
        readonly Poc _poc;

        readonly ILogger<HomeController> _logger;

        readonly AlertViewComponent _alert;

        public HomeController(         
            Poc poc,
            ILogger<HomeController> logger)
        {
            _poc = poc;

            _logger = logger;

            _alert = AlertViewComponent.Initialize(this);
        }

        [HttpGet]
        public IActionResult Index(string returnUrl)
        {
            _poc.RedisCheck();

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
