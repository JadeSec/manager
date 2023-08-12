using System.Threading.Tasks;
using App.Application.Project;
using Microsoft.AspNetCore.Mvc;

namespace App.Mvc.Controllers
{
    public class TeamController : Controller
    {
        private readonly Team _team;

        public TeamController(Team team)
        {
            _team = team;
        }

        [HttpGet]
        public async Task<IActionResult> IndexAsync([FromQuery] string search, [FromQuery] int page = 1)
        {
            return View(await _team.GetAsync(search, page));
        }
    }
}
