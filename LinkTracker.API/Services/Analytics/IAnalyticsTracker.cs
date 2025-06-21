namespace LinkTracker.API.Services.Analytics
{
    public interface IAnalyticsTracker
    {
        public Task RecordVisit(string filename, string? referralId = null);
        public Task<IEnumerable<Visit>> GetVisits();
        public Task<IEnumerable<Visit>> GetVisits(Action<AnalyticsOptions> options);
    }
}
