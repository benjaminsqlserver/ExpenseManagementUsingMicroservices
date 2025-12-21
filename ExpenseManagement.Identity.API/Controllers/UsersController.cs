using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ExpenseManagement.Identity.Infrastructure.Data;
using ExpenseManagement.BuildingBlocks.Common.Models;
using ExpenseManagement.Identity.Application.Services;

namespace ExpenseManagement.Identity.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IdentityDbContext _context;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ILogger<UsersController> _logger;

    public UsersController(
        IdentityDbContext context,
        IPasswordHasher passwordHasher,
        ILogger<UsersController> logger)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _logger = logger;
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _context.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .Select(u => new
            {
                u.Id,
                u.Email,
                u.Username,
                u.FirstName,
                u.LastName,
                u.Department,
                u.IsActive,
                Roles = u.UserRoles.Select(ur => ur.Role.Name).ToList()
            })
            .ToListAsync();

        return Ok(ApiResponse<object>.SuccessResponse(users));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(Guid id)
    {
        var user = await _context.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Id == id);

        if (user == null)
            return NotFound(ApiResponse<object>.FailureResponse("User not found"));

        var userData = new
        {
            user.Id,
            user.Email,
            user.Username,
            user.FirstName,
            user.LastName,
            user.Department,
            user.IsActive,
            Roles = user.UserRoles.Select(ur => ur.Role.Name).ToList()
        };

        return Ok(ApiResponse<object>.SuccessResponse(userData));
    }
}