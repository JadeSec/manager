using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Domain.Entities
{
    [Table("finding_attachments")]
    public class FindingAttachmentEntity
    {
        [Key]
        public long Id { get; set; }

        [ForeignKey("Finding")]
        [Column("finding_id")]
        public long FindingId { get; set; }
        public FindingEntity Finding { get; set; }

        public string Name { get; set; }

        public string Format { get; set; }

        public string Uuid { get; set; }

        public DateTime Created { get; set; }
    }
}
