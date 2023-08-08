using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Domain.Entities
{
    [Table("user_permission")]
    public class UserPermissionEntity
    {
        [Key]
        [ForeignKey("User")]
        [Column("user_id")]
        public int UserId { get; set; }
        public UserEntity User { get; set; }
        
        [ForeignKey("Role")]
        [Column("role_id")]
        public int RoleId { get; set; }
        public RoleEntity Role { get; set; }

        [ForeignKey("RoleResource")]
        [Column("role_resource_id")]
        public int RoleResourceId { get; set; }
        public RoleResourceEntity RoleResource { get; set; }

        [ForeignKey("RolePermission")]
        [Column("role_permission_id")]
        public int RolePermissionId { get; set; }
        public RoleResourceEntity RolePermission { get; set; }
    }
}