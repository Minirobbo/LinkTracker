using LinkTracker.API.Services.Analytics;
using LinkTracker.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace LinkTracker.API.Controllers
{
    [ApiController]
    public class AnalyticsController : Controller
    {
        private async Task<ActionResult> GetAnalyticsWithOptions(IAnalyticsTracker analyticsTracker, AnalyticsQuery options)
        {
            StringBuilder resultBuilder = new();

            var results = await analyticsTracker.GetVisits(options);

            if (!results.Any()) return NoContent();

            return Ok(results);
        }

        /// <summary>
        /// Get all distinct visits for all files
        /// </summary>
        /// <param name="referral">Optional referral string to reference</param>
        /// <param name="startDate">Optional start date of visits to be returned</param>
        /// <param name="endDate">Optional end date of visits to be returned</param>
        /// <returns>Returns all logged visits for all files</returns>
        [HttpGet("/analytics")]
        public async Task<ActionResult> GetAnalytics(IAnalyticsTracker analyticsTracker, [FromQuery] string? referral = null, [FromQuery] DateTime? startTime = null, [FromQuery] DateTime? endTime = null)
        {
            AnalyticsQuery query = new();
            if (referral is not null) query = query.Where(AnalyticsFilter.Referral(referral));
            if (startTime is not null) query = query.Where(AnalyticsFilter.StartDateTime(startTime.Value));
            if (endTime is not null) query = query.Where(AnalyticsFilter.EndDateTime(endTime.Value));

            return await GetAnalyticsWithOptions(analyticsTracker, query);
        }

        /// <summary>
        /// Get all distinct visits for a given file
        /// </summary>
        /// <param name="filename">Filename to get analytics for</param>
        /// <param name="referral">Optional referral string to reference</param>
        /// <returns>Returns all logged visits for the file matching the provided filename</returns>
        [HttpGet("/analytics/{filename}")]
        public async Task<ActionResult> GetSingleAnalytics(string filename, IAnalyticsTracker analyticsTracker, [FromQuery] string? referral = null)
        {
            AnalyticsQuery query = new AnalyticsQuery().Where(AnalyticsFilter.Filename(filename));
            if (referral is not null) query = query.Where(AnalyticsFilter.Referral(referral));

            return await GetAnalyticsWithOptions(analyticsTracker, query);
        }
    }
}
