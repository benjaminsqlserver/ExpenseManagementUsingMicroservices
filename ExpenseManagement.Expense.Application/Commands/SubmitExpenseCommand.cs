using MediatR;
using ExpenseManagement.BuildingBlocks.Common.Models;

namespace ExpenseManagement.Expense.Application.Commands;

public record SubmitExpenseCommand(Guid ExpenseId, Guid UserId) : IRequest<ApiResponse<bool>>;