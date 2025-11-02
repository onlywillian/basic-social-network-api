using System.Net.Http.Json;
using System.Text.Json;
using FunctionalTests.Support;

namespace FunctionalTests.Drivers
{
    public class ApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;

        public ApiClient()
        {
            var baseUrl = Environment.GetEnvironmentVariable("API_BASE_URL") 
                ?? "https://localhost:7166";

            var handler = new HttpClientHandler();
            
            if (Environment.GetEnvironmentVariable("IGNORE_SSL_ERRORS") == "true")
            {
                handler.ServerCertificateCustomValidationCallback = 
                    (message, cert, chain, errors) => true;
            }

            _httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri(baseUrl),
                Timeout = TimeSpan.FromSeconds(30)
            };

            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        public async Task<HttpResponseMessage> LoginAsync(LoginRequest request)
        {
            return await _httpClient.PostAsJsonAsync("/login", request, _jsonOptions);
        }

        public async Task<HttpResponseMessage> GetMeAsync(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            
            return await _httpClient.GetAsync("/me");
        }

        public void ClearAuthentication()
        {
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}

