using LinkTracker.API.Models;
using LinkTracker.API.Services.FileStorage;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace LinkTracker.API.Controllers
{
    [ApiController]
    public class RedirectController : Controller
    {
        [HttpGet("/{filename}")]
        public async Task<ActionResult> Get(string filename, IFileStorage fileStorage)
        {
            StoredFile? file = await fileStorage.GetFile(filename);
            if (file is not null) return File(file.Data, file.ContentType, file.GetPath());
            else return NotFound();
        }

        [HttpPost("/upload/{filename}")]
        public async Task<ActionResult> Upload(string filename, IFormFile file, IFileStorage fileStorage)
        {
            if (await fileStorage.UploadFile(new StoredFile(filename, file.OpenReadStream(), file.ContentType))) return Ok();
            else return BadRequest();
        }
    }
}
