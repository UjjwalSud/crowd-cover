using CrowdCover.Web.Models.Sharpsports;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CrowdCover.Web.Client
{
    public class SharpSportsClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _sandboxApiKey;
        private readonly string _publicApiKey;
        private readonly string _privateApiKey;
        private readonly bool _isSandbox;

        // Constructor for sandbox mode
        public SharpSportsClient(HttpClient httpClient, string sandboxApiKey, bool isSandbox)
        {
            _httpClient = httpClient;
            _sandboxApiKey = sandboxApiKey;
            _isSandbox = isSandbox;
        }

        // Constructor for live mode (with public and private keys)
        public SharpSportsClient(HttpClient httpClient, string publicApiKey, string privateApiKey)
        {
            _httpClient = httpClient;
            _publicApiKey = publicApiKey;
            _privateApiKey = privateApiKey;
            _isSandbox = false; // Explicitly set to live mode
        }

        private void SetAuthorizationHeader(HttpMethod method)
        {
            string apiKey = _isSandbox
                ? _sandboxApiKey
                : method == HttpMethod.Get ? _privateApiKey : _publicApiKey;

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", apiKey);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public class CID
        {

            public string cid { get; set; }
        }


        // Method to fetch the context token
        public async Task<string> FetchBetSyncContextAsync(string internalId, string redirectUrl, string uiMode = "system")
        {

            //if the redirect URL is empty, add a test one
            redirectUrl = String.IsNullOrWhiteSpace(redirectUrl) ? "https://example.com/redirect" : redirectUrl;

            // Prepare the request body
            var requestBody = new
            {
                internalId = internalId,
                redirectUrl = redirectUrl,
                uiMode = uiMode
            };

            // Serialize the request body to JSON
            var jsonContent = JsonConvert.SerializeObject(requestBody);


            // Create an HTTP content with the JSON body
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            // Add the Authorization header
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Token {_publicApiKey}");

            try
            {
                // Make the POST request
                var response = await _httpClient.PostAsync("v1/context", content);

                // Ensure the response is successful
                response.EnsureSuccessStatusCode();

                // Parse the response content
                var responseContent = await response.Content.ReadAsStringAsync();

                return responseContent;
            }
            catch (Exception ex)
            {
                // Handle any errors that occur during the request
                throw new InvalidOperationException("Error fetching bet sync context", ex);
            }
        }

        // POST: Authenticate
        public async Task<AuthResponse> AuthenticateAsync(string internalId)
        {
            SetAuthorizationHeader(HttpMethod.Post);

            var requestBody = new { internalId };
            var jsonContent = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://api.sharpsports.io/v1/mobile/auth", jsonContent);
            var responseContent = await response.Content.ReadAsStringAsync();
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var authResponse = JsonConvert.DeserializeObject<AuthResponse>(content);

            return new AuthResponse
            {
                Token = authResponse.Token,
                InternalId = internalId,
                ApiKey = _isSandbox ? _sandboxApiKey : _publicApiKey
            };
        }

        private async Task<T> GetAsync<T>(string url)
        {
            // Set the default authorization header for the client
            SetAuthorizationHeader(HttpMethod.Get);

            try
            {
                // Send the GET request
                var response = await _httpClient.GetAsync(url);

                // Read the response body
                var responseBody = await response.Content.ReadAsStringAsync();

                // Log the full response (for debugging purposes)
                Console.WriteLine($"Status Code: {response.StatusCode}");

                // Log the response headers
                Console.WriteLine("Response Headers:");
                foreach (var header in response.Headers)
                {
                    Console.WriteLine($"{header.Key}: {string.Join(", ", header.Value)}");
                }

                // Ensure the response is successful (throws an exception if not)
                response.EnsureSuccessStatusCode();

                // Return the deserialized response body as the specified type (T)
                return JsonConvert.DeserializeObject<T>(responseBody);
            }
            catch (Exception ex)
            {
                // Log the error and return the default value for type T
                Console.WriteLine($"Error occurred: {ex.Message}");

                // You can return default(T) to signify a failure, or the existing data if required.
                return default;
            }
        }



        // Generic POST helper
        private async Task<T> PostAsync<T>(string url, object body = null)
        {
            SetAuthorizationHeader(HttpMethod.Post);

            var jsonContent = body != null
                ? new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json")
                : null;

            var response = await _httpClient.PostAsync(url, jsonContent);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(responseBody);
        }

        public async Task<IEnumerable<Book>> FetchBooksAsync()
        {
            // Set the authorization header for the request
            SetAuthorizationHeader(HttpMethod.Get);

            // Define the books endpoint
            const string url = "https://api.sharpsports.io/v1/books";

            try
            {
                // Send a GET request to the endpoint
                var response = await _httpClient.GetAsync(url);

                // Ensure the response is successful
                response.EnsureSuccessStatusCode();

                // Read and deserialize the response content
                var responseBody = await response.Content.ReadAsStringAsync();
                var books = JsonConvert.DeserializeObject<IEnumerable<Book>>(responseBody);

                return books;
            }
            catch (HttpRequestException ex)
            {
                // Log or handle HTTP request errors
                Console.WriteLine($"HTTP Request failed: {ex.Message}");
                throw new Exception("Failed to refresh books from the API.", ex);
            }
            catch (Exception ex)
            {
                // Log or handle unexpected errors
                Console.WriteLine($"Unexpected error: {ex.Message}");
                throw;
            }
        }


        // Fetch Bettors
        public Task<IEnumerable<Bettor>> FetchBettorsAsync() =>
                GetAsync<IEnumerable<Bettor>>("https://api.sharpsports.io/v1/bettors");

        // Fetch Bettors (with API key)
        public async Task<IEnumerable<Bettor>> FetchBettorsAsync(string apiKey)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", apiKey);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return await GetAsync<IEnumerable<Bettor>>("https://api.sharpsports.io/v1/bettors");
        }

        // Fetch Bettor Accounts
        public Task<IEnumerable<BettorAccount>> FetchBettorAccountsAsync(string bettorId) =>
            GetAsync<IEnumerable<BettorAccount>>($"https://api.sharpsports.io/v1/bettors/{bettorId}/bettorAccounts");

        // Refresh Bettor Account
        public Task<BettorAccountRefreshResponse> RefreshBettorAccountAsync(string bettorAccountId, string auth = null, string extensionVersion = null)
        {
            var url = $"https://api.sharpsports.io/v1/bettorAccounts/{bettorAccountId}/refresh";
            if (!string.IsNullOrEmpty(auth)) url += $"?auth={auth}";
            if (!string.IsNullOrEmpty(extensionVersion)) url += $"&extensionVersion={extensionVersion}";

            return PostAsync<BettorAccountRefreshResponse>(url);
        }

        // Fetch Bet Slips
        public Task<IEnumerable<BetSlip>> FetchBetSlipsAsync() =>
            GetAsync<IEnumerable<BetSlip>>("https://api.sharpsports.io/v1/betSlips?limit=100");

        // Fetch Bet Slips by Bettor
        public Task<IEnumerable<BetSlip>> FetchBetSlipsByBettorAsync(string bettorId, string apiKey) =>
            GetAsync<IEnumerable<BetSlip>>($"https://api.sharpsports.io/v1/bettors/{bettorId}/betSlips?limit=50");

        // Fetch Bet Slips (with API key)
        public async Task<IEnumerable<BetSlip>> FetchBetSlipsAsync(string apiKey)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", apiKey);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var allBetSlips = new List<BetSlip>();
            int pageNum = 1; // Start with the first page
            const int limit = 100000;
            const int pageSize = 500; // Adjusted to meet API requirements

            while (true)
            {
                try
                {
                    // Build the paginated API endpoint with both pageNum and pageSize
                    string url = $"https://api.sharpsports.io/v1/betSlips?pageSize={pageSize}&pageNum={pageNum}&limit={limit}";

                    // Log the request URL for debugging
                    Console.WriteLine($"Fetching page {pageNum}: {url}");

                    // Fetch the current page of bet slips
                    var currentPageBetSlips = await GetAsync<BetSlipPage>(url);

                    // Check for null or empty response
                    if (currentPageBetSlips == null || !currentPageBetSlips.Objects.Any())
                    {
                        break; // Exit the loop if no more bet slips
                    }

                    // Add the current page's bet slips to the complete list
                    allBetSlips.AddRange(currentPageBetSlips.Objects);

                    // Increment the page number for the next request
                    pageNum++;
                }
                catch (HttpRequestException ex)
                {
                    // Log HTTP request failure details
                    Console.WriteLine($"HTTP Request failed for page {pageNum}: {ex.Message}");
                    throw new Exception("Failed to fetch bet slips from the API.", ex);
                }
                catch (Exception ex)
                {
                    // Log unexpected errors
                    Console.WriteLine($"Unexpected error while fetching page {pageNum}: {ex.Message}");
                    throw;
                }
            }

            return allBetSlips;
        }



        public async Task<IEnumerable<BetSlip>> FetchAllBetSlipsByBettorAsync(string bettorId, string apiKey)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", apiKey);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var allBetSlips = new List<BetSlip>();
            int pageNum = 1; // Start with the first page
            const int pageSize = 200; // Max allowed limit

            while (true)
            {
                // Build the paginated API endpoint
                string url = $"https://api.sharpsports.io/v1/bettors/{bettorId}/betSlips?limit={pageSize}&pageNum={pageNum}";

                // Fetch the current page of bet slips
                var currentPageBetSlips = await GetAsync<IEnumerable<BetSlip>>(url);

                // If no more results are returned, break out of the loop
                if (currentPageBetSlips == null || !currentPageBetSlips.Any())
                    break;

                // Add the fetched bet slips to the cumulative list
                allBetSlips.AddRange(currentPageBetSlips);

                // Increment the page number to fetch the next page
                pageNum++;
            }

            return allBetSlips;
        }


        // Fetch Events
        public async Task<IEnumerable<Event>> FetchEventsAsync(string apiKey)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", apiKey);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return await GetAsync<IEnumerable<Event>>("https://api.sharpsports.io/v1/events?future=true&upcoming=true&limit=500&ascending=true");
        }
    }


    public class AuthResponse
    {

        public string Token { get; set; }
        public string InternalId { get; set; }
        public string ApiKey { get; set; }

        //Token = authResponse.Token,
        //        InternalId = internalId,
        //        ApiKey = _isSandbox? _sandboxApiKey : _publicApiKey
    };
}

