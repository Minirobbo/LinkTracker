using LinkTracker.API.Models;

namespace LinkTracker.API.Services.FileStorage
{
    public interface IFileStorage
    {
        /// <summary>
        /// Uploads the provided file.
        /// </summary>
        /// <param name="file">File to be uploaded</param>
        /// <returns>Boolean if the upload was successful</returns>
        public Task<bool> UploadFile(StoredFile file);

        /// <summary>
        /// Attempts to return a file with a matching filename
        /// </summary>
        /// <param name="filename">Filename to attempt to fetch</param>
        /// <returns>Returns a StoredFile object that matches the given filename, returns null if no such file exists</returns>
        public Task<StoredFile?> GetFile(string filename);
    }
}
