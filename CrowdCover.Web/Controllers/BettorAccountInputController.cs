using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CrowdCover.Web.Data; // Ensure this is the correct namespace for your DbContext
using CrowdCover.Web.Models.Sharpsports;
using Microsoft.AspNetCore.Authorization;
using CrowdCover.Web.Services;
using static CrowdCover.Web.Controllers.AccountController;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using CrowdCover.Web.Models.ViewModels;

namespace CrowdCover.Web.Controllers
{

    [Authorize]
    public class BettorAccountInputController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly BettorAccountService _bettorAccountService;
        private readonly BetSlipService _betSlipService;
        private readonly BettorService _bettorService;
        private readonly string _apiCompleteKey;
        private readonly AccountController _accountController;

        public BettorAccountInputController(ApplicationDbContext context,
        BettorAccountService bettorAccountService,
        BetSlipService betSlipService,
        BettorService bettorService,
        IConfiguration configuration,
         AccountController accountController
        )
        {
            _context = context;
            _bettorAccountService = bettorAccountService;
            _betSlipService = betSlipService;
            _bettorService = bettorService;
            _accountController = accountController;

            // Fetch API keys from configuration
            var livePublicKey = configuration["Sharpsports:LivePublicKey"];
            var livePrivateKey = configuration["Sharpsports:LivePrivateKey"];

            if (string.IsNullOrWhiteSpace(livePublicKey) || string.IsNullOrWhiteSpace(livePrivateKey))
            {
                throw new InvalidOperationException("SharpSports Live API keys are missing in the configuration.");
            }

            _apiCompleteKey = $"{livePublicKey}:{livePrivateKey}";
        }

        // GET: BettorAccounts
        public async Task<IActionResult> Index()
        {
            var bettorAccounts = await _context.BettorAccounts
                .Include(b => b.Book)
                .Include(b => b.BookRegion)
                .Include(b => b.Metadata)
                .Select(b => new
                {
                    BettorAccount = b,
                    LinkedUser = _context.UserExtras
                        .Where(ue => ue.UserId == _context.Bettors
                            .Where(bt => bt.Id == b.Bettor) // Match Bettor using the BettorAccount's Bettor string property
                            .Select(bt => bt.UserId)
                            .FirstOrDefault())
                        .Select(ue => ue.Username)
                        .FirstOrDefault()
                })
                .ToListAsync();

            // Map to a view model
            var viewModel = bettorAccounts.Select(b => new LinkedBettorAccountViewModel
            {
                BettorAccount = b.BettorAccount,
                LinkedUser = b.LinkedUser
            }).ToList();

            // Fetch the list of users for the dropdown
            ViewData["UserList"] = await GetUserListAsync();

            return View(viewModel);
        }


        // GET: BettorAccounts/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bettorAccount = await _context.BettorAccounts
                .Include(b => b.Book)
                .Include(b => b.BookRegion)
                .Include(b => b.Metadata)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (bettorAccount == null)
            {
                return NotFound();
            }

            ViewData["UserList"] = await GetUserListAsync();
            return View(bettorAccount);
        }

        // GET: BettorAccounts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BettorAccounts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Bettor,Verified,Access,Paused,BetRefreshRequested,LatestRefreshRequestId,Balance,TimeCreated,MissingBets,IsUnverifiable,RefreshInProgress,TFA")] BettorAccount bettorAccount)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bettorAccount);
                await _context.SaveChangesAsync();
                return Redirect("~/BettorAccountInput/Index");
            }
            return View(bettorAccount);
        }

        // GET: BettorAccounts/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bettorAccount = await _context.BettorAccounts.FindAsync(id);
            if (bettorAccount == null)
            {
                return NotFound();
            }

            ViewData["UserList"] = await GetUserListAsync();
            return View(bettorAccount);
        }

        // POST: BettorAccounts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Bettor,Verified,Access,Paused,BetRefreshRequested,LatestRefreshRequestId,Balance,TimeCreated,MissingBets,IsUnverifiable,RefreshInProgress,TFA")] BettorAccount bettorAccount)
        {
            if (id != bettorAccount.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bettorAccount);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BettorAccountExists(bettorAccount.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Redirect("~/BettorAccountInput/Index");
            }

            ViewData["UserList"] = await GetUserListAsync();
            return View(bettorAccount);
        }

        // GET: BettorAccounts/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bettorAccount = await _context.BettorAccounts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bettorAccount == null)
            {
                return NotFound();
            }

            return View(bettorAccount);
        }

        // POST: BettorAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var bettorAccount = await _context.BettorAccounts.FindAsync(id);
            if (bettorAccount != null)
            {
                _context.BettorAccounts.Remove(bettorAccount);
            }

            await _context.SaveChangesAsync();
            return Redirect("~/BettorAccountInput/Index");
        }


        [HttpPost("unlink-bettor")]
        [Authorize]
        public async Task<IActionResult> UnlinkBettor(string bettorId)
        {
            if (string.IsNullOrWhiteSpace(bettorId))
            {
                return BadRequest("Bettor ID is required.");
            }

            try
            {
                // Fetch the bettor by ID
                var bettor = await _context.Bettors.FirstOrDefaultAsync(b => b.Id == bettorId);
                if (bettor == null)
                {
                    return NotFound("Bettor not found.");
                }

                // Check if the bettor is already unlinked
                if (string.IsNullOrWhiteSpace(bettor.UserId))
                {
                    return BadRequest("Bettor is not linked to any user.");
                }

                // Nullify the UserId field for the specified bettor
                bettor.UserId = "";

                // Save changes to the database
                _context.Bettors.Update(bettor);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Bettor successfully unlinked from the user.";
                return Redirect("~/BettorAccountInput/Index");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while unlinking the bettor.", error = ex.Message });
            }
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LinkBettor(string bettorId, string userId)
        {
            if (string.IsNullOrWhiteSpace(bettorId) || string.IsNullOrWhiteSpace(userId))
            {
                TempData["ErrorMessage"] = "Bettor ID and User ID are required.";
                return Redirect("~/BettorAccountInput/Index");
            }

            // Fetch InternalId
            var internalId = await _context.Bettors
                .AsNoTracking()
                .Where(b => b.Id == bettorId)
                .Select(b => b.InternalId)
                .FirstOrDefaultAsync();

            if (string.IsNullOrWhiteSpace(internalId))
            {
                TempData["ErrorMessage"] = "InternalId not found for the selected bettor.";
                return Redirect("~/BettorAccountInput/Index");
            }

            // Fetch UserExtra
            var userExtra = await _context.UserExtras.AsNoTracking().FirstOrDefaultAsync(ue => ue.UserId == userId);
            if (userExtra == null)
            {
                TempData["ErrorMessage"] = "UserExtra does not exist for the selected user.";
                return Redirect("~/BettorAccountInput/Index");
            }

            // Fetch User
            var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userExtra.UserId);
            if (user == null)
            {
                TempData["ErrorMessage"] = "User does not exist.";
                return Redirect("~/BettorAccountInput/Index");
            }

            // Create the request payload
            var linkBettorRequest = new LinkBettorRequest
            {
                UserName = userExtra.Username,
                InternalId = internalId
            };

            // Call the AccountController's method directly
            var result = await _accountController.LinkBettorToUser(linkBettorRequest) as IActionResult;

            if (result is OkObjectResult)
            {
                TempData["SuccessMessage"] = "Bettor successfully linked to the user.";
            }
            else if (result is BadRequestObjectResult badRequestResult)
            {
                TempData["ErrorMessage"] = $"Error: {badRequestResult.Value}";
            }
            else if (result is NotFoundObjectResult notFoundResult)
            {
                TempData["ErrorMessage"] = $"Error: {notFoundResult.Value}";
            }
            else
            {
                TempData["ErrorMessage"] = "An unexpected error occurred while linking the bettor.";
            }

            return Redirect("~/BettorAccountInput/Index");
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Refresh(string bettorId)
        {
            if (string.IsNullOrEmpty(bettorId))
            {
                return BadRequest("Bettor ID is required.");
            }

            try
            {
                // Fetch and save bettors (if needed)
                await _bettorService.FetchAndSaveBettorsAsync(_apiCompleteKey);

                // Fetch and save bettor accounts
                var bettorAccounts = await _bettorAccountService.FetchAndSaveBettorAccountsAsync(bettorId);

                // Fetch and save bet slips for the bettor
                var betSlips = await _betSlipService.FetchAndSaveBetSlipsByBettorAsync(bettorId, _apiCompleteKey);

                TempData["SuccessMessage"] = "Bettor accounts and related data refreshed successfully.";
                return Redirect("~/BettorAccountInput/Index");
            }
            catch (Exception ex)
            {
                // Log the error if necessary
                TempData["ErrorMessage"] = $"An error occurred while refreshing: {ex.Message}";
                return Redirect("~/BettorAccountInput/Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RefreshAll()
        {
            try
            {
                // Fetch bettors and save them to the database
                IEnumerable<Bettor> bettors;
                try
                {
                    bettors = await _bettorService.FetchAndSaveBettorsAsync(_apiCompleteKey);
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"An error occurred while fetching bettors: {ex.Message}";
                    return Redirect("~/BettorAccountInput/Index");
                }

                // Process each bettor
                foreach (var bettor in bettors)
                {
                    var bettorId = bettor.InternalId;

                    // Fetch and save bettor accounts
                    try
                    {
                        var bettorAccounts = await _bettorAccountService.FetchAndSaveBettorAccountsAsync(bettorId);
                    }
                    catch (Exception ex)
                    {
                        TempData["ErrorMessage"] = $"An error occurred while fetching and saving accounts for bettor {bettorId}: {ex.Message}";
                        continue; // Proceed with the next bettor
                    }

                    // Fetch and save bet slips for the bettor
                    try
                    {
                        var betSlips = await _betSlipService.FetchAndSaveBetSlipsByBettorAsync(bettorId, _apiCompleteKey);
                    }
                    catch (Exception ex)
                    {
                        TempData["ErrorMessage"] = $"An error occurred while fetching and saving bet slips for bettor {bettorId}: {ex.Message}";
                        continue; // Proceed with the next bettor
                    }
                }

                TempData["SuccessMessage"] = "All bettor accounts and related data refreshed successfully.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An unexpected error occurred: {ex.Message}";
            }

            return Redirect("~/BettorAccountInput/Index");
        }



        private async Task<List<SelectListItem>> GetUserListAsync()
        {
            var userExtras = await _context.UserExtras
                .Include(ue => ue.User)
                .Select(ue => new
                {
                    ue.UserId,
                    ue.Username
                })
                .ToListAsync();

            return userExtras
                .Select(ue => new SelectListItem
                {
                    Value = ue.UserId,
                    Text = ue.Username
                })
                .ToList();
        }



        private async Task<string> GetBettorInternalIdAsync(string bettorId)
        {
            var bettor = await _context.Bettors.AsNoTracking().FirstOrDefaultAsync(b => b.Id == bettorId);
            return bettor?.InternalId;
        }



        private bool BettorAccountExists(string id)
        {
            return _context.BettorAccounts.Any(e => e.Id == id);
        }
    }
}
