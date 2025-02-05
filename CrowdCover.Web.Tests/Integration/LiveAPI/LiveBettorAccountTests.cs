using CrowdCover.Web.Client;
using CrowdCover.Web.Models.Sharpsports;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace CrowdCover.Web.Tests.Integration.LiveAPI
{
    [TestFixture]
    public class LiveBettorAccountTests
    {
        private SharpSportsClient _client;

        // Live API keys
        private const string PublicKey = "f6def0ecb882af89dc8b84545740444cac876292";
        private const string PrivateKey = "da06f06127f93d6beeeb20b38067869e8c843afc";

        [SetUp]
        public void Setup()
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://api.sharpsports.io/")
            };

            // Initialize SharpSportsClient with the live API keys
            _client = new SharpSportsClient(httpClient, PublicKey, PrivateKey);
        }
        [Test]
        public async Task GetContext()
        {
            // Arrange
            var internalId = "1015btr"; // Valid internalId for the test
            var redirectUrl = "https://example.com/redirect"; // Replace with a valid URL for testing

            string contextToken = null;

            // Act
            try
            {
                contextToken = await _client.FetchBetSyncContextAsync(internalId, redirectUrl);
            }
            catch (Exception ex)
            {
                Assert.Fail($"Fetching context token failed with exception: {ex.Message}");
            }

            // Assert
            Assert.IsNotNull(contextToken, "Context token should not be null.");
            Assert.IsNotEmpty(contextToken, "Context token should not be empty.");

            // Log the context token for debugging
            Console.WriteLine($"Fetched context token: {contextToken}");
        }


        [Test]
        public async Task FetchBettorAccountsAsync_ReturnsAccounts()
        {
            const string internalId = "1015btr"; // Working internalId

            IEnumerable<BettorAccount> bettorAccounts = null;

            try
            {
                //try authorise
                var authResponse = await _client.AuthenticateAsync(internalId);
                // Fetch bettor accounts using the internalId
                bettorAccounts = await _client.FetchBettorAccountsAsync(internalId);
            }
            catch (Exception ex)
            {
                Assert.Fail($"Fetching bettor accounts failed with exception: {ex.Message}");
            }

            // Assert that bettor accounts are retrieved
            Assert.IsNotNull(bettorAccounts, "Bettor accounts should not be null.");
            Assert.IsNotEmpty(bettorAccounts, "Bettor accounts list should not be empty.");

            // Log details of fetched accounts for debugging
            foreach (var account in bettorAccounts)
            {
                Console.WriteLine(JsonConvert.SerializeObject(account, Formatting.Indented));
            }
        }

        [TearDown]
        public void TearDown()
        {
            // Cleanup or dispose of resources if necessary
        }
    }
}
