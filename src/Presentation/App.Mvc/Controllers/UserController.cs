using System.Threading.Tasks;
using App.Application.Rbac;
using Microsoft.AspNetCore.Mvc;

namespace App.Mvc.Controllers
{
    public class UserController : Controller
    {
        private readonly User _user;

        public UserController(User user)
        {
            _user = user;
        }

        [HttpGet]
        public async Task<IActionResult> IndexAsync([FromQuery] string search, [FromQuery] int page = 1)
        {
            return View(await _user.GetAsync(search, page));
        }
    }
}
