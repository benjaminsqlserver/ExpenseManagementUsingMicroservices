using MediatR;
using Microsoft.EntityFrameworkCore;
using ExpenseManagement.Expense.Infrastructure.Data;
using ExpenseManagement.Expense.Application.DTOs;
using ExpenseManagement.BuildingBlocks.Common.Models;
using ExpenseManagement.BuildingBlocks.Common.Enums;

namespace ExpenseManagement.Expense.Application.Commands;

public class CreateExpenseCommandHandler : IRequestHandler<CreateExpenseCommand, ApiResponse<ExpenseDto>>
{
    private readonly ExpenseDbContext _context;

    public CreateExpenseCommandHandler(ExpenseDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<ExpenseDto>> Handle(CreateExpenseCommand request, CancellationToken cancellationToken)
    {
        if (!request.Items.Any())
            return ApiResponse<ExpenseDto>.FailureResponse("Expense must have at least one item");

        var expense = new Domain.Entities.Expense
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            UserId = request.UserId,
            UserName = request.UserName,
            DepartmentId = request.DepartmentId,
            ExpenseDate = request.ExpenseDate,
            Status = ExpenseStatus.Draft,
            Currency = "USD",
            CreatedBy = request.UserId.ToString()
        };

        foreach (var itemRequest in request.Items)
        {
            var item = new Domain.Entities.ExpenseItem
            {
                Id = Guid.NewGuid(),
                ExpenseId = expense.Id,
                CategoryId = itemRequest.CategoryId,
                Description = itemRequest.Description,
                Amount = itemRequest.Amount,
                Quantity = itemRequest.Quantity,
                UnitPrice = itemRequest.Amount / itemRequest.Quantity,
                ItemDate = itemRequest.ItemDate,
                Merchant = itemRequest.Merchant,
                CreatedBy = request.UserId.ToString()
            };
            expense.Items.Add(item);
        }

        expense.CalculateTotalAmount();

        _context.Expenses.Add(expense);
        await _context.SaveChangesAsync(cancellationToken);

        var result = await MapToDto(expense.Id);
        return ApiResponse<ExpenseDto>.SuccessResponse(result!, "Expense created successfully");
    }

    private async Task<ExpenseDto?> MapToDto(Guid expenseId)
    {
        return await _context.Expenses
            .Include(e => e.Items)
                .ThenInclude(i => i.Category)
            .Include(e => e.Receipts)
            .Include(e => e.Department)
            .Where(e => e.Id == expenseId)
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
            .FirstOrDefaultAsync();
    }
}