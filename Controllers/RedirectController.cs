using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace LinkTracker.Controllers
{
    [ApiController]
    public class RedirectController : Controller
    {
        [HttpGet("/{id}")]
        public async Task<ActionResult> Get(string id)
        {
            MemoryStream ms = new();
            StreamWriter writer = new(ms);
            writer.WriteLine(id);
            writer.Flush();
            return File(ms, "application/text");
        }
    }
}
