namespace LinkTracker.API.Services.Analytics
{
    public class AnalyticsOptions
    {
        public bool OnlyShowReferrals { get; set; }
        public string? FileName { get; set; }

        public AnalyticsOptions()
        {
            OnlyShowReferrals = false;
            FileName = null;
        }
    }
}
