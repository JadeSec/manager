using System.Threading.Tasks;
using App.Application.Project;
using Microsoft.AspNetCore.Mvc;

namespace App.Mvc.Controllers
{
    public class OrganizationController : Controller
    {
        private readonly Organization _org;

        public OrganizationController(Organization organization)
        {
            _org = organization;
        }

        [HttpGet]
        public async Task<IActionResult> IndexAsync([FromQuery] string search, [FromQuery] int page = 1)
        {
            return View(await _org.GetAsync(search, page));
        }
    }
}
