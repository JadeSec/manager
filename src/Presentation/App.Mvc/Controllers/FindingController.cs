using App.Application.Finding;
using App.Domain.Entities;
using App.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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
        public async Task<IActionResult> IndexAsync([FromQuery]string search)
        {            
            return View(await _finding.GetAsync(search));
        }
    }
}
