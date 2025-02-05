using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrowdCover.Web.Models.Sharpsports;
using CrowdCover.Web.Data;
using CrowdCover.Web.Client;
using Microsoft.EntityFrameworkCore;

namespace CrowdCover.Web.Services
{
    public class BetSlipService
    {
        private readonly SharpSportsClient _sharpSportsClient;
        private readonly ApplicationDbContext _dbContext;

        public BetSlipService(SharpSportsClient sharpSportsClient, ApplicationDbContext dbContext)
        {
            _sharpSportsClient = sharpSportsClient;
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<BetSlip>> FetchAndSaveBetSlipsByBettorAsync(string bettorId, string apiKey)
        {
            try
            {
                // Fetch bet slips from API
                var betSlips = await _sharpSportsClient.FetchBetSlipsByBettorAsync(bettorId, apiKey);

                if (betSlips == null || !betSlips.Any())
                    return betSlips;

                // Save or update events first
                await SaveOrUpdateEventsAsync(betSlips);

                // Save or update bet slips and bets
                await SaveOrUpdateBetSlipsAsync(betSlips);

                return betSlips;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching and saving bet slips for bettor {bettorId}: {ex.Message}", ex);
            }
        }

        private async Task SaveOrUpdateEventsAsync(IEnumerable<BetSlip> betSlips)
        {
            var events = betSlips.SelectMany(slip => slip.Bets)
                                 .Where(bet => bet.Event != null)
                                 .Select(bet => bet.Event)
                                 .DistinctBy(ev => ev.Id) // Ensure uniqueness by Id
                                 .ToList();

            foreach (var ev in events)
            {
                var trackedEvent = _dbContext.Events.Local.FirstOrDefault(e => e.Id == ev.Id);
                if (trackedEvent != null)
                {
                    // Update the tracked instance
                    _dbContext.Entry(trackedEvent).CurrentValues.SetValues(ev);
                }
                else
                {
                    // Fetch the existing Event from the database
                    var existingEvent = await _dbContext.Events.AsNoTracking().FirstOrDefaultAsync(e => e.Id == ev.Id);
                    if (existingEvent == null)
                    {
                        // Add new Event if it doesn't exist
                        _dbContext.Events.Add(ev);
                    }
                    else
                    {
                        // Attach and update the existing Event
                        _dbContext.Events.Attach(existingEvent);
                        _dbContext.Entry(existingEvent).CurrentValues.SetValues(ev);
                    }
                }
            }

            await _dbContext.SaveChangesAsync();
        }


        private async Task SaveOrUpdateBetSlipsAsync(IEnumerable<BetSlip> betSlips)
        {
            foreach (var slip in betSlips)
            {
                var existingSlip = await _dbContext.BetSlips
                    .Include(s => s.Bets)
                    .ThenInclude(b => b.Event)
                    .FirstOrDefaultAsync(s => s.Id == slip.Id);

                if (existingSlip == null)
                {
                    ReuseTrackedEntities(slip);
                    _dbContext.BetSlips.Add(slip);
                }
                else
                {
                    _dbContext.Entry(existingSlip).CurrentValues.SetValues(slip);
                    UpdateNestedBets(existingSlip, slip);
                }
            }

            await _dbContext.SaveChangesAsync();
        }
        private void ReuseTrackedEntities(BetSlip slip)
        {
            // Handle the Book entity
            if (slip.Book != null)
            {
                slip.Book = HandleBookEntity(slip.Book);
            }

            // Handle Bets and nested Events
            foreach (var bet in slip.Bets)
            {
                if (bet.Event != null)
                {
                    // Ensure only one instance of the Event is tracked
                    var trackedEvent = _dbContext.Events.Local.FirstOrDefault(e => e.Id == bet.Event.Id);
                    if (trackedEvent != null)
                    {
                        bet.Event = trackedEvent;
                    }
                    else
                    {
                        var existingEvent = _dbContext.Events.AsNoTracking().FirstOrDefault(e => e.Id == bet.Event.Id);
                        if (existingEvent != null)
                        {
                            _dbContext.Events.Attach(existingEvent);
                            bet.Event = existingEvent;
                        }
                    }
                }
            }
        }

        private Book HandleBookEntity(Book book)
        {
            if (book == null) return null;

            // Check if the Book is already tracked locally
            var trackedBook = _dbContext.Books.Local.FirstOrDefault(b => b.Id == book.Id);
            if (trackedBook != null)
            {
                return trackedBook; // Reuse the tracked instance
            }

            // Check if the Book exists in the database
            var existingBook = _dbContext.Books.AsNoTracking().FirstOrDefault(b => b.Id == book.Id);
            if (existingBook != null)
            {
                // Attach the existing Book and return it
                _dbContext.Books.Attach(existingBook);
                return existingBook;
            }

            // Return the new Book for insertion
            return book;
        }



        private void UpdateNestedBets(BetSlip existingSlip, BetSlip newSlip)
        {
            // Update existing bets
            foreach (var existingBet in existingSlip.Bets)
            {
                var newBet = newSlip.Bets.FirstOrDefault(b => b.Id == existingBet.Id);
                if (newBet != null)
                {
                    // Update scalar properties
                    _dbContext.Entry(existingBet).CurrentValues.SetValues(newBet);

                    // Handle associated Event
                    if (newBet.Event != null)
                    {
                        var trackedEvent = _dbContext.Events.Local.FirstOrDefault(e => e.Id == newBet.Event.Id);
                        if (trackedEvent != null)
                        {
                            existingBet.Event = trackedEvent;
                        }
                        else
                        {
                            var existingEvent = _dbContext.Events.AsNoTracking().FirstOrDefault(e => e.Id == newBet.Event.Id);
                            if (existingEvent != null)
                            {
                                _dbContext.Events.Attach(existingEvent);
                                existingBet.Event = existingEvent;
                            }
                            else
                            {
                                existingBet.Event = newBet.Event;
                            }
                        }
                    }
                }
            }

            // Add new bets
            var newBets = newSlip.Bets.Where(b => !existingSlip.Bets.Any(eb => eb.Id == b.Id)).ToList();
            foreach (var bet in newBets)
            {
                // Handle associated Event
                if (bet.Event != null)
                {
                    var trackedEvent = _dbContext.Events.Local.FirstOrDefault(e => e.Id == bet.Event.Id);
                    if (trackedEvent != null)
                    {
                        bet.Event = trackedEvent;
                    }
                    else
                    {
                        var existingEvent = _dbContext.Events.AsNoTracking().FirstOrDefault(e => e.Id == bet.Event.Id);
                        if (existingEvent != null)
                        {
                            _dbContext.Events.Attach(existingEvent);
                            bet.Event = existingEvent;
                        }
                    }
                }

                existingSlip.Bets.Add(bet);
            }
        }


        public async Task<IEnumerable<BetSlip>> FetchAndSaveBetSlipsAsync(string apiKey)
        {
            try
            {
                // Fetch bet slips from API
                var betSlips = await _sharpSportsClient.FetchBetSlipsAsync(apiKey);

                if (betSlips == null) return Enumerable.Empty<BetSlip>();

                // Track processed Book and Event IDs to avoid duplicates
                var processedBookIds = new HashSet<string>();
                var processedEventIds = new HashSet<string>();

                foreach (var slip in betSlips)
                {
                    // Handle Book
                    if (slip.Book != null)
                    {
                        if (!processedBookIds.Contains(slip.Book.Id))
                        {
                            var trackedBook = _dbContext.Books.Local.FirstOrDefault(b => b.Id == slip.Book.Id);
                            if (trackedBook != null)
                            {
                                slip.Book = trackedBook;
                            }
                            else
                            {
                                var existingBook = await _dbContext.Books.FirstOrDefaultAsync(b => b.Id == slip.Book.Id);
                                slip.Book = existingBook ?? slip.Book; // Use existing or new
                                if (existingBook == null)
                                {
                                    _dbContext.Books.Add(slip.Book);
                                }
                            }

                            processedBookIds.Add(slip.Book.Id);
                        }
                        else
                        {
                            slip.Book = null; // Already handled
                        }
                    }

                    // Handle Slip and Bets
                    var existingSlip = await _dbContext.BetSlips
                        .Include(s => s.Bets)
                        .FirstOrDefaultAsync(s => s.Id == slip.Id);

                    if (existingSlip == null)
                    {
                        foreach (var bet in slip.Bets)
                        {
                            // Handle Event
                            if (bet.Event != null)
                            {
                                if (!processedEventIds.Contains(bet.Event.Id))
                                {
                                    var trackedEvent = _dbContext.Events.Local.FirstOrDefault(e => e.Id == bet.Event.Id);
                                    if (trackedEvent != null)
                                    {
                                        bet.Event = trackedEvent;
                                    }
                                    else
                                    {
                                        var existingEvent = await _dbContext.Events.FirstOrDefaultAsync(e => e.Id == bet.Event.Id);
                                        bet.Event = existingEvent ?? bet.Event; // Use existing or new
                                        if (existingEvent == null)
                                        {
                                            _dbContext.Events.Add(bet.Event);
                                        }
                                    }

                                    processedEventIds.Add(bet.Event.Id);
                                }
                                else
                                {
                                    bet.Event = null; // Already handled
                                }
                            }

                            bet.BetslipId = slip.Id; // Assign the BetSlipId
                        }

                        // Add new BetSlip
                        _dbContext.BetSlips.Add(slip);
                    }
                    else
                    {
                        // Update existing Slip
                        _dbContext.Entry(existingSlip).CurrentValues.SetValues(slip);

                        foreach (var bet in slip.Bets)
                        {
                            var existingBet = existingSlip.Bets.FirstOrDefault(b => b.Id == bet.Id);
                            if (existingBet == null)
                            {
                                bet.BetslipId = slip.Id; // Assign the BetSlipId
                                existingSlip.Bets.Add(bet);
                            }
                            else
                            {
                                _dbContext.Entry(existingBet).CurrentValues.SetValues(bet);
                            }

                            // Handle Event
                            if (bet.Event != null)
                            {
                                if (!processedEventIds.Contains(bet.Event.Id))
                                {
                                    var trackedEvent = _dbContext.Events.Local.FirstOrDefault(e => e.Id == bet.Event.Id);
                                    if (trackedEvent != null)
                                    {
                                        bet.Event = trackedEvent;
                                    }
                                    else
                                    {
                                        var existingEvent = await _dbContext.Events.FirstOrDefaultAsync(e => e.Id == bet.Event.Id);
                                        bet.Event = existingEvent ?? bet.Event; // Use existing or new
                                        if (existingEvent == null)
                                        {
                                            _dbContext.Events.Add(bet.Event);
                                        }
                                    }

                                    processedEventIds.Add(bet.Event.Id);
                                }
                                else
                                {
                                    bet.Event = null; // Already handled
                                }
                            }
                        }
                    }
                }

                // Save all changes to the database
                await _dbContext.SaveChangesAsync();

                return betSlips;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching and saving bet slips: {ex.Message}", ex);
            }
        }

    }
}
