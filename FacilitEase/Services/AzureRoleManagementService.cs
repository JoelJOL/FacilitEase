using System.Net.Http.Headers;
using System.Text.Json;

namespace FacilitEase.Services
{
    public class AzureRoleManagementService : IAzureRoleManagementService
    {
        private readonly HttpClient _httpClient;

        public AzureRoleManagementService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<dynamic> GetAppRoles(string accessToken)
        {
            try
            {
                // Construct the request URL to fetch application roles
                var requestUrl = "https://graph.microsoft.com/d7104f84-ab29-436f-8f06-82fcf8d81381/me";

                // Set up HttpClient with provided accessToken
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Send GET request
                var response = await _httpClient.GetAsync(requestUrl);

                if (response.IsSuccessStatusCode)
                {
                    // Read response content
                    var responseContent = await response.Content.ReadAsStringAsync();

                    // Deserialize JSON response to dynamic object
                    dynamic application = JsonSerializer.Deserialize<dynamic>(responseContent);

                    // Log and return application roles
                    Console.WriteLine(application);
                    return application;
                }
                else
                {
                    // Log failure if request was unsuccessful
                    Console.WriteLine($"Failed to fetch application details. StatusCode: {response.StatusCode}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }
    }
}