using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CrowdCover.Web.Data;
using CrowdCover.Web.Models;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using CrowdCover.Web.Models.ViewModels;
using CrowdCover.Web.Services;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.Text.Encodings.Web;
using CrowdCover.Web.Client;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using System.Linq;
using Microsoft.Extensions.Logging;
using Humanizer;

namespace CrowdCover.Web.Controllers
{

    public class UserViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Id { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }


    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;
        //room access service
        private readonly RoomAccessService _roomAccessService;
        private readonly IEmailSender _emailSender;
        SharpSportsClient _sharpSportsClient;

        public AccountController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IConfiguration configuration,
            ApplicationDbContext context,
            RoomAccessService roomAccessService,
           IEmailSender emailSender,
          SharpSportsClient sharpSportsClient
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _context = context;
            _roomAccessService = roomAccessService;
            _emailSender = emailSender;
            _sharpSportsClient = sharpSportsClient;
        }

        // Helper function to check if the email is valid
        private bool IsValidEmail(string email)
        {
            var emailAttribute = new EmailAddressAttribute();
            return emailAttribute.IsValid(email);
        }

        public class FetchContextRequest
        {
            [Required]
            public string InternalId { get; set; }

            public string RedirectUrl { get; set; }
        }

        public class SharpsportsContext
        {
            public string cid { get; set; }
        }


        /// <summary>
        /// Shows evemts user has placed a bet against
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpGet("get-user-events")]
        //[Authorize]
        public async Task<IActionResult> GetUserEvents([FromQuery] string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                return BadRequest("Username is required.");
            }

            try
            {
                // Fetch UserExtra based on the username
                var userExtra = await _context.UserExtras.AsNoTracking()
                    .FirstOrDefaultAsync(ue => ue.Username.ToLower() == userName.ToLower());
                if (userExtra == null)
                {
                    return NotFound("User not found.");
                }

                // Get the associated userId
                var userId = userExtra.UserId;

                // Fetch all bettors linked to the userId
                var bettors = await _context.Bettors.AsNoTracking()
                    .Where(b => b.UserId == userId)
                    .ToListAsync();
                if (!bettors.Any())
                {
                    return NotFound("No bettors linked to this user.");
                }

                var bettorIds = bettors.Select(X => X.Id).ToList();
                // Collect all bettor account IDs
                var bettorAccountIds = _context.BettorAccounts.AsNoTracking()
                    .Where(x => bettorIds.Contains(x.Bettor))
                    .Select(ba => ba.Id)
                    .ToList();

                // Fetch bets directly from Bet table via bettor accounts
                var betSlips = await _context.BetSlips.AsNoTracking()
                    .Where(b => bettorIds.Contains(b.Bettor))
                    .Include(x => x.Bets).ThenInclude(x => x.Event)
                    //.Include(b => b.Event) // Include the associated Event
                    .ToListAsync();

                var justEventIds = betSlips
                 .SelectMany(bs => bs.Bets) // Flatten all bets from bet slips
                 .Where(b => b.EventId != null) // Ensure EventId is not null
               //  .Select(b => b.EventId) // Select EventId
                 .Distinct() // Ensure unique EventIds
                 .ToList();


                if (!justEventIds.Any())
                {
                    return NotFound("No events found for this user's bets.");
                }
                             

                return Ok(justEventIds);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing the request.", error = ex.Message });
            }
        }


        [HttpGet("show-user-room-access")]
        //[Authorize]
        public async Task<IActionResult> ShowUserRoomAccess([FromQuery] string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                return BadRequest("Username is required.");
            }

            try
            {
                // Fetch UserExtra based on the username
                var userExtra = await _context.UserExtras.AsNoTracking()
                    .FirstOrDefaultAsync(ue => ue.Username.ToLower() == userName.ToLower());
                if (userExtra == null)
                {
                    return NotFound("User not found.");
                }

                // Get the associated userId
                var userId = userExtra.UserId;

                // Fetch all bettors linked to the userId
                var bettors = await _context.Bettors.AsNoTracking()
                    .Where(b => b.UserId == userId)
                    .ToListAsync();
                if (!bettors.Any())
                {
                    return NotFound("No bettors linked to this user.");
                }

                var bettorIds = bettors.Select(X=>X.Id).ToList();
                // Collect all bettor account IDs
                var bettorAccountIds = _context.BettorAccounts.AsNoTracking()
                    .Where(x=> bettorIds.Contains(x.Bettor))
                    .Select(ba => ba.Id)                    
                    .ToList();

                // Fetch bets directly from Bet table via bettor accounts
                var betSlips = await _context.BetSlips.AsNoTracking()
                    .Where(b => bettorIds.Contains( b.Bettor))
                    .Include(x=>x.Bets).ThenInclude(x=>x.Event)
                    //.Include(b => b.Event) // Include the associated Event
                    .ToListAsync();

                var justEventIds = betSlips
                 .SelectMany(bs => bs.Bets) // Flatten all bets from bet slips
                 .Where(b => b.EventId != null) // Ensure EventId is not null
                 .Select(b => b.EventId) // Select EventId
                 .Distinct() // Ensure unique EventIds
                 .ToList();


                if (!justEventIds.Any())
                {
                    return NotFound("No events found for this user's bets.");
                }

                // Fetch all streaming rooms linked to these events
                var rooms = await _context.StreamingRoomEvents.AsNoTracking()
                    .Include(x=>x.StreamingRoom)
                    .Where(x=> justEventIds.Contains( x.EventId))
                    .ToListAsync();

                var distinctRooms = rooms.Select(X => X.StreamingRoom).ToList().Distinct().ToList();

                if (!distinctRooms.Any())
                {
                    return NotFound("No rooms found for the events linked to this user's bets.");
                }

                // Map results to a simplified view model
                var result = distinctRooms.Select(room => new
                {
                    room.Id,
                    room.Name,
                    room.Description,
                    room.Slug,
                    room.Active,
                });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing the request.", error = ex.Message });
            }
        }


        //[Authorize]

        [HttpPost("fetch-context-token")]
        public async Task<IActionResult> FetchContextToken([FromBody] FetchContextRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.InternalId))
            {
                return BadRequest(new { message = "InternalId is required." });
            }

            try
            {
                var contextToken = await _sharpSportsClient.FetchBetSyncContextAsync(request.InternalId, request.RedirectUrl);
                var parsedToken = JsonConvert.DeserializeObject<SharpsportsContext>(contextToken);

                if (string.IsNullOrEmpty(contextToken))
                {
                    return StatusCode(500, new { message = "Failed to fetch context token." });
                }

                return Ok(new { ContextToken = parsedToken });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching the context token.", error = ex.Message });
            }
        }

        public class LinkBettorRequest()
        {
            public string UserName { get; set; }
            public string InternalId { get; set; }
        }

        [HttpPost("link-bettor-to-user")]
        [Authorize]
        public async Task<IActionResult> LinkBettorToUser([FromBody] LinkBettorRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.InternalId))
            {
                return BadRequest(new { message = "InternalId is required." });
            }

            // Get the currently logged-in user
            var user = await _context.UserExtras.AsNoTracking().FirstOrDefaultAsync(x => x.Username.Trim().ToLower() == request.UserName.Trim().ToLower());
            if (user == null)
            {
                return Unauthorized(new { message = "User is not authenticated." });
            }

            try
            {
                // Find the Bettor by the provided InternalId
                var bettor = await _context.Bettors.FirstOrDefaultAsync(b => b.InternalId == request.InternalId);

                if (bettor == null)
                {
                    return NotFound(new { message = "Bettor not found." });
                }

                // Check if the Bettor is already linked to another user
                if (!string.IsNullOrEmpty(bettor.UserId) && bettor.UserId != user.UserId)
                {
                    return BadRequest(new { message = "This bettor is already linked to another user." });
                }

                // Link the bettor to the current user
                bettor.UserId = user.UserId;
                _context.Bettors.Update(bettor);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Bettor successfully linked to the user." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while linking the bettor.", error = ex.Message });
            }
        }



        // Forgot Password Endpoint
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                // Don't reveal that the user does not exist or is not confirmed
                return Ok(new { message = "If the email is registered, you will receive a reset link." });
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            var callbackUrl = Url.Action(
                nameof(ResetPassword),
                "Account",
                new { code },
                protocol: Request.Scheme);

            await _emailSender.SendEmailAsync(
                model.Email,
                "Reset Password",
                $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>."
            );

            return Ok(new { message = "If the email is registered, you will receive a reset link." });
        }

        public class ForgotPasswordModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            // Check if the email address is valid
            if (!IsValidEmail(model.Email))
            {
                return BadRequest(new { message = "The email address is not in a valid format." });
            }

            var existingUser = await _context.UserExtras.FirstOrDefaultAsync(x => x.Username.ToLower() == model.Username.ToLower());
            if (existingUser != null)
            {
                return BadRequest(new { message = "There is already a user with that name" });
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new IdentityUser { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // Automatically confirm the email
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var confirmResult = await _userManager.ConfirmEmailAsync(user, token);

                if (confirmResult.Succeeded)
                {
                    // Create the UserExtra entity
                    var userExtra = new UserExtra
                    {
                        SharpsportBettorId = "",
                        Username = model.Username,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        UserId = user.Id
                    };

                    _context.UserExtras.Add(userExtra);
                    await _context.SaveChangesAsync();

                    // Automatically log in the user after registration
                    var loginResult = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, isPersistent: false, lockoutOnFailure: false);
                    if (loginResult.Succeeded)
                    {
                        // Generate JWT token
                        var jwtToken = GenerateJwtToken(user);
                        var refreshToken = await GenerateRefreshToken(user);

                        return Ok(new { Token = jwtToken, RefreshToken = refreshToken, Message = "Registration and login successful" });
                    }
                    else
                    {
                        return BadRequest(new { message = "Registration successful but automatic login failed." });
                    }
                }
                else
                {
                    return BadRequest(confirmResult.Errors);
                }
            }

            return BadRequest(result.Errors);
        }


        [HttpPost("clear-linked-bettors")]
        [Authorize]
        public async Task<IActionResult> ClearLinkedBettors([FromQuery] string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                return BadRequest("Username is required.");
            }

            try
            {
                // Fetch UserExtra based on the username
                var userExtra = await _context.UserExtras.AsNoTracking()
                    .FirstOrDefaultAsync(ue => ue.Username.ToLower() == userName.ToLower());
                if (userExtra == null)
                {
                    return NotFound("User not found.");
                }

                // Get the associated userId
                var userId = userExtra.UserId;

                // Fetch all bettors linked to the userId
                var bettors = await _context.Bettors.AsNoTracking()
                    .Where(b => b.UserId == userId)
                    .ToListAsync();
                if (!bettors.Any())
                {
                    return NotFound("No bettors linked to this user.");
                } 

                // Nullify the UserId field for all linked bettors
                foreach (var bettor in bettors)
                {
                    bettor.UserId = "";
                }

                // Save changes to the database
                _context.Bettors.UpdateRange(bettors);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Successfully cleared linked bettors." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while clearing linked bettors.", error = ex.Message });
            }
        }




        [HttpGet("linked-bettors")]
        public async Task<IActionResult> GetLinkedBettorsAndAccounts(string username)
        {
            // Get the logged-in user

            //get extra
            var userExtra = await _context.UserExtras.AsNoTracking().FirstOrDefaultAsync(x => x.Username.ToLower() == username.ToLower());
            if (userExtra == null)
            {
                return BadRequest("User does not exist.");
            }

            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userExtra.UserId);




            // Fetch all bettors linked to the logged-in user
            var bettors = await _context.Bettors
                .Where(b => b.UserId == user.Id)
                .Include(b => b.Metadata) // Optional: Include metadata if needed
                .ToListAsync();

            //if (bettors == null || !bettors.Any())
            //{
            //    return NotFound("No bettors linked to this user.");
            //}

            // Fetch all bettor accounts for each bettor
            var result = new List<UserBettorViewModel>();

            foreach (var bettor in bettors)
            {
                var bettorAccounts = await _context.BettorAccounts
                    .Where(ba => ba.Bettor == bettor.Id)
                    .Include(ba => ba.Book)
                    .Include(ba => ba.BookRegion)
                    .Include(ba => ba.Metadata)
                    .ToListAsync();

                var bettorViewModel = new UserBettorViewModel
                {
                    BettorId = bettor.Id,
                    InternalId = bettor.InternalId,
                    Metadata = bettor.Metadata,
                    BettorAccounts = bettorAccounts.Select(ba => new BettorAccountViewModel
                    {
                        Id = ba.Id,
                        BettorId = ba.Bettor,
                        Book = ba.Book,
                        BookRegion = ba.BookRegion,
                        Verified = ba.Verified,
                        Access = ba.Access,
                        Paused = ba.Paused,
                        BetRefreshRequested = ba.BetRefreshRequested,
                        LatestRefreshResponse = ba.LatestRefreshResponse,
                        Balance = ba.Balance,
                        TimeCreated = ba.TimeCreated,
                        MissingBets = ba.MissingBets,
                        IsUnverifiable = ba.IsUnverifiable,
                        RefreshInProgress = ba.RefreshInProgress,
                        TFA = ba.TFA,
                        Metadata = ba.Metadata
                    }).ToList()
                };

                result.Add(bettorViewModel);
            }

            // Fetch accessible rooms for the user
            var accessibleRooms = await _roomAccessService.GetAccessibleRoomsForUser(user.Id);

            // Return both bettors and rooms as a combined result
            userExtra.User = null;

            var userViewModel = new UserViewModel
            {
                FirstName = userExtra?.FirstName,
                LastName = userExtra?.LastName,
                Username = userExtra?.Username,
                Id = user?.Id,
                Email = user?.Email,
                PhoneNumber = user?.PhoneNumber
            };

            var response = new
            {
                User = userViewModel,
                Bettors = result ?? null,
                AccessibleRooms = accessibleRooms.Select(room => new
                {
                    room.Id,
                    room.Name
                }) ?? null
            };

            return Ok(response);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            var storedToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == request.RefreshToken && !rt.IsRevoked);

            if (storedToken == null || storedToken.ExpiryDate <= DateTime.UtcNow)
            {
                return Unauthorized("Invalid refresh token");
            }

            var user = await _userManager.FindByIdAsync(storedToken.UserId);
            if (user == null)
            {
                return Unauthorized("User not found");
            }

            // Revoke the old refresh token
            storedToken.IsRevoked = true;
            _context.RefreshTokens.Update(storedToken);
            await _context.SaveChangesAsync();

            // Generate a new access token and refresh token
            var newToken = GenerateJwtToken(user);
            var newRefreshToken = await GenerateRefreshToken(user);


            return Ok(new { Token = newToken, RefreshToken = newRefreshToken });
        }

        [HttpGet("get-refresh-token")]
        public async Task<IActionResult> GetRefreshToken([FromQuery] RefreshTokenRequest request)
        {
            var storedToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == request.RefreshToken && !rt.IsRevoked);

            if (storedToken == null || storedToken.ExpiryDate <= DateTime.UtcNow)
            {
                return Unauthorized("Invalid refresh token");
            }

            var user = await _userManager.FindByIdAsync(storedToken.UserId);
            if (user == null)
            {
                return Unauthorized("User not found");
            }

            // Revoke the old refresh token
            storedToken.IsRevoked = true;
            _context.RefreshTokens.Update(storedToken);
            await _context.SaveChangesAsync();

            // Generate a new access token and refresh token
            var newToken = GenerateJwtToken(user);
            var newRefreshToken = await GenerateRefreshToken(user);

            return Ok(new { Token = newToken, RefreshToken = newRefreshToken });
        }



        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var appUser = await _userManager.FindByEmailAsync(model.Email);
            if (appUser == null)
            {
                return Unauthorized("Invalid login attempt");
            }

            var result = await _signInManager.PasswordSignInAsync(appUser.UserName, model.Password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var token = GenerateJwtToken(appUser);
                var refreshToken = await GenerateRefreshToken(appUser);

                var userExtra = await _context.UserExtras.AsNoTracking()
               .FirstOrDefaultAsync(x => x.UserId == appUser.Id);

                return Ok(new { Token = token, RefreshToken = refreshToken, Username = userExtra.Username.ToLower() ?? null });
            }

            return Unauthorized("Invalid login attempt");
        }


        [HttpGet("accessible-rooms")]
        public async Task<IActionResult> GetAccessibleRooms()
        {
            // Get the logged-in user
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized("User is not logged in.");
            }

            // Call the service to get the accessible rooms
            var accessibleRooms = await _roomAccessService.GetAccessibleRoomsForUser(user.Id);

            if (!accessibleRooms.Any())
            {
                return NotFound("No accessible rooms found for this user.");
            }

            return Ok(accessibleRooms);
        }

        private async Task<string> GenerateRefreshToken(IdentityUser user)
        {
            var refreshToken = new RefreshToken
            {
                Token = Guid.NewGuid().ToString(),
                UserId = user.Id,
                ExpiryDate = DateTime.UtcNow.AddDays(7), // Set refresh token expiration
                IsRevoked = false
            };

            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync();

            return refreshToken.Token;
        }



        private string GenerateJwtToken(IdentityUser user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(7),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
