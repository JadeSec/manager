using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Domain.Entities
{
    [Table("analyze_test")]
    public class AnalyzeTestEntity
    {
        [Key]
        public long Id { get; set; }

        [ForeignKey("Analyze")]
        [Column("analyzes_id")]
        public long AnalyzeId { get; set; }
        public AnalyzeEntity Analyze { get; set; }

        public string Title { get; set; }

        public DateTime Created { get; set; }
    }
}
