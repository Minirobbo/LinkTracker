using LinkTracker.Shared.Models;

namespace LinkTracker.DashboardWASM.Services
{
    public class FetchAnalyticsFromAPI : IFetchAnalytics
    {
        public IEnumerable<Visit> GetVisits(string? filename = null, string? referral = null)
        {
            //TODO: Implement API Access
            throw new NotImplementedException();
        }
    }
}
