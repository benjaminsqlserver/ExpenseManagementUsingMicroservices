using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseManagement.Identity.Application.DTOs
{
    public record LoginRequest(string Email, string Password);

    public record LoginResponse(
        string AccessToken,
        string RefreshToken,
        DateTime ExpiresAt,
        UserDto User
    );

    public record RefreshTokenRequest(string RefreshToken);

    public record UserDto(
        Guid Id,
        string Email,
        string Username,
        string FirstName,
        string LastName,
        string FullName,
        string? Department,
        List<string> Roles,
        List<string> Permissions
    );
}
