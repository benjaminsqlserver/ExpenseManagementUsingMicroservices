using Microsoft.JSInterop;

namespace ExpenseManagement.BlazorUI.Services;

public class TokenService : ITokenService
{
    private readonly IJSRuntime _jsRuntime;
    private const string ACCESS_TOKEN_KEY = "accessToken";
    private const string REFRESH_TOKEN_KEY = "refreshToken";

    public TokenService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task SetTokenAsync(string token)
    {
        await _jsRuntime.InvokeVoidAsync("sessionStorage.setItem", ACCESS_TOKEN_KEY, token);
    }

    public async Task<string?> GetTokenAsync()
    {
        return await _jsRuntime.InvokeAsync<string?>("sessionStorage.getItem", ACCESS_TOKEN_KEY);
    }

    public async Task RemoveTokenAsync()
    {
        await _jsRuntime.InvokeVoidAsync("sessionStorage.removeItem", ACCESS_TOKEN_KEY);
        await _jsRuntime.InvokeVoidAsync("sessionStorage.removeItem", REFRESH_TOKEN_KEY);
    }

    public async Task SetRefreshTokenAsync(string refreshToken)
    {
        await _jsRuntime.InvokeVoidAsync("sessionStorage.setItem", REFRESH_TOKEN_KEY, refreshToken);
    }

    public async Task<string?> GetRefreshTokenAsync()
    {
        return await _jsRuntime.InvokeAsync<string?>("sessionStorage.getItem", REFRESH_TOKEN_KEY);
    }
}