using System;
using System.Threading.Tasks;
using App.Application.Project;
using App.Domain.Exceptions;
using App.Mvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace App.Mvc.Controllers
{
    [Route("[controller]/[action]")]
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

        [HttpGet("{id}")]        
        public async Task<IActionResult> UpdateAsync(int id)
        {
            try
            {
                var entity = await _team.GetAsync(id) ?? throw new ControllerException("Team not found.");      

                return View(new TeamFormModel(entity));
            }
            catch (Exception e)
            {
                _logger.LogError(nameof(UpdateAsync), e);
            }

            return RedirectToAction("Index");
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromForm] TeamFormModel form)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var entity = await _team.GetAsync(id) ?? throw new ControllerException("Team not found.");

                    await _team.UpdateAsync(await form.ToEntityAsync(entity));

                    return RedirectToAction("Index");
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