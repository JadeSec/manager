using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Domain.Entities
{
    [Table("project_group_users")]
    public class ProjectGroupUsersEntity
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("ProjectGroup")]
        [Column("project_groups_id")]
        public int GroupId { get; set; }
        public ProjectGroupEntity Group { get; set; }

        [ForeignKey("User")]
        [Column("users_id")]
        public int UserId { get; set; }
        public UserEntity User { get; set; }

        [ForeignKey("Role")]
        [Column("roles_id")]
        public int RoleId { get; set; }
        public UserEntity Role { get; set; }

        [ForeignKey("RoleResource")]
        [Column("role_resources_id")]
        public int RoleResourceId { get; set; }
        public RoleResourceEntity RoleResource { get; set; }

        [ForeignKey("RolePermission")]
        [Column("role_permissions_id")]
        public int RolePermissionId { get; set; }
        public RolePermissionEntity RolePermission { get; set; }

        public DateTime Created { get; set; }
    }
}
