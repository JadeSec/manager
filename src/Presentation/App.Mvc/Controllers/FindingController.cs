using System.Threading.Tasks;
using App.Application.Finding;
using Microsoft.AspNetCore.Mvc;

namespace App.Mvc.Controllers
{
    public class FindingController : Controller
    {
        private readonly Finding _finding;

        public FindingController(Finding finding)
        {
            _finding = finding;
        }

        [HttpGet]
        public async Task<IActionResult> IndexAsync([FromQuery] string search, [FromQuery] int page = 1)
        {
            return View(await _finding.GetAsync(search, page));
        }
    }
}
