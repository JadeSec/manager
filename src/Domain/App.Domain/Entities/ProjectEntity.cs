using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Domain.Entities
{
    [Table("projects")]
    public class ProjectEntity
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Provider")]
        [Column("project_provider_id")]
        public int ProjectProviderId { get; set; }
        public ProjectProviderEntity Provider { get; set; }

        [ForeignKey("Organization")]
        [Column("project_organization_id")]
        public int? ProjectOrganizationId { get; set; }
        public ProjectOrganizationEntity Organization { get; set; }

        [ForeignKey("Team")]
        [Column("project_team_id")]
        public int? ProjectTeamId { get; set; }
        public ProjectTeamEntity Team { get; set; }

        public string Name { get; set; }

        public DateTime Created { get; set; }
    }
}
