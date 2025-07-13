using LinkTracker.Shared.Models;

namespace LinkTracker.DashboardWASM.Services
{
    public interface IFetchAnalytics
    {
        public Task<IEnumerable<Visit>> GetVisitsAsync(string? filename = null, string? referral = null);
    }
}
