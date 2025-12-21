using ExpenseManagement.BuildingBlocks.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseManagement.Expense.Domain.Entities
{
    public class Department : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Guid? ManagerUserId { get; set; }
        public string? ManagerName { get; set; }
        public bool IsActive { get; set; } = true;
        public decimal? MonthlyBudget { get; set; }

        public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
    }
}
