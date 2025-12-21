using ExpenseManagement.BlazorUI.Models;
using System.Net.Http.Headers;

namespace ExpenseManagement.BlazorUI.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly HttpClient _httpClient;
        private readonly ITokenService _tokenService;

        public ExpenseService(HttpClient httpClient, ITokenService tokenService)
        {
            _httpClient = httpClient;
            _tokenService = tokenService;
        }

        private async Task SetAuthorizationHeaderAsync()
        {
            var token = await _tokenService.GetTokenAsync();
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<PagedResult<ExpenseDto>> GetExpensesAsync(int pageNumber = 1, int pageSize = 10, ExpenseStatus? status = null)
        {
            await SetAuthorizationHeaderAsync();

            var statusQuery = status.HasValue ? $"&status={status.Value}" : "";
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<PagedResult<ExpenseDto>>>(
                $"/expense/expenses?pageNumber={pageNumber}&pageSize={pageSize}{statusQuery}");

            return response?.Data ?? new PagedResult<ExpenseDto>();
        }

        public async Task<ExpenseDto?> GetExpenseByIdAsync(Guid id)
        {
            await SetAuthorizationHeaderAsync();

            var response = await _httpClient.GetFromJsonAsync<ApiResponse<ExpenseDto>>($"/expense/expenses/{id}");
            return response?.Data;
        }

        public async Task<ApiResponse<ExpenseDto>> CreateExpenseAsync(CreateExpenseRequest request)
        {
            await SetAuthorizationHeaderAsync();

            var response = await _httpClient.PostAsJsonAsync("/expense/expenses", request);
            return await response.Content.ReadFromJsonAsync<ApiResponse<ExpenseDto>>()
                   ?? new ApiResponse<ExpenseDto> { Success = false, Message = "Failed to create expense" };
        }

        public async Task<ApiResponse<bool>> SubmitExpenseAsync(Guid id)
        {
            await SetAuthorizationHeaderAsync();

            var response = await _httpClient.PostAsync($"/expense/expenses/{id}/submit", null);
            return await response.Content.ReadFromJsonAsync<ApiResponse<bool>>()
                   ?? new ApiResponse<bool> { Success = false, Message = "Failed to submit expense" };
        }

        public async Task<ApiResponse<bool>> DeleteExpenseAsync(Guid id)
        {
            await SetAuthorizationHeaderAsync();

            var response = await _httpClient.DeleteAsync($"/expense/expenses/{id}");
            return await response.Content.ReadFromJsonAsync<ApiResponse<bool>>()
                   ?? new ApiResponse<bool> { Success = false, Message = "Failed to delete expense" };
        }
    }
}
