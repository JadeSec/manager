using System.Threading.Tasks;
using App.Application.Project;
using Microsoft.AspNetCore.Mvc;

namespace App.Mvc.Controllers
{
    public class GroupController : Controller
    {
        private readonly Group _group;

        public GroupController(Group group)
        {
            _group = group;
        }

        [HttpGet]
        public async Task<IActionResult> IndexAsync([FromQuery] string search, [FromQuery] int page = 1)
        {
            return View(await _group.GetAsync(search, page));
        }
    }
}
