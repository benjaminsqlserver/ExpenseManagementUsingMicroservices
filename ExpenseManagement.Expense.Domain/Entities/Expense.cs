using ExpenseManagement.BuildingBlocks.Common.Enums;
using ExpenseManagement.BuildingBlocks.Common.Models;


namespace ExpenseManagement.Expense.Domain.Entities
{
    public class Expense : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public Guid? DepartmentId { get; set; }
        public Department? Department { get; set; }
        public ExpenseStatus Status { get; set; } = ExpenseStatus.Draft;
        public decimal TotalAmount { get; set; }
        public string Currency { get; set; } = "USD";
        public DateTime ExpenseDate { get; set; }
        public DateTime? SubmittedAt { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public DateTime? RejectedAt { get; set; }
        public string? RejectionReason { get; set; }
        public string? ApproverComments { get; set; }

        public ICollection<ExpenseItem> Items { get; set; } = new List<ExpenseItem>();
        public ICollection<Receipt> Receipts { get; set; } = new List<Receipt>();

        public void Submit()
        {
            if (Status != ExpenseStatus.Draft)
                throw new InvalidOperationException("Only draft expenses can be submitted");

            if (!Items.Any())
                throw new InvalidOperationException("Expense must have at least one item");

            Status = ExpenseStatus.Submitted;
            SubmittedAt = DateTime.UtcNow;
        }

        public void Approve(string comments)
        {
            if (Status != ExpenseStatus.PendingApproval)
                throw new InvalidOperationException("Only pending expenses can be approved");

            Status = ExpenseStatus.Approved;
            ApprovedAt = DateTime.UtcNow;
            ApproverComments = comments;
        }

        public void Reject(string reason)
        {
            if (Status != ExpenseStatus.PendingApproval)
                throw new InvalidOperationException("Only pending expenses can be rejected");

            Status = ExpenseStatus.Rejected;
            RejectedAt = DateTime.UtcNow;
            RejectionReason = reason;
        }

        public void CalculateTotalAmount()
        {
            TotalAmount = Items.Sum(i => i.Amount);
        }
    }

}
