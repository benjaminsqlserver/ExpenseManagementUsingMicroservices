// Services/Expense/Expense.Application/Queries/GetExpenseByIdQueryHandler.cs
using MediatR;
using Microsoft.EntityFrameworkCore;
using ExpenseManagement.Expense.Infrastructure.Data;
using ExpenseManagement.BuildingBlocks.Common.Models;
using ExpenseManagement.Expense.Application.DTOs;

namespace ExpenseManagement.Expense.Application.Queries;

public class GetExpenseByIdQueryHandler
    : IRequestHandler<GetExpenseByIdQuery, ApiResponse<ExpenseDto>>
{
    private readonly ExpenseDbContext _context;

    public GetExpenseByIdQueryHandler(ExpenseDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<ExpenseDto>> Handle(
        GetExpenseByIdQuery request,
        CancellationToken cancellationToken)
    {
        var expense = await _context.Expenses
            .Include(e => e.Items)
                .ThenInclude(i => i.Category)
            .Include(e => e.Receipts)
            .Include(e => e.Department)
            .Where(e => e.Id == request.ExpenseId && e.UserId == request.UserId)
            .Select(e => new ExpenseDto(
                e.Id,
                e.Title,
                e.Description,
                e.UserId,
                e.UserName,
                e.DepartmentId,
                e.Department != null ? e.Department.Name : null,
                e.Status,
                e.TotalAmount,
                e.Currency,
                e.ExpenseDate,
                e.SubmittedAt,
                e.CreatedAt,
                e.Items.Select(i => new ExpenseItemDto(
                    i.Id,
                    i.CategoryId,
                    i.Category.Name,
                    i.Description,
                    i.Amount,
                    i.Quantity,
                    i.UnitPrice,
                    i.ItemDate,
                    i.Merchant
                )).ToList(),
                e.Receipts.Select(r => new ReceiptDto(
                    r.Id,
                    r.FileName,
                    r.ContentType,
                    r.FileSize,
                    r.FileUrl
                )).ToList()
            ))
            .FirstOrDefaultAsync(cancellationToken);

        if (expense == null)
            return ApiResponse<ExpenseDto>.FailureResponse("Expense not found");

        return ApiResponse<ExpenseDto>.SuccessResponse(expense);
    }
}
