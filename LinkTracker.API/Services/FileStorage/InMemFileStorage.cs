﻿using LinkTracker.Shared.Models;

namespace LinkTracker.API.Services.FileStorage
{
    public class InMemFileStorage : IFileStorage
    {
        private Dictionary<string, StoredFile> filestorage = [];

        public async Task<StoredFile?> GetFile(string filename)
        {
            return filestorage.GetValueOrDefault(filename, null);
        }

        public async Task<bool> UploadFile(StoredFile file)
        {
            return filestorage.TryAdd(file.GetPath(), file);
        }
    }
}
