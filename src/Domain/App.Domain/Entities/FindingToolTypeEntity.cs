using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Domain.Entities
{
    [Table("finding_tool_type")]
    public class FindingToolTypeEntity
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
