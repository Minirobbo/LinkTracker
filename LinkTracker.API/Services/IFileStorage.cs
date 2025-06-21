using LinkTracker.API.Models;

namespace LinkTracker.API.Services
{
    public interface IFileStorage
    {
        public Task<bool> UploadFile(StoredFile file);
        public Task<StoredFile?> GetFile(string filename);
    }
}
