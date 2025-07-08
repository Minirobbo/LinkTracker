using LinkTracker.API.Models;
using LinkTracker.API.Services.FileStorage;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkTracker.TestSuite.Services.FileStorage
{
    public abstract class FileStorageTests<T> where T : IFileStorage
    {
        public abstract T GetBasicFileStorage();
        
        [Fact]
        public async Task GetFile_NoFiles_ReturnsNullWhenFetchingEmpty()
        {
            //Arrange
            const string FILEPATH = "testName.ext";
            var fileStorage = GetBasicFileStorage();

            //Assert
            Assert.Null(await fileStorage.GetFile(FILEPATH));
        }

        [Fact]
        public async Task UploadGetFile_NewFile_FetchesFile()
        {
            //Arrange
            const string FILEPATH = "testName.ext";
            const string CONTENT_TYPE = "application/any";
            var fileStorage = GetBasicFileStorage();
            var streamMock = new Mock<Stream>();
            var file = new StoredFile(FILEPATH, streamMock.Object, CONTENT_TYPE);

            //Act
            if (!await fileStorage.UploadFile(file)) Assert.Fail("Unable to upload file");

            //Assert
            Assert.Equal(await fileStorage.GetFile(FILEPATH), file);
        }

        [Fact]
        public async Task UploadGetFile_DuplicateFiles_FailsSecondUpload()
        {
            //Arrange
            const string FILEPATH = "testName.ext";
            const string CONTENT_TYPE = "application/any";
            var fileStorage = GetBasicFileStorage();
            var streamMock = new Mock<Stream>();
            var file = new StoredFile(FILEPATH, streamMock.Object, CONTENT_TYPE);

            //Act
            if (!await fileStorage.UploadFile(file)) Assert.Fail("Unable to upload file");

            //Assert
            Assert.False(await fileStorage.UploadFile(file));
        }

        [Fact]
        public async Task UploadGetFile_NewFile_ReturnsNullWhenFetchingNonExistantFile()
        {
            //Arrange
            const string FILEPATH = "testName.ext";
            const string FILEPATH2 = "testName2.ext";
            const string CONTENT_TYPE = "application/any";
            var fileStorage = GetBasicFileStorage();
            var streamMock = new Mock<Stream>();
            var file = new StoredFile(FILEPATH, streamMock.Object, CONTENT_TYPE);

            //Act
            if (!await fileStorage.UploadFile(file)) Assert.Fail("Unable to upload file");

            //Assert
            Assert.Null(await fileStorage.GetFile(FILEPATH2));
        }

        [Fact]
        public async Task UploadGetFile_NewFiles_FetchesMultipleFiles()
        {
            //Arrange
            const string FILEPATH = "testName.ext";
            const string FILEPATH2 = "testName2.ext";
            const string CONTENT_TYPE = "application/any";
            var fileStorage = GetBasicFileStorage();
            var streamMock = new Mock<Stream>();
            var file = new StoredFile(FILEPATH, streamMock.Object, CONTENT_TYPE);
            var file2 = new StoredFile(FILEPATH2, streamMock.Object, CONTENT_TYPE);

            //Act
            if (!await fileStorage.UploadFile(file)) Assert.Fail("Unable to upload file");
            if (!await fileStorage.UploadFile(file2)) Assert.Fail("Unable to upload file 2");

            //Assert
            Assert.Equal(await fileStorage.GetFile(FILEPATH), file);
            Assert.Equal(await fileStorage.GetFile(FILEPATH2), file2);
        }

        [Fact]
        public async Task UploadFile_DuplicateFiles_ReturnsFalseWhenUploadingDuplicate()
        {
            //Arrange
            const string FILEPATH = "testName.ext";
            const string CONTENT_TYPE = "application/any";
            var fileStorage = GetBasicFileStorage();
            var streamMock = new Mock<Stream>();
            var file = new StoredFile(FILEPATH, streamMock.Object, CONTENT_TYPE);

            //Act
            if (!await fileStorage.UploadFile(file)) Assert.Fail("Unable to upload first file");

            //Assert
            Assert.False(await fileStorage.UploadFile(file));
        }
    }

    public class InMemoryFileStorageTests : FileStorageTests<InMemFileStorage>
    {
        public override InMemFileStorage GetBasicFileStorage()
        {
            return new InMemFileStorage();
        }
    }
}
