//using CrowdCover.Web.Client; // Ensure this namespace includes your SharpSportsClient class
//using CrowdCover.Web.Data; // Ensure this namespace includes your ApplicationDbContext class
//using CrowdCover.Web.Models.Sharpsports; // Ensure this namespace includes your Bettor model
//using CrowdCover.Web.Services; // Ensure this namespace includes your BettorService class
//using Microsoft.EntityFrameworkCore;
//using Newtonsoft.Json;
//using NUnit.Framework;
//using System;
//using System.Collections.Generic;
//using System.Net.Http;
//using System.Threading.Tasks;

//namespace CrowdCover.Web.Tests.Integration
//{
//    public class Tests
//    {
//        private HttpClient _theHttpClient;
//        private SharpSportsClient _apiClient;
//        private ApplicationDbContext _dbContext;
//        private BettorService _bettorService;

//        string publicKeyLive = "f6def0ecb882af89dc8b84545740444cac876292";
//        string privateKeyLive = "da06f06127f93d6beeeb20b38067869e8c843afc";

//        [SetUp]
//        public void Setup()
//        {
//            // Initialize the HttpClient with the base address for SharpSports API
//            _theHttpClient = new HttpClient
//            {
//                BaseAddress = new Uri("https://api.sharpsports.io/")
//            };

//            // Initialize the SharpSportsClient with HttpClient
//            _apiClient = new SharpSportsClient(_theHttpClient, publicKeyLive, privateKeyLive);

//            //// Initialize EF Core in-memory database for testing
//            //var options = new DbContextOptionsBuilder<ApplicationDbContext>()
//            //    .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Use a unique name for each test
//            //    .Options;
//            //_dbContext = new ApplicationDbContext(options);

//            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
//              .UseSqlServer("Data Source=173.212.231.129;Persist Security Info=True;User ID=NadavDrewe;Password=Diagonal23;Initial Catalog=CrowdCover;Encrypt=False")
//              .Options;

//            _dbContext = new ApplicationDbContext(options);

//            // Initialize the BettorService with the SharpSportsClient and ApplicationDbContext
//            _bettorService = new BettorService(_apiClient, _dbContext);
//        }

//        [Test]
//        public async Task FetchBettorsAsync_ReturnsValidData()
//        {
//            // The API key to authenticate the requests
//            var apiKey = "872f75892417e32172235da1ab31d73ab4c83b04"; // Replace with a valid API key for the test

//            // Perform the request to fetch bettors
//            IEnumerable<Bettor> bettors = null;
//            try
//            {
//                bettors = await _apiClient.FetchBettorsAsync(apiKey);
//            }
//            catch (Exception ex)
//            {
//                Assert.Fail($"An exception occurred while fetching bettors: {ex.Message}");
//            }

//            // Assert that the result is not null and contains data
//            Assert.IsNotNull(bettors, "Bettors should not be null.");
//            Assert.IsNotEmpty(bettors, "Bettors list should not be empty.");
//        }

//        [Test]
//        public async Task FetchAndSaveBettorsAsync_SavesDataToDatabase()
//        {
//            // The API key to authenticate the requests
//            var apiKey = "872f75892417e32172235da1ab31d73ab4c83b04"; // Replace with a valid API key for the test

//            // Perform the operation to fetch bettors and save them to the database
//            IEnumerable<Bettor> bettors = null;
//            try
//            {
//                bettors = await _bettorService.FetchAndSaveBettorsAsync(apiKey);
//            }
//            catch (Exception ex)
//            {
//                Assert.Fail($"An exception occurred while fetching and saving bettors: {ex.Message}");
//            }

//            // Assert that the bettors were fetched correctly
//            Assert.IsNotNull(bettors, "Bettors should not be null.");
//            Assert.IsNotEmpty(bettors, "Bettors list should not be empty.");

//            // Assert that the bettors were saved to the database
//            var savedBettors = await _dbContext.Bettors.ToListAsync();
//            Assert.IsNotNull(savedBettors, "Saved bettors should not be null.");
//            Assert.IsNotEmpty(savedBettors, "Saved bettors list should not be empty.");
//            Assert.AreEqual(bettors.Count(), savedBettors.Count, "The number of bettors saved should match the number fetched.");
//        }

//        //[Test]
//        //public async Task RefreshBettorAccountAsync_ValidBettorAccountId_ReturnsRefreshResponse()
//        //{
//        //    // The API key and bettor account ID for testing
//        //    var bettorAccountId = "BETTOR_ACCOUNT_ID"; // Replace with a valid bettor account ID for the test

//        //    // Perform the operation to refresh the bettor account
//        //    BettorAccountRefreshResponse refreshResponse = null;
//        //    try
//        //    {
//        //        refreshResponse = await _bettorService.RefreshBettorAccountAsync(bettorAccountId);
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        Assert.Fail($"An exception occurred while refreshing bettor account {bettorAccountId}: {ex.Message}");
//        //    }

//        //    // Assert that the refresh response is not null
//        //    Assert.IsNotNull(refreshResponse, "Refresh response should not be null.");
//        //}

//        //[Test]
//        //public async Task FetchAndSaveBooksAsync_SavesDataToDatabase()
//        //{
//        //    // The API key to authenticate the requests
//        //    var apiKey = "872f75892417e32172235da1ab31d73ab4c83b04"; // Replace with a valid API key for the test
//        //    IEnumerable<Book> books = null;

//        //    try
//        //    {
//        //        // Fetch books from the API
//        //        books = await _apiClient.FetchBooksAsync(publicKeyLive, privateKeyLive);
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        Assert.Fail($"An exception occurred while fetching books: {ex.Message}");
//        //    }

//        //    // Save books to the database if they don't already exist
//        //    if (books != null)
//        //    {
//        //        foreach (var book in books)
//        //        {
//        //            var existingBook = await _dbContext.Books.FindAsync(book.Id);
//        //            if (existingBook == null)
//        //            {
//        //                _dbContext.Books.Add(book);
//        //            }
//        //        }

//        //        await _dbContext.SaveChangesAsync();
//        //    }

//        //    // Assert that the fetched books are not null or empty
//        //    Assert.IsNotNull(books, "Books should not be null.");
//        //    Assert.IsNotEmpty(books, "Books list should not be empty.");

//        //    // Fetch saved books from the database and perform assertions
//        //    var savedBooks = await _dbContext.Books.ToListAsync();
//        //    Assert.IsNotNull(savedBooks, "Saved books should not be null.");
//        //    Assert.IsNotEmpty(savedBooks, "Saved books list should not be empty.");
//        //    Assert.AreEqual(books.Count(), savedBooks.Count, "The number of books saved should match the number fetched.");
//        //}

//        //[TearDown]
//        //public void TearDown()
//        //{
//        //    _theHttpClient?.Dispose();
//        //    _dbContext?.Dispose();
//        //}
//    }
//}
