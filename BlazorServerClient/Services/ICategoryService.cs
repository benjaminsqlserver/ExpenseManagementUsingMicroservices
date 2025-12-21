using ExpenseManagement.BlazorUI.Models;

namespace ExpenseManagement.BlazorUI.Services;

public interface ICategoryService
{
    Task<List<CategoryDto>> GetCategoriesAsync();
    Task<List<DepartmentDto>> GetDepartmentsAsync();
}