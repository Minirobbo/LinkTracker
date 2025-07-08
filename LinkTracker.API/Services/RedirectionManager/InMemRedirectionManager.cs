namespace LinkTracker.API.Services.RedirectionManager
{
    public class InMemRedirectionManager : IRedirectionManager
    {
        private Dictionary<string, string> codeUrlPairs = [];

        public async Task<bool> CreateLink(string redirectCode, string url)
        {
            return codeUrlPairs.TryAdd(redirectCode, url);
        }

        public async Task<string> GetLink(string redirectCode)
        {
            return codeUrlPairs.GetValueOrDefault(redirectCode, string.Empty);
        }
    }
}
