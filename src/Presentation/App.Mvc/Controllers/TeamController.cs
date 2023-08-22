using System;
using System.Threading.Tasks;
using App.Application.Project;
using App.Mvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace App.Mvc.Controllers
{
    public class TeamController : Controller
    {
        private readonly Team _team;

        private readonly ILogger<TeamController> _logger;

        public TeamController(Team team, ILogger<TeamController> logger)
        {
            _team = team;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> IndexAsync([FromQuery] string search, [FromQuery] int page = 1)
           => View(await _team.GetAsync(search, page));


        [HttpGet]
        public IActionResult Create() 
            => View();        

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm] TeamFormModel form)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _team.CreateAsync(await form.ToEntityAsync());

                    return RedirectToAction("Index", new { search = $"name:{form.Name.ToLower()}" });
                }
            }
            catch (Exception e)
            {
                _logger.LogError(nameof(CreateAsync), e);
            }

            return View(form);
        }

        //[HttpPost]
        //[ApiResponse]
        //[Route("/api/[controller]/[action]")]
        //public async Task<IActionResult> CreateAsync([FromForm] TeamFormModel form)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            await _team.CreateAsync(await form.ToEntityAsync());

        //            return Ok();
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        _logger.LogError(nameof(CreateAsync), e);

        //        return BadRequest("The error is very, not create a tema in moment, try again later.");
        //    }
        //    return BadRequest(ModelState);
        //}
    }
}