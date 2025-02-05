using CrowdCover.Web.Client;
using NUnit.Framework;
using System.Net.Http;
using System.Threading.Tasks;

namespace CrowdCover.Web.Tests.Integration.Auth
{
    public class Tests
    {
        private HttpClient _theHttpClient;
        private SharpSportsClient _authService;

        string publicKeyLive = "f6def0ecb882af89dc8b84545740444cac876292";
        string privateKeyLive = "da06f06127f93d6beeeb20b38067869e8c843afc";

        [SetUp]
        public void Setup()
        {
            // Initialize the HttpClient with the base address for SharpSports API
            _theHttpClient = new HttpClient
            {
                BaseAddress = new Uri("https://api.sharpsports.io/")
            };

            // Initialize the SharpSportsAuthService with HttpClient and API key
            _authService = new SharpSportsClient(_theHttpClient, publicKeyLive, privateKeyLive);
        }

        [Test]
        public async Task Authenticate()
        {
            // The internal ID for the test bettor
            var internalId = "test_bettor";

            // Perform the authentication and capture the result
            var token = await _authService.AuthenticateAsync(internalId);


            // Assert that the result is not null or empty
            Assert.IsNotNull(token.Token, "Authentication token should not be null.");
            Assert.IsNotEmpty(token.Token, "Authentication token should not be empty.");

        }

        [TearDown]
        public void TearDown()
        {
            _theHttpClient?.Dispose();
        }
    }
}
