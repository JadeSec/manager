using System.Threading.Tasks;
using App.Application.Project;
using Microsoft.AspNetCore.Mvc;

namespace App.Mvc.Controllers
{
    public class ProjectController : Controller
    {
        private readonly Project _project;

        public ProjectController(Project project)
        {
            _project = project;
        }

        [HttpGet]
        public async Task<IActionResult> IndexAsync([FromQuery] string search, [FromQuery] int page = 1)
        {
            return View(await _project.GetAsync(search, page));
        }
    }
}
