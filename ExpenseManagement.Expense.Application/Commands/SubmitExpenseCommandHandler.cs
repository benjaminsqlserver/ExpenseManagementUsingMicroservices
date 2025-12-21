using MediatR;
using Microsoft.EntityFrameworkCore;
using ExpenseManagement.Expense.Infrastructure.Data;
using ExpenseManagement.BuildingBlocks.Common.Models;

namespace ExpenseManagement.Expense.Application.Commands;

public class SubmitExpenseCommandHandler : IRequestHandler<SubmitExpenseCommand, ApiResponse<bool>>
{
    private readonly ExpenseDbContext _context;

    public SubmitExpenseCommandHandler(ExpenseDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<bool>> Handle(SubmitExpenseCommand request, CancellationToken cancellationToken)
    {
        var expense = await _context.Expenses
            .Include(e => e.Items)
            .FirstOrDefaultAsync(e => e.Id == request.ExpenseId && e.UserId == request.UserId, cancellationToken);

        if (expense == null)
            return ApiResponse<bool>.FailureResponse("Expense not found or you don't have permission");

        try
        {
            expense.Submit();
            await _context.SaveChangesAsync(cancellationToken);
            return ApiResponse<bool>.SuccessResponse(true, "Expense submitted successfully");
        }
        catch (InvalidOperationException ex)
        {
            return ApiResponse<bool>.FailureResponse(ex.Message);
        }
    }
}