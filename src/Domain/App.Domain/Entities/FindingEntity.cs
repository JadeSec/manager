using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Domain.Entities
{
    [Table("findings")]
    public class FindingEntity
    {
        [Key]
        public long Id { get; set; }

        [ForeignKey("Project")]
        [Column("project_id")]
        public int ProjectId { get; set; }
        public ProjectEntity Project { get; set; }

        [ForeignKey("Status")]
        [Column("finding_status_id")]
        public int FindingStatusId { get; set; }        
        public FindingStatusEntity Status { get; set; }

        [ForeignKey("Severity")]
        [Column("finding_severity_id")]
        public int FindingSeverityId { get; set; }        
        public FindingSeverityEntity Severity { get; set; }

        [ForeignKey("AnalyzeTest")]
        [Column("analyze_test_id")]
        public long? AnalyzeTestId { get; set; }
        public AnalyzeTestEntity AnalyzeTest { get; set; }

        [ForeignKey("Tool")]
        [Column("finding_tool_id")]
        public int? FindingToolId { get; set; }
        public FindingToolEntity Tool { get; set; }

        public string Title { get; set; }

        public string Cwe { get; set; }

        public string Cve { get; set; }

        public string Description { get; set; }

        public string Mitigation { get; set; }

        public string References { get; set; }

        public int? Line { get; set; }

        [Column("file_path")]
        public string FilePath { get; set; }

        [Column("code_snippet")]
        public string CodeSnippet { get; set; }

        public string Sha1 { get; set; }

        public DateTime? Modified { get; set; }

        public DateTime Created { get; set; }
    }
}