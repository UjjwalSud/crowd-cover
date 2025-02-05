using CrowdCover.Web.Controllers;
using CrowdCover.Web.Data;
using CrowdCover.Web.Models;
using CrowdCover.Web.Models.Sharpsports;
using CrowdCover.Web.Models.ViewModels;
using CrowdCover.Web.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CrowdCover.Web.Tests.Integration
{
    public class AccountControllerIntegrationTests
    {
        private HttpClient _httpClient;

        private IServiceScope _serviceScope;
        private ApplicationDbContext _dbContext;
        private AccountController _accountController;
        private RoomAccessService _roomAccessService;
        private UserManager<IdentityUser> _userManager;
        private SignInManager<IdentityUser> _signInManager;



        [SetUp]
        public async Task Setup()
        {
            // Step 1: Setup services and use the production database connection string from your app configuration
            var services = new ServiceCollection();

            // Here, we're setting up the DbContext to use your production connection string
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json") // Load settings from your appsettings.json
                .AddEnvironmentVariables()
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Step 2: Identity services (real services)
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Step 2: Add logging services required by Identity
            services.AddLogging();

            // Step 3: JWT authentication setup, with real JWT key and issuer from configuration
            var jwtKey = configuration["Jwt:Key"];
            if (string.IsNullOrWhiteSpace(jwtKey))
            {
                throw new InvalidOperationException("JWT Key is missing in configuration.");
            }

            services.AddAuthentication()
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidAudience = configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
                    };
                });

            // Step 4: Add required services
            services.AddScoped<RoomAccessService>();

            IServiceProvider _serviceProvider;
            // Step 5: Build the service provider
            _serviceProvider = services.BuildServiceProvider();

            // Create a service scope (for proper disposal)
            _serviceScope = _serviceProvider.CreateScope();

            // Step 6: Get required services from the scope
            _dbContext = _serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            _userManager = _serviceScope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
            _signInManager = _serviceScope.ServiceProvider.GetRequiredService<SignInManager<IdentityUser>>();
            _roomAccessService = _serviceScope.ServiceProvider.GetRequiredService<RoomAccessService>();

            // Step 7: Manually instantiate the AccountController using real services
            _accountController = new AccountController(
                _userManager,
                _signInManager,
                configuration,  // Pass the real configuration for JWT token generation
                _dbContext,
                _roomAccessService,
                null,
                null
            );

            // Step 8: Create a test user and add it to the production database (if it doesn't already exist)
            var testUser = await _userManager.FindByEmailAsync("emailnadz@gmail.com");
            if (testUser == null)
            {
                testUser = new IdentityUser
                {
                    Id = "test_user_id",
                    Email = "emailnadz@gmail.com",
                    UserName = "emailnadz@gmail.com"
                };
                var createResult = await _userManager.CreateAsync(testUser, "Diagonal23!");
                if (!createResult.Succeeded)
                {
                    throw new Exception($"Failed to create test user: {string.Join(", ", createResult.Errors.Select(e => e.Description))}");
                }
            }

            // Step 9: Mock the user claims and identity in the controller's HttpContext
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, testUser.Id),
        new Claim(ClaimTypes.Email, testUser.Email)
    };

            var identity = new ClaimsIdentity(claims, "TestAuth");
            var userPrincipal = new ClaimsPrincipal(identity);

            _accountController.ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = userPrincipal }
            };
        }


        [Test]
        public async Task GetAccessibleRoomsForUser_ValidUserId_ReturnsAccessibleRooms()
        {

            var user = _dbContext.Users.First();
            // Arrange: Create a test user, bettor, bettor account, bet slips, and streaming rooms
            var userId = user.Id; // Replace with a valid user ID
         

            // Act: Call the RoomAccessService to get accessible rooms for the user
            var accessibleRooms = await _roomAccessService.GetAccessibleRoomsForUser(userId);

            // Assert: Verify that the accessible rooms contain the expected room
            Assert.IsNotNull(accessibleRooms, "Accessible rooms should not be null.");
            Assert.IsNotEmpty(accessibleRooms, "Accessible rooms should not be empty.");
            Assert.AreEqual(1, accessibleRooms.Count, "There should be exactly one accessible room.");
            Assert.AreEqual("Test Streaming Room", accessibleRooms[0].Name, "The accessible room should have the correct name.");
        }

        [Test]
        public async Task LinkBettor_ValidInternalId_LinksBettorToLoggedInUser()
        {

            // Arrange: Create a test bettor in the database
            var bettor = new Bettor
            {
                Id = Guid.NewGuid().ToString(),
                InternalId = "BTTR_1234",
                TimeCreated = DateTime.UtcNow,
                Metadata = new Metadata
                {
                    Handle = 1000,
                    UnitSize = 1,
                    NetProfit = 1000,
                    WinPercentage = 75.5,
                    TotalAccounts = 1
                }
            };

            _dbContext.Bettors.Add(bettor);
            await _dbContext.SaveChangesAsync();

            // Assume you have a logged-in user with valid auth token
            var token = "replace_with_valid_auth_token"; // JWT token
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            // Act: Make API request to link bettor
            var linkResponse = await _httpClient.PostAsJsonAsync("api/account/link-bettor", "BTTR_1234");

            // Assert: Bettor should now be linked to the logged-in user
            Assert.IsTrue(linkResponse.IsSuccessStatusCode, "Linking bettor should succeed.");
            var updatedBettor = await _dbContext.Bettors.FirstOrDefaultAsync(b => b.InternalId == "BTTR_1234");
            Assert.IsNotNull(updatedBettor.UserId, "Bettor should now be linked to the logged-in user.");
        }

        [Test]
        public async Task GetLinkedBettorsAndAccounts_ReturnsBettorsAndAccounts()
        {
            // Arrange: Create a test user, bettor, and bettor accounts
            var bettor = new Bettor
            {
                Id = Guid.NewGuid().ToString(),
                InternalId = "BTTR_5678",
                TimeCreated = DateTime.UtcNow,
                Metadata = new Metadata
                {
                    Handle = 1000,
                    UnitSize = 1,
                    NetProfit = 1000,
                    WinPercentage = 75.5,
                    TotalAccounts = 1
                },
                UserId = "replace_with_test_user_id" // Replace with valid user ID
            };

            var bettorAccount = new BettorAccount
            {
                Id = Guid.NewGuid().ToString(),
                Bettor = bettor.Id,
                Book = new Book { Id = Guid.NewGuid().ToString(), Name = "TestBook" },
                Balance = 100,
                Verified = true,
                TimeCreated = DateTime.UtcNow
            };

            _dbContext.Bettors.Add(bettor);
            _dbContext.BettorAccounts.Add(bettorAccount);
            await _dbContext.SaveChangesAsync();

            // Assume you have a logged-in user with valid auth token
            var token = "replace_with_valid_auth_token"; // JWT token
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            // Act: Make API request to get linked bettors and accounts
            var response = await _httpClient.GetAsync("api/account/linked-bettors");

            // Assert: Check the response contains the bettor and bettor accounts
            Assert.IsTrue(response.IsSuccessStatusCode, "API call to get linked bettors should succeed.");
            var content = await response.Content.ReadAsStringAsync();
            var linkedBettors = JsonConvert.DeserializeObject<List<UserBettorViewModel>>(content);
            Assert.IsNotNull(linkedBettors, "Linked bettors response should not be null.");
            Assert.IsNotEmpty(linkedBettors, "Linked bettors should not be empty.");
            Assert.AreEqual(1, linkedBettors.Count, "There should be exactly one linked bettor.");
            Assert.AreEqual(1, linkedBettors.First().BettorAccounts.Count, "The bettor should have exactly one linked bettor account.");
        }

        [Test]
        public async Task GetLinkedBettorsAndAccounts_WithLogin_ReturnsBettorsAndRooms()
        {
            // Step 1: Log in and get a JWT token
            var loginPayload = new
            {
                Email = "emailnadz@gmail.com",
                Password = "Diagonal23!"
            };

            var loginResponse = await _httpClient.PostAsJsonAsync("api/account/login", loginPayload);
            Assert.IsTrue(loginResponse.IsSuccessStatusCode, "Login should succeed.");

            var loginContent = await loginResponse.Content.ReadAsStringAsync();
            var loginData = JsonConvert.DeserializeObject<dynamic>(loginContent);
            string token = loginData.Token;

            Assert.IsFalse(string.IsNullOrEmpty(token), "JWT token should not be null or empty.");

            // Step 2: Use the token to call the 'linked-bettors' endpoint
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync("api/account/linked-bettors");

            // Step 3: Assert the results
            Assert.IsTrue(response.IsSuccessStatusCode, "The 'linked-bettors' API call should succeed.");

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<dynamic>(content);

            // Assert that bettors and accessible rooms are returned
            Assert.IsNotNull(result.Bettors, "Bettors should not be null.");
            Assert.IsNotEmpty(result.Bettors, "Bettors should not be empty.");
            Assert.IsNotNull(result.AccessibleRooms, "Accessible rooms should not be null.");
            Assert.IsNotEmpty(result.AccessibleRooms, "Accessible rooms should not be empty.");
        }

        [TearDown]
        public void TearDown()
        {
            _httpClient?.Dispose();
            _dbContext?.Dispose();
            _userManager?.Dispose();
            _serviceScope?.Dispose();
        }

        public void Dispose()
        {
            _serviceScope?.Dispose();
        }
    }
}

