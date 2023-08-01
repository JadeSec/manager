using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Domain.Entities
{
    [Table("poc")]
    public class PocEntity
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Poc2")]
        [Column("poc2_id")]
        public int Poc2Id { get; set; }
        public object Poc2 { get; set; }

        public string Name { get; set; }
    }
}
