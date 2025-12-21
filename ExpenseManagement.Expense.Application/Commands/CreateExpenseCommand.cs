using MediatR;
using ExpenseManagement.BuildingBlocks.Common.Models;
using ExpenseManagement.Expense.Application.DTOs;

namespace ExpenseManagement.Expense.Application.Commands;

public record CreateExpenseCommand(
    string Title,
    string Description,
    Guid UserId,
    string UserName,
    Guid? DepartmentId,
    DateTime ExpenseDate,
    List<CreateExpenseItemRequest> Items
) : IRequest<ApiResponse<ExpenseDto>>;