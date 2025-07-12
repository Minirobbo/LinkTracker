
using LinkTracker.Shared.Models;

namespace LinkTracker.API.Services.Analytics
{
    public class InMemAnalytics : IAnalyticsTracker
    {
        private Dictionary<string, List<Visit>> visits = [];

        public async Task<IEnumerable<Visit>> GetVisits()
        {
            return await GetVisits(o => { });
        }

        public async Task<IEnumerable<Visit>> GetVisits(Action<AnalyticsOptions> options)
        {
            AnalyticsOptions analytics = new();
            options.Invoke(analytics);
            IEnumerable<Visit> fetchedVisits;

            if (analytics.FileName is not null)
            {
                fetchedVisits = visits.GetValueOrDefault(analytics.FileName, []);
            }
            else
            {
                fetchedVisits = visits.Values.SelectMany(s => s);
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
