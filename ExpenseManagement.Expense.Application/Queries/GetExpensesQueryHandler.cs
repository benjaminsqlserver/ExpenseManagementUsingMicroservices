// Services/Expense/Expense.Application/Queries/GetExpensesQueryHandler.cs
using MediatR;
using Microsoft.EntityFrameworkCore;
using ExpenseManagement.Expense.Infrastructure.Data;
using ExpenseManagement.BuildingBlocks.Common.Models;
using ExpenseManagement.Expense.Application.DTOs;

namespace ExpenseManagement.Expense.Application.Queries;

public class GetExpensesQueryHandler
    : IRequestHandler<GetExpensesQuery, ApiResponse<PagedResult<ExpenseDto>>>
{
    private readonly ExpenseDbContext _context;

    public GetExpensesQueryHandler(ExpenseDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<PagedResult<ExpenseDto>>> Handle(
        GetExpensesQuery request,
        CancellationToken cancellationToken)
    {
        var query = _context.Expenses
            .Include(e => e.Department)
            .AsQueryable();

        // Non-admins only see their own expenses
        if (!request.IsAdmin)
        {
            query = query.Where(e => e.UserId == request.UserId);
        }

        if (request.Status.HasValue)
        {
            query = query.Where(e => e.Status == request.Status);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var expenses = await query
            .OrderByDescending(e => e.CreatedAt)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
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
                new List<ExpenseItemDto>(), // lightweight list view
                new List<ReceiptDto>()
            ))
            .ToListAsync(cancellationToken);

        var pagedResult = new PagedResult<ExpenseDto>
        {
            Items = expenses,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };


        return ApiResponse<PagedResult<ExpenseDto>>.SuccessResponse(pagedResult);
    }
}
