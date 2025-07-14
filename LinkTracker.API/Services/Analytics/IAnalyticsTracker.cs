using LinkTracker.Shared.Models;

namespace LinkTracker.API.Services.Analytics
{
    public interface IAnalyticsTracker
    {
        /// <summary>
        /// Records a visit to a given filename with an optional referralId.
        /// </summary>
        /// <param name="filename">Filename to record the visit to</param>
        /// <param name="referralId">Optional referralId to record with the visit</param>
        public Task RecordVisit(string filename, string? referralId = null);

        /// <summary>
        /// Returns all visits in order of UTC time of visit.
        /// </summary>
        /// <returns>Enumerable list of Visit objects in order of time of visit</returns>
        public Task<IEnumerable<Visit>> GetVisits();

        /// <summary>
        /// Returns all visits in order of UTC time of visit.
        /// </summary>
        /// <param name="options">Options to apply to limit analytics to be returned</param>
        /// <returns>Enumerable list of Visit objects in order of time of visit, filtered by the options parameter</returns>
        public Task<IEnumerable<Visit>> GetVisits(AnalyticsQuery options);
    }
}
