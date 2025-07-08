using LinkTracker.API.Services.Analytics;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace LinkTracker.API.Controllers
{
    [ApiController]
    public class AnalyticsController : Controller
    {
        private async Task<ActionResult> GetAnalyticsWithOptions(IAnalyticsTracker analyticsTracker, Action<AnalyticsOptions> options)
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
        /// <returns>Returns all logged visits for all files</returns>
        [HttpGet("/analytics")]
        public async Task<ActionResult> GetAnalytics(IAnalyticsTracker analyticsTracker, [FromQuery] string? referral = null)
        {
            return await GetAnalyticsWithOptions(analyticsTracker, o => o.Referral = referral);
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
            return await GetAnalyticsWithOptions(analyticsTracker, o => 
            { 
                o.FileName = filename;
                o.Referral = referral;
            });
        }
    }
}
