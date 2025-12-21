using ExpenseManagement.BuildingBlocks.Common.Models;


namespace ExpenseManagement.Expense.Domain.Entities
{
    public class ExpenseItem : BaseEntity
    {
        public Guid ExpenseId { get; set; }
        public Expense Expense { get; set; } = null!;
        public Guid CategoryId { get; set; }
        public Category Category { get; set; } = null!;
        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public int Quantity { get; set; } = 1;
        public decimal UnitPrice { get; set; }
        public DateTime ItemDate { get; set; }
        public string? Merchant { get; set; }
        public string? Notes { get; set; }
    }

}
