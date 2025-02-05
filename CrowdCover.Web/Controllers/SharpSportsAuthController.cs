//using CrowdCover.Web.Client;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;

//namespace CrowdCover.Web.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class SharpSportsAuthController : ControllerBase
//    {
//        private HttpClient _httpClient;
//        private SharpSportsClient _authService;

//        public SharpSportsAuthController()
//        {
//            // Initialize the HttpClient with the base address for SharpSports API
//            _httpClient = new HttpClient
//            {
//                BaseAddress = new Uri("https://api.sharpsports.io/")
//            };

//            // Initialize the SharpSportsAuthService with HttpClient and API key
//            _authService = new SharpSportsClient(_httpClient, "872f75892417e32172235da1ab31d73ab4c83b04");
//        }

//        [HttpGet]
//        public async Task<IActionResult> Get([FromQuery] string userName)
//        {
//            var internalId = "test_bettor";

//            // Perform the authentication and capture the result
//            var token = await _authService.AuthenticateAsync(internalId);

//            return Ok(token);
//        }
//    }
//}
