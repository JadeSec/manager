using System;
using App.Domain.Entities;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using App.Infra.CrossCutting.Helpers;
using System.ComponentModel.DataAnnotations;
using App.Infra.CrossCutting.Validators;

namespace App.Application.Project.Models
{
    public class TeamPayload
    {
        [MinLength(1)]
        [MaxLength(50)]
        public string Name { get; set; }
        
        [Image(width: 100, height: 100, new string[] { "image/jpeg" })]
        public IFormFile Icon { get; set; }

        [MinLength(20)]
        [MaxLength(255)]
        public string Description { get; set; }                     

        public static async Task<ProjectTeamEntity> ToEntityAsync(TeamPayload payload)
        {
            return new ProjectTeamEntity()
            {
                Name = payload.Name,               
                Description = payload.Description,
                Icon = await FormFileHelper.ConvertToBase64Async(payload.Icon),
                Created = DateTime.Now
            };
        }
    }
}
