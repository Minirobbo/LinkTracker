namespace LinkTracker.API.Models
{
    public class Visit
    {
        public string Filename { get; set; }
        public string? ReferralId { get; set; }
        public DateTime UtcTime { get; set; }

        public Visit(string filename, DateTime utcTime, string? referralId = null)
        {
            Filename = filename;
            ReferralId = referralId;
            UtcTime = utcTime;
        }

        public Visit(string filename, string? referralId = null) : this(filename, DateTime.UtcNow, referralId) { }
    }
}