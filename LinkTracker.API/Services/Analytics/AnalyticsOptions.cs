using LinkTracker.Shared.Models;

namespace LinkTracker.API.Services.Analytics
{
    public class AnalyticsOptions
    {
        public bool OnlyShowReferrals { get; set; }
        public string? FileName { get; set; }
        public string? Referral { get; set; }

        public AnalyticsOptions()
        {
            OnlyShowReferrals = false;
            FileName = null;
            Referral = null;
        }

        public IEnumerable<Visit> ApplyOptions(IEnumerable<Visit> visits)
        {
            if (OnlyShowReferrals) visits = visits.Where(v => v.ReferralId is not null);
            if (!string.IsNullOrEmpty(FileName)) visits = visits.Where(v => v.Filename == FileName);
            if (!string.IsNullOrEmpty(Referral)) visits = visits.Where(v => v.ReferralId == Referral);
            return visits;
        }
    }
}
