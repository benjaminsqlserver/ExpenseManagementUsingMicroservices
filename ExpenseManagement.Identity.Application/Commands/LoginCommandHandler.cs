using ExpenseManagement.BuildingBlocks.Common.Models;
using ExpenseManagement.Identity.Application.Common.Security;
using ExpenseManagement.Identity.Application.DTOs;
using ExpenseManagement.Identity.Application.Services;
using ExpenseManagement.Identity.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExpenseManagement.Identity.Application.Commands;

public class LoginCommandHandler : IRequestHandler<LoginCommand, ApiResponse<LoginResponse>>
{
    private readonly IdentityDbContext _context;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public LoginCommandHandler(
        IdentityDbContext context,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<ApiResponse<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                    .ThenInclude(r => r.RolePermissions)
                        .ThenInclude(rp => rp.Permission)
            .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

        if (user == null)
            return ApiResponse<LoginResponse>.FailureResponse("Invalid email or password");

        if (!user.IsActive)
            return ApiResponse<LoginResponse>.FailureResponse("Account is inactive");

        if (user.IsLockedOut)
            return ApiResponse<LoginResponse>.FailureResponse("Account is locked");

        if (!_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
        {
            user.FailedLoginAttempts++;
            if (user.FailedLoginAttempts >= 5)
                user.LockoutEnd = DateTime.UtcNow.AddMinutes(30);

            await _context.SaveChangesAsync(cancellationToken);
            return ApiResponse<LoginResponse>.FailureResponse("Invalid email or password");
        }

        // Reset failed attempts on successful login
        user.FailedLoginAttempts = 0;
        user.LastLoginAt = DateTime.UtcNow;

        var roles = user.UserRoles.Select(ur => ur.Role.Name).ToList();
        var permissions = user.UserRoles
            .SelectMany(ur => ur.Role.RolePermissions)
            .Select(rp => rp.Permission.Name)
            .Distinct()
            .ToList();

        var (accessToken, expiresAt) = _jwtTokenGenerator.GenerateAccessToken(user, roles, permissions);
        // Generate raw refresh token (returned to client)
        var refreshToken = RefreshTokenHelper.Generate();

        // Hash before storing
        var refreshTokenHash = RefreshTokenHelper.Hash(refreshToken);

        var refreshTokenEntity = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            TokenHash = refreshTokenHash,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        };

        _context.RefreshTokens.Add(refreshTokenEntity);
        await _context.SaveChangesAsync(cancellationToken);

        var userDto = new UserDto(
            user.Id,
            user.Email,
            user.Username,
            user.FirstName,
            user.LastName,
            user.FullName,
            user.Department,
            roles,
            permissions
        );

        var response = new LoginResponse(accessToken, refreshToken, expiresAt, userDto);
        return ApiResponse<LoginResponse>.SuccessResponse(response, "Login successful");
    }
}