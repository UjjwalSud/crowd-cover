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
    public class BetsInputController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BetsInputController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Bets
        public async Task<IActionResult> Index()
        {
            return View(await _context.Bets
                .Include(b => b.Event)
                .ToListAsync());
        }

        // GET: Bets/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bet = await _context.Bets
                .Include(b => b.Event)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (bet == null)
            {
                return NotFound();
            }

            return View(bet);
        }

        // GET: Bets/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Bets/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Type,EventId,Segment,Proposition,SegmentDetail,Position,Line,OddsAmerican,Status,Outcome,Live,Incomplete,BookDescription,MarketSelection,AutoGrade,SegmentId,PositionId,SdioMarketId,SportradarMarketId,OddsjamMarketId")] Bet bet)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bet);
        }

        // GET: Bets/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bet = await _context.Bets.FindAsync(id);
            if (bet == null)
            {
                return NotFound();
            }

            return View(bet);
        }

        // POST: Bets/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Type,EventId,Segment,Proposition,SegmentDetail,Position,Line,OddsAmerican,Status,Outcome,Live,Incomplete,BookDescription,MarketSelection,AutoGrade,SegmentId,PositionId,SdioMarketId,SportradarMarketId,OddsjamMarketId")] Bet bet)
        {
            if (id != bet.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bet);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BetExists(bet.Id))
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
            return View(bet);
        }

        // GET: Bets/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bet = await _context.Bets
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bet == null)
            {
                return NotFound();
            }

            return View(bet);
        }

        // POST: Bets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var bet = await _context.Bets.FindAsync(id);
            if (bet != null)
            {
                _context.Bets.Remove(bet);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool BetExists(string id)
        {
            return _context.Bets.Any(e => e.Id == id);
        }
    }
}
