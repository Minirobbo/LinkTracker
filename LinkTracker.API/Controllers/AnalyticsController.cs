using LinkTracker.API.Services.Analytics;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace LinkTracker.API.Controllers
{
    [ApiController]
    public class AnalyticsController : Controller
    {
        [HttpGet("/analytics")]
        public async Task<ActionResult> GetGeneralAnalytics(IAnalyticsTracker analyticsTracker)
        {
            StringBuilder resultBuilder = new();

            var results = await analyticsTracker.GetVisits();

            if (!results.Any()) return NotFound();

            string output = new(results.GroupBy(v => v.Filename).SelectMany(g => $"{g.Key} {g.Count()}\n").ToArray());
            return Ok(output);
        }

        [HttpGet("/analytics/{filename}")]
        public async Task<ActionResult> GetGeneralAnalytics(string filename, IAnalyticsTracker analyticsTracker)
        {
            StringBuilder resultBuilder = new();

            var results = await analyticsTracker.GetVisits(o => o.FileName = filename);

            if (!results.Any()) return NotFound();

            string output = $"{filename} {results.Count()}";
            return Ok(output);
        }
    }
}
