
using LinkTracker.Shared.Models;

namespace LinkTracker.API.Services.Analytics
{
    public class InMemAnalytics : IAnalyticsTracker
    {
        //private Dictionary<string, List<Visit>> visits = [];
        //Testing code:
        private Dictionary<string, List<Visit>> visits = new() {
            {"abc", [new Visit("abc", new DateTime(2025, 7, 8)),
                     new Visit("abc", new DateTime(2025, 7, 9)),
                     new Visit("abc", new DateTime(2025, 7, 12)),
                     new Visit("abc", new DateTime(2025, 7, 12)),
                     new Visit("abc", new DateTime(2025, 7, 13)),
                     new Visit("abc", new DateTime(2025, 7, 14)),
                     new Visit("abc", new DateTime(2025, 7, 18))] }
        };

        public async Task<IEnumerable<Visit>> GetVisits() => await GetVisits(new());

        public async Task<IEnumerable<Visit>> GetVisits(AnalyticsQuery analytics)
        {
            Console.WriteLine(analytics.QueryClause);
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
