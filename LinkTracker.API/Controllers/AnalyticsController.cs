using LinkTracker.API.Services.Analytics;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace LinkTracker.API.Controllers
{
    [ApiController]
    public class AnalyticsController : Controller
    {
        /// <summary>
        /// Get all distinct visits for all files
        /// </summary>
        /// <returns>Returns all logged visits for all files</returns>
        [HttpGet("/analytics")]
        public async Task<ActionResult> GetAnalytics(IAnalyticsTracker analyticsTracker)
        {
            StringBuilder resultBuilder = new();

            var results = await analyticsTracker.GetVisits();

            if (!results.Any()) return NoContent();

            return Ok(results);
        }

        /// <summary>
        /// Get all distinct visits for a given file
        /// </summary>
        /// <param name="filename">Filename to get analytics for</param>
        /// <returns>Returns all logged visits for the file matching the provided filename</returns>
        [HttpGet("/analytics/{filename}")]
        public async Task<ActionResult> GetSingleAnalytics(string filename, IAnalyticsTracker analyticsTracker)
        {
            StringBuilder resultBuilder = new();

            var results = await analyticsTracker.GetVisits(o => o.FileName = filename);

            if (!results.Any()) return NotFound();

            return Ok(results);
        }
    }
}
