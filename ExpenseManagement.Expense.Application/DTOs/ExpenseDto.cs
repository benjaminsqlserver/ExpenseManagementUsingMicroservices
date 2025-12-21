using ExpenseManagement.BuildingBlocks.Common.Enums;

namespace ExpenseManagement.Expense.Application.DTOs;

public record ExpenseDto(
    Guid Id,
    string Title,
    string Description,
    Guid UserId,
    string UserName,
    Guid? DepartmentId,
    string? DepartmentName,
    ExpenseStatus Status,
    decimal TotalAmount,
    string Currency,
    DateTime ExpenseDate,
    DateTime? SubmittedAt,
    DateTime CreatedAt,
    List<ExpenseItemDto> Items,
    List<ReceiptDto> Receipts
);

public record ExpenseItemDto(
    Guid Id,
    Guid CategoryId,
    string CategoryName,
    string Description,
    decimal Amount,
    int Quantity,
    decimal UnitPrice,
    DateTime ItemDate,
    string? Merchant
);

public record ReceiptDto(
    Guid Id,
    string FileName,
    string ContentType,
    long FileSize,
    string? FileUrl
);

public record CreateExpenseRequest(
    string Title,
    string Description,
    Guid? DepartmentId,
    DateTime ExpenseDate,
    List<CreateExpenseItemRequest> Items
);

public record CreateExpenseItemRequest(
    Guid CategoryId,
    string Description,
    decimal Amount,
    int Quantity,
    DateTime ItemDate,
    string? Merchant
);

public record UpdateExpenseRequest(
    string Title,
    string Description,
    Guid? DepartmentId,
    DateTime ExpenseDate,
    List<CreateExpenseItemRequest> Items
);