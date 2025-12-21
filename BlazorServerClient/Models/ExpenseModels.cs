
namespace ExpenseManagement.BlazorUI.Models;

public enum ExpenseStatus
{
    Draft = 0,
    Submitted = 1,
    PendingApproval = 2,
    Approved = 3,
    Rejected = 4,
    PendingReimbursement = 5,
    Reimbursed = 6,
    Cancelled = 7
}

public class ExpenseDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public Guid? DepartmentId { get; set; }
    public string? DepartmentName { get; set; }
    public ExpenseStatus Status { get; set; }
    public decimal TotalAmount { get; set; }
    public string Currency { get; set; } = "USD";
    public DateTime ExpenseDate { get; set; }
    public DateTime? SubmittedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<ExpenseItemDto> Items { get; set; } = new();
    public List<ReceiptDto> Receipts { get; set; } = new();
}

public class ExpenseItemDto
{
    public Guid Id { get; set; }
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public DateTime ItemDate { get; set; }
    public string? Merchant { get; set; }
}

public class ReceiptDto
{
    public Guid Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public string? FileUrl { get; set; }
}

public class CreateExpenseRequest
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid? DepartmentId { get; set; }
    public DateTime ExpenseDate { get; set; } = DateTime.Now;
    public List<CreateExpenseItemRequest> Items { get; set; } = new();
}

public class CreateExpenseItemRequest
{
    public Guid CategoryId { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public int Quantity { get; set; } = 1;
    public DateTime ItemDate { get; set; } = DateTime.Now;
    public string? Merchant { get; set; }
}

public class CategoryDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool RequiresReceipt { get; set; }
    public decimal? MaxAmountPerExpense { get; set; }
}

public class DepartmentDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

public class PagedResult<T>
{
    public List<T> Items { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;
}