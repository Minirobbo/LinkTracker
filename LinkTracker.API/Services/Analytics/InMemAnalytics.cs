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

            if (analytics.FileName is not null)
            {
                IEnumerable<Visit> matching = visits.GetValueOrDefault(analytics.FileName, []);
                return matching.Where(v => !analytics.OnlyShowReferrals || (analytics.OnlyShowReferrals && v.ReferralId is not null));
            }

            return visits.Values.SelectMany(s => s);
        }

        public async Task RecordVisit(string filename, string? referralId = null)
        {
            Visit visit = new(filename, referralId);
            if (!visits.TryAdd(filename, [visit]))
            {
                visits[filename].Add(visit);
            }
        }
    }
}
