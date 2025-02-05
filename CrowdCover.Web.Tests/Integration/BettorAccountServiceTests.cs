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
    public class BettorAccountServiceTests
    {
        private HttpClient _theHttpClient;
        private SharpSportsClient _apiClient;
        private ApplicationDbContext _dbContext;
        private BettorAccountService _bettorAccountService;
        private BetSlipService _betSlipService;
        private BettorService _bettorService;

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

            // Initialize EF Core with the real database connection string
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer("Data Source=173.212.231.129;Persist Security Info=True;User ID=NadavDrewe;Password=Diagonal23;Initial Catalog=CrowdCover;Encrypt=False")
                .EnableSensitiveDataLogging()
                .Options;

            _dbContext = new ApplicationDbContext(options);

            // Initialize the BettorAccountService with the SharpSportsClient and ApplicationDbContext
            _bettorAccountService = new BettorAccountService(_apiClient, _dbContext);

            // Initialize the BetSlipService with the SharpSportsClient and ApplicationDbContext
            _betSlipService = new BetSlipService(_apiClient, _dbContext);

            _bettorService = new BettorService(_apiClient, _dbContext);

            completeKey = publicKeyLive + ":" + privateKeyLive;
        }

        [Test]
        public async Task FetchAndSaveBooksAsync_SavesDataToDatabase()
        {
            // Step 1: Fetch books from the SharpSports API
            IEnumerable<Book> books = null;
            try
            {
                books = await _apiClient.FetchBooksAsync();
            }
            catch (Exception ex)
            {
                Assert.Fail($"An exception occurred while fetching books: {ex.Message}");
            }

            // Step 2: Validate that books were fetched successfully
            Assert.IsNotNull(books, "Books should not be null.");
            Assert.IsNotEmpty(books, "Books list should not be empty.");

            // Step 3: Save books to the database
            try
            {
                foreach (var book in books)
                {
                    // Check if the book already exists in the database
                    var existingBook = await _dbContext.Books.FirstOrDefaultAsync(b => b.Id == book.Id);
                    if (existingBook == null)
                    {
                        await _dbContext.Books.AddAsync(book);
                    }
                    else
                    {
                        // Update existing book details (if needed)
                        existingBook.Name = book.Name;
                        // Add other properties to update as necessary
                        _dbContext.Books.Update(existingBook);
                    }
                }

                // Save changes to the database
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Assert.Fail($"An exception occurred while saving books to the database: {ex.Message}");
            }

            // Step 4: Assert that books were saved to the database
            var savedBooks = await _dbContext.Books.ToListAsync();
            Assert.IsNotNull(savedBooks, "Saved books should not be null.");
            Assert.IsNotEmpty(savedBooks, "Saved books list should not be empty.");
            Assert.AreEqual(books.Count(), savedBooks.Count, "The number of books saved should match the number fetched.");
        }



        [Test]
        public async Task FetchAndSaveBettorAccountsAsync_SavesDataToDatabase()
        {

            var bettorInternalId = "";

            // Perform the request to fetch bettors
            IEnumerable<Bettor> bettors = null;

            try
            {
                bettors = await _bettorService.FetchAndSaveBettorsAsync(completeKey);
            }
            catch (Exception ex)
            {
                Assert.Fail($"An exception occurred while fetching bettors: {ex.Message}");
            }

            foreach (var bettor in bettors)
            {

                var bettorId = bettor.InternalId;

                // Perform the operation to fetch bettor accounts and save them to the database
                IEnumerable<BettorAccount> bettorAccounts = null;
                try
                {
                    bettorAccounts = await _bettorAccountService.FetchAndSaveBettorAccountsAsync(bettorId);
                }
                catch (Exception ex)
                {
                    Assert.Fail($"An exception occurred while fetching and saving bettor accounts for bettor {bettorId}: {ex.Message}");
                }

                //now betslips

                IEnumerable<BetSlip> betSlips = null;
                try
                {
                    betSlips = await _betSlipService.FetchAndSaveBetSlipsByBettorAsync(bettorId, completeKey);
                }
                catch (Exception ex)
                {
                    Assert.Fail($"An exception occurred while fetching and saving bet slips for bettor {bettorId}: {ex.Message}");
                }

               
            }

            // Assert that the bettor accounts were fetched correctly
            //Assert.IsNotNull(bettorAccounts, "Bettor accounts should not be null.");
            //Assert.IsNotEmpty(bettorAccounts, "Bettor accounts list should not be empty.");

            //// Assert that the bettor accounts were saved to the database
            //var savedBettorAccounts = await _dbContext.BettorAccounts.ToListAsync();
            //Assert.IsNotNull(savedBettorAccounts, "Saved bettor accounts should not be null.");
            //Assert.IsNotEmpty(savedBettorAccounts, "Saved bettor accounts list should not be empty.");
            //Assert.AreEqual(bettorAccounts.Count(), savedBettorAccounts.Count, "The number of bettor accounts saved should match the number fetched.");
        }

        [TearDown]
        public void TearDown()
        {
            _theHttpClient?.Dispose();
            _dbContext?.Dispose();
        }
    }
}
