using ExpenseManagement.BuildingBlocks.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseMaqnagement.Identity.Domain.Entities
{
    public class UserClaim : BaseEntity
    {
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public string ClaimType { get; set; } = string.Empty;
        public string ClaimValue { get; set; } = string.Empty;
    }
}
