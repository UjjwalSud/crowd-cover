using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CrowdCover.Web.Data;
using CrowdCover.Web.Models.Sharpsports;
using Microsoft.AspNetCore.Authorization;

namespace CrowdCover.Web.Controllers
{
    [Authorize]
    public class BetSlipsInputController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BetSlipsInputController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BetSlips
        public async Task<IActionResult> Index()
        {
            var betSlips = await _context.BetSlips
              .Include(bs => bs.Book) // Ensure that the Book navigation property is loaded
              .ToListAsync();

            // Manually populate the Book for each BetSlip
            foreach (var betSlip in betSlips)
            {
                if (betSlip.Book != null && !string.IsNullOrEmpty(betSlip.Book.Id))
                {
                    // Fetch the Book associated with this BetSlip and manually set the Book property
                    var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == betSlip.Book.Id);
                    betSlip.Book = book;
                }
            }

            return View(betSlips);
            // return View(await _context.BetSlips.ToListAsync());
        }

        // GET: BetSlips/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var betSlip = await _context.BetSlips
                .FirstOrDefaultAsync(m => m.Id == id);
            if (betSlip == null)
            {
                return NotFound();
            }

            return View(betSlip);
        }

        // GET: BetSlips/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BetSlips/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Bettor,Book,BettorAccount,BookRef,TimePlaced,Type,Subtype,OddsAmerican,AtRisk,ToWin,Status,Outcome,RefreshResponse,Incomplete,NetProfit,DateClosed,TimeClosed,TypeSpecial")] BetSlip betSlip)
        {
            if (ModelState.IsValid)
            {
                _context.Add(betSlip);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(betSlip);
        }

        // GET: BetSlips/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var betSlip = await _context.BetSlips.FindAsync(id);
            if (betSlip == null)
            {
                return NotFound();
            }
            return View(betSlip);
        }

        // POST: BetSlips/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Bettor,Book,BettorAccount,BookRef,TimePlaced,Type,Subtype,OddsAmerican,AtRisk,ToWin,Status,Outcome,RefreshResponse,Incomplete,NetProfit,DateClosed,TimeClosed,TypeSpecial")] BetSlip betSlip)
        {
            if (id != betSlip.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(betSlip);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BetSlipExists(betSlip.Id))
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
            return View(betSlip);
        }

        // GET: BetSlips/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var betSlip = await _context.BetSlips
                .FirstOrDefaultAsync(m => m.Id == id);
            if (betSlip == null)
            {
                return NotFound();
            }

            return View(betSlip);
        }

        // POST: BetSlips/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var betSlip = await _context.BetSlips.FindAsync(id);
            if (betSlip != null)
            {
                _context.BetSlips.Remove(betSlip);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BetSlipExists(string id)
        {
            return _context.BetSlips.Any(e => e.Id == id);
        }
    }
}
