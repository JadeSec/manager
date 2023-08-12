using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Domain.Entities
{
    [Table("project_groups")]
    public class ProjectGroupEntity
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime Created { get; set; }
    }
}
