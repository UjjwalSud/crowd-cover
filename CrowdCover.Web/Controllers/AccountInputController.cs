using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CrowdCover.Web.Data; // Ensure this is your DbContext namespace
using CrowdCover.Web.Models; // Ensure this is the correct namespace for your ApplicationUser model
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace CrowdCover.Web.Controllers
{
    [Authorize]
    //route
    public class AccountInputController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public AccountInputController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            return View(users);
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.Users
                // .Include(u => u.Bettor) // Assuming Bettor is a related entity
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // New action to handle Edit or Create UserExtra based on existence
        // In AccountInputController

        public async Task<IActionResult> EditOrCreateUserExtra(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Check if the UserExtra exists for this IdentityUser
            var userExtra = await _context.UserExtras.FirstOrDefaultAsync(ue => ue.UserId == id);

            if (userExtra != null)
            {
                // Redirect to the Edit action in the UserExtraController
                return RedirectToAction("Edit", "UserExtra", new { id = userExtra.Id });
            }
            else
            {
                // Redirect to the Create action in the UserExtraController, passing the selected user's Id
                return RedirectToAction("Create", "UserExtra", new { userId = id });
            }
        }



        // POST: Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,UserName,Email,PhoneNumber")] IdentityUser user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingUser = await _userManager.FindByIdAsync(id);
                    if (existingUser != null)
                    {
                        // Update user properties
                        existingUser.UserName = user.UserName;
                        existingUser.Email = user.Email;
                        existingUser.PhoneNumber = user.PhoneNumber;
                        //existingUser.BettorId = user.BettorId;

                        await _userManager.UpdateAsync(existingUser);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Redirect("~/AccountInput/Index");
            }

            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.Users
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    // Optionally, handle success logic (e.g., logging)
                }
                else
                {
                    // Handle failure to delete (optional)
                    ModelState.AddModelError("", "Failed to delete user.");
                    return View("Delete", user);
                }
            }
            else
            {
                return NotFound();
            }

            return Redirect("~/AccountInput/Index");
        }


        private bool UserExists(string id)
        {
            return _userManager.Users.Any(u => u.Id == id);
        }
    }
}
