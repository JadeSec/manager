using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Domain.Entities
{
    [Table("finding_tool")]
    public class FindingToolEntity
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Type")]
        [Column("finding_tool_type_id")]
        public int TypeId { get; set; }
        public FindingToolTypeEntity Type { get; set; }

        public string Name { get; set; }
    }
}
