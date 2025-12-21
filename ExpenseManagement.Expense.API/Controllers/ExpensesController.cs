using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using System.Security.Claims;
using ExpenseManagement.Expense.Application.Commands;
using ExpenseManagement.Expense.Application.Queries;
using ExpenseManagement.Expense.Application.DTOs;
using ExpenseManagement.BuildingBlocks.Common.Enums;

namespace ExpenseManagement.Expense.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ExpensesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<ExpensesController> _logger;

    public ExpensesController(IMediator mediator, ILogger<ExpensesController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    private Guid GetUserId() => Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new UnauthorizedAccessException());
    private string GetUserName() => User.FindFirst(ClaimTypes.Name)?.Value ?? "Unknown";
    private bool IsAdmin() => User.IsInRole("Admin") || User.IsInRole("Finance");

    [HttpGet]
    public async Task<IActionResult> GetExpenses(
        [FromQuery] ExpenseStatus? status,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var query = new GetExpensesQuery(GetUserId(), IsAdmin(), status, pageNumber, pageSize);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetExpense(Guid id)
    {
        var query = new GetExpenseByIdQuery(id, GetUserId());
        var result = await _mediator.Send(query);

        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateExpense([FromBody] CreateExpenseRequest request)
    {
        var command = new CreateExpenseCommand(
            request.Title,
            request.Description,
            GetUserId(),
            GetUserName(),
            request.DepartmentId,
            request.ExpenseDate,
            request.Items
        );

        var result = await _mediator.Send(command);

        if (!result.Success)
            return BadRequest(result);

        return CreatedAtAction(nameof(GetExpense), new { id = result.Data!.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateExpense(Guid id, [FromBody] UpdateExpenseRequest request)
    {
        // TODO: Implement UpdateExpenseCommand
        return Ok();
    }

    [HttpPost("{id}/submit")]
    public async Task<IActionResult> SubmitExpense(Guid id)
    {
        var command = new SubmitExpenseCommand(id, GetUserId());
        var result = await _mediator.Send(command);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteExpense(Guid id)
    {
        // TODO: Implement DeleteExpenseCommand
        return Ok();
    }
}