using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CrowdCover.Web.Data;
using CrowdCover.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using CrowdCover.Web.Models.ViewModels;
using CrowdCover.Web.Models.Sharpsports;
using Microsoft.AspNetCore.Authorization;

namespace CrowdCover.Web.Controllers
{
    [Route("UserExtraInput")]
    [Authorize]
    public class UserExtraInputController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public UserExtraInputController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: UserExtras
        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            var userExtras = await _context.UserExtras.Include(ue => ue.User).ToListAsync();
            return View(userExtras);
        }

        // GET: UserExtras/Details/5
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var userExtra = await _context.UserExtras
                .Include(ue => ue.User)
                .FirstOrDefaultAsync(ue => ue.Id == id);

            if (userExtra == null)
            {
                return NotFound();
            }

            return View(userExtra);
        }

        // GET: UserExtras/Create
        [HttpGet("Create")]
        public async Task<IActionResult> Create(string userId = null)
        {
            if (userId != null)
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return NotFound();
                }

                var model = new UserExtra
                {
                    UserId = userId,
                    Username = user.UserName
                };

                return View(model);
            }

            // Fetch the list of users to display in the dropdown
            var users = await _userManager.Users.ToListAsync();
            ViewBag.Users = new SelectList(users, "Id", "Email"); // Bind Id to UserId, and Email for display

            return View(new UserExtra());
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,Username,FirstName,LastName")] UserExtra userExtra)
        {
            if (!string.IsNullOrEmpty(userExtra.UserId))
            {
                // Check if the user exists
                var user = await _userManager.FindByIdAsync(userExtra.UserId);
                if (user == null)
                {
                    return NotFound();
                }

                // Check if the user already has a row in UserExtra
                var existingUserExtra = await _context.UserExtras.FirstOrDefaultAsync(ue => ue.UserId == userExtra.UserId);
                if (existingUserExtra != null)
                {
                    ModelState.AddModelError("UserId", String.Format("This user already has extra data -- {0} has the username {1}", user.Email, userExtra.Username));
                    return View(userExtra); // Return the view with the error message
                }

                // Check if the username is already taken in UserExtra
                var existingUsername = await _context.UserExtras.FirstOrDefaultAsync(ue => ue.Username == userExtra.Username);
                if (existingUsername != null)
                {
                    ModelState.AddModelError("Username", "This username is already taken.");
                    return View(userExtra); // Return the view with the error message
                }
            }

            // Directly save the UserExtra without checking ModelState validity
            _context.Add(userExtra);
            await _context.SaveChangesAsync();

            return Redirect("~/UserExtraInput/Index");
        }



        // GET: UserExtras/Edit/5
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var userExtra = await _context.UserExtras.Include(ue => ue.User).FirstOrDefaultAsync(ue => ue.Id == id);
            if (userExtra == null)
            {
                return NotFound();
            }

            return View(userExtra);
        }

        // POST: UserExtras/Edit/5
        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Username,FirstName,LastName")] UserExtra userExtra)
        {
            if (id != userExtra.Id)
            {
                return NotFound();
            }

            //if (ModelState.IsValid)
            //{
                try
                {
                    var existingUserExtra = await _context.UserExtras.Include(ue => ue.User).FirstOrDefaultAsync(ue => ue.Id == id);
                    if (existingUserExtra != null)
                    {
                        existingUserExtra.Username = userExtra.Username;
                        existingUserExtra.FirstName = userExtra.FirstName;
                        existingUserExtra.LastName = userExtra.LastName;

                        _context.Update(existingUserExtra);
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExtraExists(userExtra.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Redirect("~/UserExtraInput/Index");
            //}

            return View(userExtra);
        }

        // GET: UserExtras/Delete/5
        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var userExtra = await _context.UserExtras.Include(ue => ue.User).FirstOrDefaultAsync(ue => ue.Id == id);
            if (userExtra == null)
            {
                return NotFound();
            }

            return View(userExtra);
        }

        // POST: UserExtras/Delete/5
        [HttpPost("Delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userExtra = await _context.UserExtras.FindAsync(id);
            if (userExtra != null)
            {
                _context.UserExtras.Remove(userExtra);
                await _context.SaveChangesAsync();
            }

            return Redirect("~/UserExtraInput/Index");
        }

        // GET: UserExtras/ViewBetHistory/5
        [HttpGet("ViewBetHistory/{id}")]
        public async Task<IActionResult> ViewBetHistory(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            // Fetch the bet slips and bets linked to the specified bettor
            var betSlips = await _context.BetSlips
                .Include(bs => bs.Bets)
                .Where(bs => bs.Bettor == id)
                .ToListAsync();

            if (betSlips == null || betSlips.Count == 0)
            {
                ViewBag.Message = "No bet history found for this bettor.";
                return View(new List<BetSlip>()); // Return an empty list to the view if no data is found
            }

            return View(betSlips);
        }


        // POST: UserExtras/UnlinkBettor/5
        [HttpPost("UnlinkBettor/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UnlinkBettor(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var userExtra = await _context.UserExtras.FirstOrDefaultAsync(ue => ue.Id == id);
            if (userExtra == null)
            {
                return NotFound();
            }

            // Unlink the bettor by setting SharpsportBettorId to an empty string instead of null
            userExtra.SharpsportBettorId = string.Empty;
            _context.Update(userExtra);
            await _context.SaveChangesAsync();

            return Redirect("~/UserExtraInput/Index");
        }




        // Updated GET: UserExtras/LinkBettor/5
        [HttpGet("LinkBettor/{id}")]
        public async Task<IActionResult> LinkBettor(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var userExtra = await _context.UserExtras.Include(ue => ue.User).FirstOrDefaultAsync(ue => ue.Id == id);
            if (userExtra == null)
            {
                return NotFound();
            }

            // Fetch the list of Bettors to display in the dropdown
            var availableBettors = await _context.Bettors
                .Where(b => !_context.UserExtras.Any(ue => ue.SharpsportBettorId == b.Id)) // Only include Bettors not already linked
                .ToListAsync();

            // Create a view model to pass both the userExtra and the list of bettors to the view
            var model = new LinkBettorViewModel
            {
                UserExtra = userExtra,
                Bettors = availableBettors
            };

            return View(model);
        }

        // POST: UserExtras/LinkBettor/5
        [HttpPost("LinkBettor/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LinkBettor(int id, string selectedBettorId)
        {
            if (id == 0 || string.IsNullOrEmpty(selectedBettorId))
            {
                return NotFound();
            }

            var userExtra = await _context.UserExtras.Include(ue => ue.User).FirstOrDefaultAsync(ue => ue.Id == id);
            if (userExtra == null)
            {
                return NotFound();
            }

            // Ensure the selected Bettor is not already linked to a different UserExtra
            var linkedUserExtra = await _context.UserExtras.FirstOrDefaultAsync(ue => ue.SharpsportBettorId == selectedBettorId);
            if (linkedUserExtra != null)
            {
                ModelState.AddModelError("SharpsportBettorId", "This Bettor is already linked to another user.");
                return View(userExtra);
            }

            userExtra.SharpsportBettorId = selectedBettorId;
            _context.Update(userExtra);
            await _context.SaveChangesAsync();

            return Redirect("~/UserExtraInput/Index");
        }


        private bool UserExtraExists(int id)
        {
            return _context.UserExtras.Any(ue => ue.Id == id);
        }
    }
}
