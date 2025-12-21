using ExpenseManagement.BuildingBlocks.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseManagement.Expense.Domain.Entities
{
    public class Receipt : BaseEntity
    {
        public Guid ExpenseId { get; set; }
        public Expense Expense { get; set; } = null!;
        public string FileName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public long FileSize { get; set; }
        public string? FileUrl { get; set; }
    }
}
