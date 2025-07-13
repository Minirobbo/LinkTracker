using LinkTracker.Shared.Models;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace LinkTracker.DashboardWASM.Services
{
    public class FetchAnalyticsFromAPI : IFetchAnalytics
    {
        private HttpClient _httpClient;

        public FetchAnalyticsFromAPI(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Visit>> GetVisitsAsync(string? filename = null, string? referral = null)
        {
            var response = await _httpClient.GetAsync("/analytics");
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var stream = await response.Content.ReadAsStreamAsync();
                JsonSerializerOptions options = new(JsonSerializerDefaults.Web);
                return await JsonSerializer.DeserializeAsync<IEnumerable<Visit>>(stream, options);
            }
            else
            {
                return [];
            }
        }
    }
}
