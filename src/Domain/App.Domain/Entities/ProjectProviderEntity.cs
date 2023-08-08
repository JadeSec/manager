using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Domain.Entities
{
    [Table("project_provider")]
    public class ProjectProviderEntity
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public DateTime Created { get; set; }
    }
}
