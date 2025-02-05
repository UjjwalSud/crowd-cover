using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CrowdCover.Web.Data;
using CrowdCover.Web.Models;
using CrowdCover.Web.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using CrowdCover.Web.Models.Sharpsports;

namespace CrowdCover.Web.Controllers
{

    [Authorize]
    public class StreamingRoomsInputController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StreamingRoomsInputController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> SearchEvents(string term)
        {
            var timeThreshold = DateTime.UtcNow.AddHours(-72);

            var events = await _context.Events
                .Where(e => e.StartTime.HasValue && (e.StartTime.Value >= timeThreshold || e.StartTime.Value >= DateTime.UtcNow) && e.Name.Contains(term))
                .OrderBy(e => e.StartTime)
                .Take(50)
                .Select(e => new { id = e.Id, text = e.Name })
                .ToListAsync();

            if (!events.Any())
            {
                return Json(new { message = "No events in timeframe" });
            }

            return Json(events);
        }



        // GET: StreamingRoomsInput
        // GET: StreamingRoomsInput
        public async Task<IActionResult> Index()
        {
            var streamingRooms = await _context.StreamingRooms
                .Include(sr => sr.StreamingRoomEvents)
                .ThenInclude(sre => sre.Event) // Include the related Event through the join table
                .Include(sr => sr.StreamingRoomBooks) // Include the StreamingRoomBooks join table
                .ThenInclude(srb => srb.Book) // Include the related Book through the join table
                .ToListAsync();

            return View(streamingRooms);
        }


        // GET: StreamingRoomsInput/Details/5
        // GET: StreamingRoomsInput/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var streamingRoom = await _context.StreamingRooms
                .Include(sr => sr.StreamingRoomBooks) // Include the StreamingRoomBooks join table
                .ThenInclude(srb => srb.Book) // Include the related Book through the join table
                .FirstOrDefaultAsync(m => m.Id == id);

            if (streamingRoom == null)
            {
                return NotFound();
            }

            return View(streamingRoom);
        }

        // GET: StreamingRoomsInput/Create
        public IActionResult Create()
        {
            return View();
        }


        // POST: StreamingRoomsInput/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,GameId,Name,Description,Slug,Active,WhenCreatedUTC,StartTimeUTC,EndTimeUTC")] StreamingRoom streamingRoom)
        {
            //if (ModelState.IsValid)
            //{
            streamingRoom.WhenCreatedUTC = DateTime.UtcNow;
            streamingRoom.Active = true;
            _context.Add(streamingRoom);
            await _context.SaveChangesAsync();

            // Retrieve all books and link them to the newly created streaming room
            var allBooks = await _context.Books.ToListAsync();
            streamingRoom.StreamingRoomBooks = new List<StreamingRoomBook>();
            foreach (var book in allBooks)
            {
                streamingRoom.StreamingRoomBooks.Add(new StreamingRoomBook
                {
                    StreamingRoomId = streamingRoom.Id,
                    BookId = book.Id // Assuming book has Id as the identifier
                });
            }

            // Save the changes to link the books to the streaming room
            await _context.SaveChangesAsync();

            return Redirect("~/StreamingRoomsInput/Index");
            //}
            //return View(streamingRoom);
        }


        // GET: StreamingRoomsInput/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var streamingRoom = await _context.StreamingRooms.FindAsync(id);
            if (streamingRoom == null)
            {
                return NotFound();
            }
            return View(streamingRoom);
        }

        // POST: StreamingRoomsInput/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,GameId,Name,Description,Slug,Active,WhenCreatedUTC,StartTimeUTC,EndTimeUTC")] StreamingRoom streamingRoom)
        {
            if (id != streamingRoom.Id)
            {
                return NotFound();
            }

            //if (ModelState.IsValid)
            //{
            try
            {
                _context.Update(streamingRoom);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StreamingRoomExists(streamingRoom.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Redirect("~/StreamingRoomsInput/Index");
        }

        // GET: StreamingRoomsInput/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var streamingRoom = await _context.StreamingRooms
                .FirstOrDefaultAsync(m => m.Id == id);
            if (streamingRoom == null)
            {
                return NotFound();
            }

            return View(streamingRoom);
        }

        // POST: StreamingRoomsInput/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var streamingRoom = await _context.StreamingRooms.FindAsync(id);
            if (streamingRoom != null)
            {
                _context.StreamingRooms.Remove(streamingRoom);
            }

            await _context.SaveChangesAsync();
            return Redirect("~/StreamingRoomsInput/Index");
        }

        private bool StreamingRoomExists(int id)
        {
            return _context.StreamingRooms.Any(e => e.Id == id);
        }

        // GET: StreamingRoomsInput/AddEventToRoom/5
        public IActionResult AddEventToRoom(int id)
        {
            // Calculate the time threshold (72 hours ago)
            var timeThreshold = DateTime.UtcNow.AddHours(-1000);

            // Get the events from the last 72 hours or future events
            var events = _context.Events
                //   .Where(e => e.StartTime.HasValue && (e.StartTime.Value >= timeThreshold || e.StartTime.Value >= DateTime.UtcNow))
                .OrderBy(e => e.StartTime)
                .ToList();

            ViewBag.Events = events ?? new List<Models.Sharpsports.Event>();

            var room = _context.StreamingRooms.Find(id);
            return View(room);
        }


        // GET: Confirmation for removing an event from a streaming room
        [HttpGet]
        [ActionName("ConfirmRemoveEventFromRoom")]
        public async Task<IActionResult> ConfirmRemoveEventFromRoom(int streamingRoomId, string eventId)
        {
            var streamingRoom = await _context.StreamingRooms
                .Include(sr => sr.StreamingRoomEvents)
                .ThenInclude(sre => sre.Event)
                .FirstOrDefaultAsync(sr => sr.Id == streamingRoomId);

            if (streamingRoom == null)
            {
                return NotFound();
            }

            var linkedEvent = streamingRoom.StreamingRoomEvents.FirstOrDefault(e => e.EventId == eventId);
            if (linkedEvent == null)
            {
                return NotFound("Event not found in this room.");
            }

            return View(new RemoveEventViewModel
            {
                StreamingRoomId = streamingRoomId,
                EventId = eventId,
                EventName = linkedEvent.Event.Name
            });
        }

        // POST: Actually remove the event from the room
        [HttpPost]
        [ActionName("RemoveEventFromRoom")]
        public async Task<IActionResult> RemoveEventFromRoom(RemoveEventViewModel model)
        {
            var streamingRoomEvent = await _context.StreamingRoomEvents
                .FirstOrDefaultAsync(sre => sre.StreamingRoomId == model.StreamingRoomId && sre.EventId == model.EventId);

            if (streamingRoomEvent != null)
            {
                _context.StreamingRoomEvents.Remove(streamingRoomEvent);
                await _context.SaveChangesAsync();
            }

            return Redirect("~/StreamingRoomsInput/Index");
        }





        // POST: StreamingRoomsInput/AddEventToRoom/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEventToRoom(int id, string eventId)
        {
            if (ModelState.IsValid)
            {
                var streamingRoom = await _context.StreamingRooms
                    .Include(sr => sr.StreamingRoomEvents)
                    .FirstOrDefaultAsync(sr => sr.Id == id);

                if (streamingRoom != null)
                {
                    var existingLink = streamingRoom.StreamingRoomEvents.FirstOrDefault(sre => sre.EventId == eventId);
                    if (existingLink == null)
                    {
                        streamingRoom.StreamingRoomEvents.Add(new StreamingRoomEvent
                        {
                            StreamingRoomId = id,
                            EventId = eventId
                        });

                        await _context.SaveChangesAsync();
                    }
                }
                return Redirect("~/StreamingRoomsInput/Index");
            }
            ViewData["EventId"] = new SelectList(_context.Events, "Id", "Name", eventId);
            return View();
        }

        // GET: StreamingRoomsInput/AddBookToRoom/5
        // GET: StreamingRoomsInput/AddBookToRoom/5
        public IActionResult AddBookToRoom(int id)
        {
            // Get the list of books available to add to the streaming room
            var books = _context.Books.OrderBy(b => b.Name).ToList();
            ViewBag.Books = books ?? new List<Book>();

            var room = _context.StreamingRooms.Find(id);
            return View(room);
        }


        // POST: StreamingRoomsInput/AddBookToRoom/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddBookToRoom(int id, string bookId)
        {
            if (ModelState.IsValid)
            {
                var streamingRoom = await _context.StreamingRooms
                    .Include(sr => sr.StreamingRoomBooks)
                    .FirstOrDefaultAsync(sr => sr.Id == id);

                if (streamingRoom != null)
                {
                    var existingLink = streamingRoom.StreamingRoomBooks.FirstOrDefault(srb => srb.BookId == bookId);
                    if (existingLink == null)
                    {
                        streamingRoom.StreamingRoomBooks.Add(new StreamingRoomBook
                        {
                            StreamingRoomId = id,
                            BookId = bookId
                        });

                        await _context.SaveChangesAsync();
                    }
                }
                return Redirect("~/StreamingRoomsInput/Index");
            }
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Name", bookId);
            return View();
        }

        // GET: StreamingRoomsInput/ConfirmRemoveBookFromRoom/5
        [HttpGet]
        [ActionName("ConfirmRemoveBookFromRoom")]
        public async Task<IActionResult> ConfirmRemoveBookFromRoom(int streamingRoomId, string bookId)
        {
            var streamingRoom = await _context.StreamingRooms
                .Include(sr => sr.StreamingRoomBooks)
                .ThenInclude(srb => srb.Book)
                .FirstOrDefaultAsync(sr => sr.Id == streamingRoomId);

            if (streamingRoom == null)
            {
                return NotFound();
            }

            var linkedBook = streamingRoom.StreamingRoomBooks.FirstOrDefault(b => b.BookId == bookId);
            if (linkedBook == null)
            {
                return NotFound("Book not found in this room.");
            }

            var model =
            new RemoveBookViewModel
            {
                StreamingRoomId = streamingRoomId,
                BookId = bookId,
                BookName = linkedBook.Book.Name
            };

            return View("~/Views/StreamingRoomsInput/ConfirmRemoveBookFromRoom.cshtml", model);

        }


        // POST: StreamingRoomsInput/RemoveBookFromRoom
        [HttpPost]
        [ActionName("RemoveBookFromRoom")]
        public async Task<IActionResult> RemoveBookFromRoom(RemoveBookViewModel model)
        {
            var streamingRoomBook = await _context.StreamingRoomBooks
                .FirstOrDefaultAsync(srb => srb.StreamingRoomId == model.StreamingRoomId && srb.BookId == model.BookId);

            if (streamingRoomBook != null)
            {
                _context.StreamingRoomBooks.Remove(streamingRoomBook);
                await _context.SaveChangesAsync();
            }

            return Redirect("~/StreamingRoomsInput/Index");
        }




    }

}

