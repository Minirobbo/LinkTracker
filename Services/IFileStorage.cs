using LinkTracker.Models;

namespace LinkTracker.Services
{
    public interface IFileStorage
    {
        public Task<bool> UploadFile(StoredFile file);
        public Task<StoredFile?> GetFile(string filename);
    }
}
