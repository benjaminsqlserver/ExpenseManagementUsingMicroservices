using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ExpenseManagement.Expense.Infrastructure.Data;
using ExpenseManagement.BuildingBlocks.Common.Models;

namespace ExpenseManagement.Expense.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CategoriesController : ControllerBase
{
    private readonly ExpenseDbContext _context;

    public CategoriesController(ExpenseDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetCategories()
    {
        var categories = await _context.Categories
            .Where(c => c.IsActive)
            .Select(c => new
            {
                c.Id,
                c.Name,
                c.Code,
                c.Description,
                c.RequiresReceipt,
                c.MaxAmountPerExpense
            })
            .ToListAsync();

        return Ok(ApiResponse<object>.SuccessResponse(categories));
    }
}