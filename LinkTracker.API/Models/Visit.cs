namespace LinkTracker.API.Models
{
    public record Visit(string Filename, DateTime UtcTime, string? ReferralId = null);
}