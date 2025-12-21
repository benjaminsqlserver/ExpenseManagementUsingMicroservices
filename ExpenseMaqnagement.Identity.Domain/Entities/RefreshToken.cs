using ExpenseManagement.BuildingBlocks.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseMaqnagement.Identity.Domain.Entities
{
    public class RefreshToken : BaseEntity
    {
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public bool IsRevoked { get; set; }
        public string? RevokedByIp { get; set; }
        public DateTime? RevokedAt { get; set; }
        public string CreatedByIp { get; set; } = string.Empty;

        public bool IsActive => !IsRevoked && DateTime.UtcNow < ExpiresAt;
    }
}
