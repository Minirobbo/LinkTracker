using LinkTracker.API.Services.Analytics;
using LinkTracker.API.Services.FileStorage;
using LinkTracker.API.Services.RedirectionManager;
using LinkTracker.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace LinkTracker.API.Controllers
{
    [ApiController]
    public class RedirectController : Controller
    {
        /// <summary>
        /// Gets the given filed stored with the provided name
        /// </summary>
        /// <param name="filename">Filename of file to be fetched</param>
        /// <param name="ext">Extension of file</param>
        /// <param name="referral">Optional referral string for analytics</param>
        /// <returns>Returns the matching file found in storage, or a NotFound error</returns>
        [HttpGet("/{filename}.{ext}")]
        public async Task<ActionResult> Get(string filename, string ext, IFileStorage fileStorage, IAnalyticsTracker analyticsTracker, [FromQuery] string? referral = null)
        {
            string filePath = $"{filename}.{ext}";
            StoredFile? file = await fileStorage.GetFile(filePath);
            if (file is not null)
            {
                await analyticsTracker.RecordVisit(filePath, referral);
                return File(file.Data, file.ContentType, file.GetPath());
            }
            else return NotFound();
        }

        /// <summary>
        /// Uploads a file to be able to be fetched by the provided filename
        /// </summary>
        /// <param name="filename">Filename to be searched by</param>
        /// <param name="file">Uploaded file</param>
        /// <returns>Ok if successfully uploaded, BadRequest if not</returns>
        [HttpPost("/upload/{filename}")]
        public async Task<ActionResult> Upload(string filename, IFormFile file, IFileStorage fileStorage)
        {
            if (await fileStorage.UploadFile(new StoredFile(filename, file.OpenReadStream(), file.ContentType))) return Ok();
            else return BadRequest();
        }

        /// <summary>
        /// Redirects to the link corresponding to the provided code
        /// </summary>
        /// <param name="code">Code to be checked for link</param>
        /// <param name="referral">Optional referral string for analytics</param>
        /// <returns>Redirects to the matching url, or a NotFound error</returns>
        [HttpGet("/{code}")]
        public async Task<ActionResult> Get(string code, IRedirectionManager linkManager, IAnalyticsTracker analyticsTracker, [FromQuery] string? referral = null)
        {
            string link = await linkManager.GetLink(code);
            if (!string.IsNullOrEmpty(link))
            {
                await analyticsTracker.RecordVisit(code, referral);
                return Redirect(link);
            }
            else return NotFound();
        }

        /// <summary>
        /// Sets up a code to redirect to a link provided
        /// </summary>
        /// <param name="code">Code used to redirect from</param>
        /// <param name="linkedUrl">Url to link to</param>
        /// <returns>Ok if successfully setup, BadRequest if not</returns>
        [HttpPost("/link/{code}")]
        public async Task<ActionResult> Upload(string code, string linkedUrl, IRedirectionManager linkManager)
        {
            if (await linkManager.CreateLink(code, linkedUrl)) return Ok();
            else return BadRequest();
        }
    }
}
