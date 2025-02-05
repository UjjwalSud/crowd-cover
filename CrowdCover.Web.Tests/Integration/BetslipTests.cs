using CrowdCover.Web.Client;
using CrowdCover.Web.Data;
using CrowdCover.Web.Models.Sharpsports;
using CrowdCover.Web.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace CrowdCover.Web.Tests.Integration
{
    public class BetSlipServiceTests
    {
        private HttpClient _theHttpClient;
        private SharpSportsClient _apiClient;
        private ApplicationDbContext _dbContext;
        private BetSlipService _betSlipService;
        private IConfiguration _configuration;

        string completeKey = "";
        
        [SetUp]
        public void Setup()
        {
            // Load configuration from appsettings.json
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            // Retrieve connection string from appsettings.json
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            // Initialize EF Core database with the real SQL Server
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            _dbContext = new ApplicationDbContext(options);

            // Initialize the HttpClient with the base address for SharpSports API
            _theHttpClient = new HttpClient
            {
                BaseAddress = new Uri("https://api.sharpsports.io/")
            };

            // Retrieve API keys from appsettings.json
            var publicKeyLive = _configuration["Sharpsports:LivePublicKey"];
            var privateKeyLive = _configuration["Sharpsports:LivePrivateKey"];
            completeKey = $"{publicKeyLive}:{privateKeyLive}";

            // Initialize the SharpSportsClient with HttpClient
            _apiClient = new SharpSportsClient(_theHttpClient, publicKeyLive, privateKeyLive);

            // Initialize the BetSlipService with the SharpSportsClient and ApplicationDbContext
            _betSlipService = new BetSlipService(_apiClient, _dbContext);
        }

        [Test]
        public async Task FetchAndSaveBetSlipsAsync_SavesDataToDatabase()
        {

            // Perform the operation to fetch bet slips and save them to the database
            IEnumerable<BetSlip> betSlips = null;
            try
            {
                betSlips = await _betSlipService.FetchAndSaveBetSlipsAsync(completeKey);
            }
            catch (Exception ex)
            {
                Assert.Fail($"An exception occurred while fetching and saving bet slips: {ex.Message}");
            }

            // Assert that the bet slips were fetched correctly
            Assert.IsNotNull(betSlips, "Bet slips should not be null.");
            Assert.IsNotEmpty(betSlips, "Bet slips list should not be empty.");

            // Assert that the bet slips were saved to the database
            var savedBetSlips = await _dbContext.BetSlips.ToListAsync();
            Assert.IsNotNull(savedBetSlips, "Saved bet slips should not be null.");
            Assert.IsNotEmpty(savedBetSlips, "Saved bet slips list should not be empty.");
            Assert.AreEqual(betSlips.Count(), savedBetSlips.Count, "The number of bet slips saved should match the number fetched.");
        }

        [Test]
        public async Task FetchAndSaveBetSlipsByBettorAsync_SavesDataToDatabase()
        {

            var bettorId = "";
            //get bettor first
            IEnumerable<Bettor> bettors = null;
            try
            {
                bettors = await _apiClient.FetchBettorsAsync(completeKey);
                bettorId = bettors.ToList().First().Id;
            }
            catch (Exception ex)
            {
                Assert.Fail($"An exception occurred while fetching bettors: {ex.Message}");
            }

            // Perform the operation to fetch bet slips by bettor and save them to the database
            IEnumerable<BetSlip> betSlips = null;
            try
            {
                betSlips = await _betSlipService.FetchAndSaveBetSlipsByBettorAsync(bettorId, completeKey);
            }
            catch (Exception ex)
            {
                Assert.Fail($"An exception occurred while fetching and saving bet slips for bettor {bettorId}: {ex.Message}");
            }

            // Assert that the bet slips were fetched correctly
            Assert.IsNotNull(betSlips, "Bet slips should not be null.");
            Assert.IsNotEmpty(betSlips, "Bet slips list should not be empty.");

            // Assert that the bet slips were saved to the database
            var savedBetSlips = await _dbContext.BetSlips.ToListAsync();
            Assert.IsNotNull(savedBetSlips, "Saved bet slips should not be null.");
            Assert.IsNotEmpty(savedBetSlips, "Saved bet slips list should not be empty.");
            Assert.AreEqual(betSlips.Count(), savedBetSlips.Count, "The number of bet slips saved should match the number fetched.");
        }

        [TearDown]
        public void TearDown()
        {
            _theHttpClient?.Dispose();
            _dbContext?.Dispose();
        }
    }
}
