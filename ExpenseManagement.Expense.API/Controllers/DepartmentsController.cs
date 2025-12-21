using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ExpenseManagement.Expense.Infrastructure.Data;
using ExpenseManagement.BuildingBlocks.Common.Models;

namespace ExpenseManagement.Expense.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DepartmentsController : ControllerBase
{
    private readonly ExpenseDbContext _context;

    public DepartmentsController(ExpenseDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetDepartments()
    {
        var departments = await _context.Departments
            .Where(d => d.IsActive)
            .Select(d => new
            {
                d.Id,
                d.Name,
                d.Code,
                d.Description
            })
            .ToListAsync();

        return Ok(ApiResponse<object>.SuccessResponse(departments));
    }
}