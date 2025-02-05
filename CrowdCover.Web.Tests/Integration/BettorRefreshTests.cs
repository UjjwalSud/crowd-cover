//using CrowdCover.Web.Client;
//using CrowdCover.Web.Services;
//using CrowdCover.Web.Models.Sharpsports;
//using NUnit.Framework;
//using System;
//using System.Net.Http;
//using System.Threading.Tasks;

//namespace CrowdCover.Web.Tests.Integration
//{
//    public class BettorAccountServiceTests
//    {
//        private HttpClient _theHttpClient;
//        private SharpSportsClient _apiClient;
//        private BettorService _bettorAccountService;

//        [SetUp]
//        public void Setup()
//        {
//            // Initialize the HttpClient with the base address for SharpSports API
//            _theHttpClient = new HttpClient
//            {
//                BaseAddress = new Uri("https://api.sharpsports.io/")
//            };

//            // Initialize the SharpSportsClient with HttpClient
//            _apiClient = new SharpSportsClient(_theHttpClient, "your_api_key_here"); // Replace with a valid API key

//            // Initialize the BettorAccountService with the SharpSportsClient
//            _bettorAccountService = new BettorService(_apiClient, new Data.ApplicationDbContext());
//        }

//        [Test]
//        public async Task RefreshBettorAccountAsync_ValidBettorAccountId_ReturnsRefreshResponse()
//        {
//            // The API key and bettor account ID for testing
//            var bettorAccountId = "BETTOR_ACCOUNT_ID"; // Replace with a valid bettor account ID for the test

//            //get bettor first
//            IEnumerable<Bettor> bettors = null;
//            try
//            {
//                bettors = await _apiClient.FetchBettorsAsync(apiKey);
//                bettorId = bettors.ToList().First().Id;
//            }
//            catch (Exception ex)
//            {
//                Assert.Fail($"An exception occurred while fetching bettors: {ex.Message}");
//            }

//            // Perform the operation to refresh the bettor account
//            BettorAccountRefreshResponse refreshResponse = null;
//            try
//            {
//                refreshResponse = await _bettorAccountService.RefreshBettorAccountAsync(bettorAccountId);
//            }
//            catch (Exception ex)
//            {
//                Assert.Fail($"An exception occurred while refreshing bettor account {bettorAccountId}: {ex.Message}");
//            }

//            // Assert that the refresh response is not null
//            Assert.IsNotNull(refreshResponse, "Refresh response should not be null.");
//        }

//        [TearDown]
//        public void TearDown()
//        {
//            _theHttpClient?.Dispose();
//        }
//    }
//}
