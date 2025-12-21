using MediatR;
using ExpenseManagement.Identity.Application.DTOs;
using ExpenseManagement.BuildingBlocks.Common.Models;

namespace ExpenseManagement.Identity.Application.Commands;

public record LoginCommand(string Email, string Password) : IRequest<ApiResponse<LoginResponse>>;