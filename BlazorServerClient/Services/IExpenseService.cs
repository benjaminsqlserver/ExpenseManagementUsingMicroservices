using ExpenseManagement.BlazorUI.Models;


namespace ExpenseManagement.BlazorUI.Services;

public interface IExpenseService
{
    Task<PagedResult<ExpenseDto>> GetExpensesAsync(int pageNumber = 1, int pageSize = 10, ExpenseStatus? status = null);
    Task<ExpenseDto?> GetExpenseByIdAsync(Guid id);
    Task<ApiResponse<ExpenseDto>> CreateExpenseAsync(CreateExpenseRequest request);
    Task<ApiResponse<bool>> SubmitExpenseAsync(Guid id);
    Task<ApiResponse<bool>> DeleteExpenseAsync(Guid id);
}