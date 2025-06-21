using LinkTracker.API.Models;

namespace LinkTracker.API.Services.FileStorage
{
    public interface IFileStorage
    {
        public Task<bool> UploadFile(StoredFile file);
        public Task<StoredFile?> GetFile(string filename);
    }
}
