using MediatR;
using ExpenseManagement.BuildingBlocks.Common.Models;
using ExpenseManagement.Expense.Application.DTOs;
using ExpenseManagement.BuildingBlocks.Common.Enums;

namespace ExpenseManagement.Expense.Application.Queries;

public record GetExpensesQuery(
    Guid UserId,
    bool IsAdmin,
    ExpenseStatus? Status,
    int PageNumber,
    int PageSize
) : IRequest<ApiResponse<PagedResult<ExpenseDto>>>;