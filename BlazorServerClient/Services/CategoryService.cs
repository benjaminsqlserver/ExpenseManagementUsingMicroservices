namespace ExpenseManagement.BlazorUI.Services
{
    using ExpenseManagement.BlazorUI.Models;
   
    using System.Net.Http.Headers;
    using System.Net.Http.Json;

   

    public class CategoryService : ICategoryService
    {
        private readonly HttpClient _httpClient;
        private readonly ITokenService _tokenService;

        public CategoryService(HttpClient httpClient, ITokenService tokenService)
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

        public async Task<List<CategoryDto>> GetCategoriesAsync()
        {
            await SetAuthorizationHeaderAsync();

            var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<CategoryDto>>>("/expense/categories");
            return response?.Data ?? new List<CategoryDto>();
        }

        public async Task<List<DepartmentDto>> GetDepartmentsAsync()
        {
            await SetAuthorizationHeaderAsync();

            var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<DepartmentDto>>>("/expense/departments");
            return response?.Data ?? new List<DepartmentDto>();
        }
    }

}
