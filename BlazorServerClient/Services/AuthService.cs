using System.Net.Http.Json;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components.Authorization;

namespace ExpenseManagement.BlazorUI.Services;

public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;
    private readonly ITokenService _tokenService;
    private readonly AuthenticationStateProvider _authStateProvider;

    public AuthService(
        HttpClient httpClient,
        ITokenService tokenService,
        AuthenticationStateProvider authStateProvider)
    {
        _httpClient = httpClient;
        _tokenService = tokenService;
        _authStateProvider = authStateProvider;
    }

    public async Task<LoginResult> LoginAsync(string email, string password)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("/identity/auth/login", new
            {
                Email = email,
                Password = password
            });

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<LoginResponse>>();

                if (result?.Success == true && result.Data != null)
                {
                    await _tokenService.SetTokenAsync(result.Data.AccessToken);
                    await _tokenService.SetRefreshTokenAsync(result.Data.RefreshToken);

                    // Update authentication state
                    (_authStateProvider as CustomAuthenticationStateProvider)?.NotifyAuthenticationStateChanged();

                    var userInfo = new UserInfo(
                        result.Data.User.Id,
                        result.Data.User.Email,
                        result.Data.User.FullName,
                        result.Data.User.Roles
                    );

                    return new LoginResult(true, "Login successful", userInfo);
                }
            }

            return new LoginResult(false, "Invalid email or password");
        }
        catch (Exception ex)
        {
            return new LoginResult(false, $"Error: {ex.Message}");
        }
    }

    public async Task LogoutAsync()
    {
        await _tokenService.RemoveTokenAsync();
        (_authStateProvider as CustomAuthenticationStateProvider)?.NotifyAuthenticationStateChanged();
    }
}

// DTOs for API responses
public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
}

public class LoginResponse
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public UserDto User { get; set; } = null!;
}

public class UserDto
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public List<string> Roles { get; set; } = new();
    public List<string> Permissions { get; set; } = new();
}