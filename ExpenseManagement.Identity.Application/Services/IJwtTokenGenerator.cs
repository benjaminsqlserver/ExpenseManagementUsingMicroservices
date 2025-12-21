using ExpenseMaqnagement.Identity.Domain.Entities;

namespace ExpenseManagement.Identity.Application.Services
{
    public interface IJwtTokenGenerator
    {
        (string Token, DateTime ExpiresAt) GenerateAccessToken(User user, List<string> roles, List<string> permissions);
        string GenerateRefreshToken();
    }
}