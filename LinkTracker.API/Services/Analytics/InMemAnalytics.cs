
using LinkTracker.Shared.Models;

namespace LinkTracker.API.Services.Analytics
{
    public class InMemAnalytics : IAnalyticsTracker
    {
        private Dictionary<string, List<Visit>> visits = [];

        public async Task<IEnumerable<Visit>> GetVisits() => await GetVisits(new());

        public async Task<IEnumerable<Visit>> GetVisits(AnalyticsQuery analytics)
        {
            IEnumerable<Visit> fetchedVisits;
            fetchedVisits = visits.Values.SelectMany(s => s);

            if (analytics.TryGetFirstValueForProperty("fileName", out string filename))
            {
                fetchedVisits = visits.GetValueOrDefault(filename, []);
            }

            return analytics.ApplyOptions(fetchedVisits).OrderBy(v => v.UtcTime);
        }

        public async Task RecordVisit(string filename, string? referralId = null)
        {
            Visit visit = new(filename, DateTime.UtcNow, referralId);
            if (!visits.TryAdd(filename, [visit]))
            {
                visits[filename].Add(visit);
            }
        }
    }
}
