using LinkTracker.Shared.Models;

namespace LinkTracker.DashboardWASM.Services
{
    public interface IFetchAnalytics
    {
        public Task<IEnumerable<Visit>> GetVisitsAsync();
        public Task<IEnumerable<Visit>> GetVisitsAsync(AnalyticsQuery query);
    }
}
