﻿using System.Threading.Tasks;
using App.Application.Project;
using Microsoft.AspNetCore.Mvc;

namespace App.Mvc.Controllers
{
    public class ProjectController : Controller
    {
        private readonly Team _team;
        private readonly Project _project;

        public ProjectController(Project project, Team team)
        {
            _team = team;
            _project = project;
        }

        [HttpGet]
        public async Task<IActionResult> IndexAsync([FromQuery] string search, [FromQuery] int page = 1)
        {
            return View(await _project.GetAsync(search, page));
        }

        [HttpGet]
        public async Task<IActionResult> TeamAsync([FromQuery] string search, [FromQuery] int page = 1)
        {
            return View(await _team.GetAsync(search, page));
        }
    }
}
