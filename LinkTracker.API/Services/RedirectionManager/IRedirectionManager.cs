using LinkTracker.API.Models;

namespace LinkTracker.API.Services.RedirectionManager
{
    public interface IRedirectionManager
    {
        /// <summary>
        /// Creates a link with the given redirect code to a url
        /// </summary>
        /// <param name="redirectCode">Code to be shared and redirected from</param>
        /// <param name="url">Url to be redirected to</param>
        /// <returns>Boolean if the linking was successful</returns>
        public Task<bool> CreateLink(string redirectCode, string url);

        /// <summary>
        /// Attempts to get a url from a redirect code
        /// </summary>
        /// <param name="redirectCode">Code to attempt to fetch</param>
        /// <returns>Returns a url string if found, is empty if no such matching code exists</returns>
        public Task<string> GetLink(string redirectCode);
    }
}
