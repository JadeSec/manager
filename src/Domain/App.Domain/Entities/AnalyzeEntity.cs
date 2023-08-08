using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Domain.Entities
{
    [Table("analyzes")]
    public class AnalyzeEntity
    {
        [Key]
        public long Id { get; set; }

        [ForeignKey("Status")]
        [Column("analyze_status")]
        public int AnalyzeStatusId { get; set; }
        public AnalyzeStatusEntity Status { get; set; }

        public string Title { get; set; }

        public DateTime Created { get; set; }
    }
}
