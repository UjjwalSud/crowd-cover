using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CrowdCover.Web.Data; // Ensure this is the correct namespace for your DbContext
using CrowdCover.Web.Models.Sharpsports;
using Microsoft.AspNetCore.Authorization;
using CrowdCover.Web.Client;

namespace CrowdCover.Web.Controllers
{
    [Authorize]
    public class BooksInputController : Controller
    {
        private readonly ApplicationDbContext _context;
        SharpSportsClient _sharpSportsClient;

        public BooksInputController(ApplicationDbContext context, SharpSportsClient sharpSportsClient)
        {
            _context = context;
            _sharpSportsClient = sharpSportsClient;
        }

        // GET: Books
        public async Task<IActionResult> Index()
        {
            return View(await _context.Books.ToListAsync());
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return Redirect("~/BooksInput/Index");
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Books/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Abbr,Status,RefreshCadenceActive,SdkRequired,PullBackToDate,MaxHistoryMonths,MaxHistoryBets,HistoryDetail,MobileOnly")] Book book)
        {
            if (ModelState.IsValid)
            {
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return Redirect("~/BooksInput/Index");
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,Abbr,Status,RefreshCadenceActive,SdkRequired,PullBackToDate,MaxHistoryMonths,MaxHistoryBets,HistoryDetail,MobileOnly")] Book book)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.Id))
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
            return Redirect("~/BooksInput/Index");
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
            }

            return Redirect("~/BooksInput/Index");
        }

        // RefreshBooks: Fetch books from the API and save to the database
        public async Task<IActionResult> RefreshBooks()
        {
            try
            {
                // Fetch books from the API
                var books = await _sharpSportsClient.FetchBooksAsync();

                foreach (var book in books)
                {
                    // Check if the book already exists in the database
                    var existingBook = await _context.Books.FirstOrDefaultAsync(b => b.Id == book.Id);
                    if (existingBook == null)
                    {
                        // Add new book
                        await _context.Books.AddAsync(book);
                    }
                    else
                    {
                        // Update existing book details
                        existingBook.Name = book.Name;
                        existingBook.Abbr = book.Abbr;
                        existingBook.Status = book.Status;
                        existingBook.RefreshCadenceActive = book.RefreshCadenceActive;
                        existingBook.SdkRequired = book.SdkRequired;
                        existingBook.PullBackToDate = book.PullBackToDate;
                        existingBook.MaxHistoryMonths = book.MaxHistoryMonths;
                        existingBook.MaxHistoryBets = book.MaxHistoryBets;
                        existingBook.HistoryDetail = book.HistoryDetail;
                        existingBook.MobileOnly = book.MobileOnly;

                        _context.Books.Update(existingBook);
                    }
                }

                // Save changes to the database
                await _context.SaveChangesAsync();
                return Redirect("~/BooksInput/Index");
            }
            catch (Exception ex)
            {
                // Log or handle the error appropriately
                Console.WriteLine($"Error refreshing books: {ex.Message}");
                return BadRequest("An error occurred while refreshing books.");
            }
        }

        private bool BookExists(string id)
        {
            return _context.Books.Any(e => e.Id == id);
        }
    }
}
