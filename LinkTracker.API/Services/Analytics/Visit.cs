namespace LinkTracker.API.Services.Analytics
{
    public class Visit
    {
        public string Filename { get; set; }
        public string? ReferralId { get; set; }

        public Visit(string filename, string? referralId = null)
        {
            Filename = filename;
            ReferralId = referralId;
        }
    }
}
