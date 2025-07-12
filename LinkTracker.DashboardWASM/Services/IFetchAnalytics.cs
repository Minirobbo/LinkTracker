using LinkTracker.Shared.Models;

namespace LinkTracker.DashboardWASM.Services
{
    public interface IFetchAnalytics
    {
        public IEnumerable<Visit> GetVisits(string? filename = null, string? referral = null);
    }
}
