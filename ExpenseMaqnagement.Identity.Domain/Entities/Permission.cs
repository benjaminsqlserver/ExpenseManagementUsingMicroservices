using ExpenseManagement.BuildingBlocks.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseMaqnagement.Identity.Domain.Entities
{
    public class Permission : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }
}
