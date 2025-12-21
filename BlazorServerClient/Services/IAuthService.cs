namespace ExpenseManagement.BlazorUI.Services;

public interface IAuthService
{
    Task<LoginResult> LoginAsync(string email, string password);
    Task LogoutAsync();
}

public record LoginResult(bool Success, string Message, UserInfo? User = null);
public record UserInfo(Guid Id, string Email, string FullName, List<string> Roles);
