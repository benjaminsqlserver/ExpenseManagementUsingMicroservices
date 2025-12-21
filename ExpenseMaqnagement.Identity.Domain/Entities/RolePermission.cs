using ExpenseManagement.BuildingBlocks.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseMaqnagement.Identity.Domain.Entities
{
    public class RolePermission : BaseEntity
    {
        public Guid RoleId { get; set; }
        public Role Role { get; set; } = null!;
        public Guid PermissionId { get; set; }
        public Permission Permission { get; set; } = null!;
    }
}
