using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CrowdCover.Web.Data; // Ensure this is the correct namespace for your DbContext
using CrowdCover.Web.Models.Sharpsports;
using Microsoft.AspNetCore.Authorization;

namespace CrowdCover.Web.Controllers
{
    [Authorize]
    public class BettorsInputController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BettorsInputController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Bettors
        public async Task<IActionResult> Index()
        {
            return View(await _context.Bettors.ToListAsync());
        }

        // GET: Bettors/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bettor = await _context.Bettors
                .FirstOrDefaultAsync(m => m.Id == id);

            if (bettor == null)
            {
                return NotFound();
            }

            return View(bettor);
        }

        // GET: Bettors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Bettors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Email,PhoneNumber")] Bettor bettor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bettor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bettor);
        }

        // GET: Bettors/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bettor = await _context.Bettors.FindAsync(id);
            if (bettor == null)
            {
                return NotFound();
            }

            return View(bettor);
        }

        // POST: Bettors/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,Email,PhoneNumber")] Bettor bettor)
        {
            if (id != bettor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bettor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BettorExists(bettor.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(bettor);
        }

        // GET: Bettors/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bettor = await _context.Bettors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bettor == null)
            {
                return NotFound();
            }

            return View(bettor);
        }

        // POST: Bettors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var bettor = await _context.Bettors.FindAsync(id);
            if (bettor != null)
            {
                _context.Bettors.Remove(bettor);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool BettorExists(string id)
        {
            return _context.Bettors.Any(e => e.Id == id);
        }
    }
}
