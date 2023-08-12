using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Domain.Entities
{
    [Table("project_group_projects")]
    public class ProjectGroupProjectEntity
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("ProjectGroup")]
        [Column("project_groups_id")]
        public int GroupId { get; set; }
        public ProjectGroupEntity Group { get; set; }

        [ForeignKey("Project")]
        [Column("projects_id")]
        public int ProjectId { get; set; }
        public ProjectEntity Project { get; set; }
    }
}
