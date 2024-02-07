using System.Net.Http.Headers;
using System.Text.Json;

namespace FacilitEase.Services
{
    public class AzureRoleManagementService:IAzureRoleManagementService
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
                var requestUrl = $"https://graph.microsoft.com/v1.0/applications/d7104f84-ab29-436f-8f06-82fcf8d81381";

                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await httpClient.GetAsync(requestUrl);
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    dynamic application = JsonSerializer.Deserialize<dynamic>(responseContent);
                    Console.WriteLine(application?.appRoles);
                    return application?.appRoles;
                }
                else
                {
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
