using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Domain.Entities
{
    [Table("finding_severity")]
    public class FindingSeverityEntity
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public int Sla { get; set; }
    }
}
