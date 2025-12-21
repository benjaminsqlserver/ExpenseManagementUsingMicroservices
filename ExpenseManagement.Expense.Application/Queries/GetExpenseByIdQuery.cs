using MediatR;
using ExpenseManagement.BuildingBlocks.Common.Models;
using ExpenseManagement.Expense.Application.DTOs;

namespace ExpenseManagement.Expense.Application.Queries;

public record GetExpenseByIdQuery(Guid ExpenseId, Guid UserId) : IRequest<ApiResponse<ExpenseDto>>;