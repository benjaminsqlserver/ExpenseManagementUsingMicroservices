namespace ExpenseManagement.BlazorUI.Services;

public interface ITokenService
{
    Task SetTokenAsync(string token);
    Task<string?> GetTokenAsync();
    Task RemoveTokenAsync();
    Task SetRefreshTokenAsync(string refreshToken);
    Task<string?> GetRefreshTokenAsync();
}