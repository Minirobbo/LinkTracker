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

        public async Task<IEnumerable<Visit>> GetVisitsAsync() => await GetVisitsAsync(new());

        public async Task<IEnumerable<Visit>> GetVisitsAsync(AnalyticsQuery query)
        {
            var response = await _httpClient.GetAsync($"/analytics{query.QueryClause}");
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
