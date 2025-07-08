using LinkTracker.API.Services.RedirectionManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkTracker.TestSuite.Services.RedirectionManager
{
    public abstract class RedirectionManagerTests<T> where T : IRedirectionManager
    {
        public abstract T GetBaseRedirector();

        [Fact]
        public async Task GetLink_NoneCreated_ReturnsEmpty()
        {
            const string code = "abcdef";
            var linkManager = GetBaseRedirector();

            Assert.True(await linkManager.GetLink(code) == "");
        }

        [Fact]
        public async Task CreateGetLink_CreateLink_ReturnsLink()
        {
            const string code = "abcdef";
            const string url = "www.google.com";
            var linkManager = GetBaseRedirector();

            if (!await linkManager.CreateLink(code, url)) Assert.Fail("Couldn't create link");

            Assert.True(await linkManager.GetLink(code) == url);
        }

        [Fact]
        public async Task CreateGetLink_CreateDuplicateLink_FailsSecondCreation()
        {
            const string code = "abcdef";
            const string url = "www.google.com";
            var linkManager = GetBaseRedirector();

            if (!await linkManager.CreateLink(code, url)) Assert.Fail("Couldn't create link");

            Assert.False(await linkManager.CreateLink(code, url));
        }

        [Fact]
        public async Task CreateGetLink_CreateLinkTryDifferent_FailsToFetchNonExistantLink()
        {
            const string code = "abcdef";
            const string code2 = "abcdefg";
            const string url = "www.google.com";
            var linkManager = GetBaseRedirector();

            if (!await linkManager.CreateLink(code, url)) Assert.Fail("Couldn't create link");

            Assert.True(await linkManager.GetLink(code2) == "");
        }

        [Fact]
        public async Task CreateGetLink_CreateLinks_ReturnsBothLinks()
        {
            const string code = "abcdef";
            const string url = "www.google.com";
            const string code2 = "abcdefg";
            const string url2 = "www.bing.com";
            var linkManager = GetBaseRedirector();

            if (!await linkManager.CreateLink(code, url)) Assert.Fail("Couldn't create link");
            if (!await linkManager.CreateLink(code2, url2)) Assert.Fail("Couldn't create link 2");

            Assert.True(await linkManager.GetLink(code) == url);
            Assert.True(await linkManager.GetLink(code2) == url2);
        }

        [Fact]
        public async Task CreateGetLink_CreateLinksWithSameUrl_ReturnsBothLinks()
        {
            const string code = "abcdef";
            const string url = "www.google.com";
            const string code2 = "abcdefg";
            var linkManager = GetBaseRedirector();

            if (!await linkManager.CreateLink(code, url)) Assert.Fail("Couldn't create link");
            if (!await linkManager.CreateLink(code2, url)) Assert.Fail("Couldn't create link 2");

            Assert.True(await linkManager.GetLink(code) == url);
            Assert.True(await linkManager.GetLink(code2) == url);
        }
    }

    public class InMemRedirectionManagerTests : RedirectionManagerTests<InMemRedirectionManager>
    {
        public override InMemRedirectionManager GetBaseRedirector()
        {
            return new InMemRedirectionManager();
        }
    }
}
