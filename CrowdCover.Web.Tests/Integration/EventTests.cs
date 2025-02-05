using CrowdCover.Web.Client;
using CrowdCover.Web.Data;
using CrowdCover.Web.Models.Sharpsports;
using CrowdCover.Web.Services;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace CrowdCover.Web.Tests.Integration
{
    public class EventServiceTests
    {
        private HttpClient _theHttpClient;
        private SharpSportsClient _apiClient;
        private ApplicationDbContext _dbContext;
        private EventService _eventService;
        string publicKeyLive = "f6def0ecb882af89dc8b84545740444cac876292";
        string privateKeyLive = "da06f06127f93d6beeeb20b38067869e8c843afc";
        string completeKey = "";

        [SetUp]
        public void Setup()
        {
            // Initialize the HttpClient with the base address for SharpSports API
            _theHttpClient = new HttpClient
            {
                BaseAddress = new Uri("https://api.sharpsports.io/")
            };

            // Initialize the SharpSportsClient with HttpClient
            _apiClient = new SharpSportsClient(_theHttpClient, publicKeyLive, privateKeyLive); // Replace with a valid API key

            // Initialize EF Core in-memory database for testing
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _dbContext = new ApplicationDbContext(options);

            // Initialize the EventService with the SharpSportsClient and ApplicationDbContext
            _eventService = new EventService(_apiClient, _dbContext);

            completeKey = publicKeyLive + ":" + privateKeyLive;
        }

        [Test]
        public async Task FetchAndSaveEventsAsync_SavesDataToDatabase()
        {
            // The API key to authenticate the requests
            var apiKey = "872f75892417e32172235da1ab31d73ab4c83b04"; // Replace with a valid API key for the test

            // Perform the operation to fetch events and save them to the database
            IEnumerable<Event> events = null;
            try
            {
                events = await _eventService.FetchAndSaveEventsAsync(apiKey);
            }
            catch (Exception ex)
            {
                Assert.Fail($"An exception occurred while fetching and saving events: {ex.Message}");
            }

            // Assert that the events were fetched correctly
            Assert.IsNotNull(events, "Events should not be null.");
            Assert.IsNotEmpty(events, "Events list should not be empty.");

            // Assert that the events were saved to the database
            var savedEvents = await _dbContext.Events.ToListAsync();
            Assert.IsNotNull(savedEvents, "Saved events should not be null.");
            Assert.IsNotEmpty(savedEvents, "Saved events list should not be empty.");
            Assert.AreEqual(events.Count(), savedEvents.Count, "The number of events saved should match the number fetched.");
        }

        [TearDown]
        public void TearDown()
        {
            _theHttpClient?.Dispose();
            _dbContext?.Dispose();
        }
    }
}
