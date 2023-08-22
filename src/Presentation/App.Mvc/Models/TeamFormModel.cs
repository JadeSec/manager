using System;
using App.Domain.Entities;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using App.Infra.CrossCutting.Helpers;
using System.ComponentModel.DataAnnotations;
using App.Infra.CrossCutting.Validators;

namespace App.Mvc.Models
{
    public class TeamFormModel
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [Image(width: 100, height: 100, new string[] { "jpeg", "jpg" })]
        public IFormFile Icon { get; set; }

        [Required]        
        [MaxLength(255)]
        public string Description { get; set; }                     

        public async Task<ProjectTeamEntity> ToEntityAsync()
        {
            return new ProjectTeamEntity()
            {
                Name = Name,
                Created = DateTime.Now,
                Description = Description,
                Icon = await FormFileHelper.ConvertToBase64Async(Icon)
            };
        }
    }
}
