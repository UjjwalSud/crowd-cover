using CrowdCover.Web.Data;
using CrowdCover.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace CrowdCover.Web.Services
{
    public class RoomAccessService
    {
        private readonly ApplicationDbContext _context;

        public RoomAccessService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<StreamingRoom>> GetAccessibleRoomsForUser(string userId)
        {
            // Fetch bettors linked to the user
            var bettors = await _context.Bettors
                .AsNoTracking()
                .Where(b => b.UserId == userId)
                .Select(b => b.Id)
                .ToListAsync();

            if (!bettors.Any())
            {
                return new List<StreamingRoom>();
            }

            // Get all bettor accounts linked to these bettors
            var bettorAccounts = await _context.BettorAccounts
                .AsNoTracking()
                .Where(ba => bettors.Contains(ba.Bettor))
                .Select(ba => ba.Id)
                .ToListAsync();

            if (!bettorAccounts.Any())
            {
                return new List<StreamingRoom>();
            }

            // Get all bet slips linked to the bettor accounts
            var betSlips = await _context.BetSlips
                .AsNoTracking()
                .Where(bs => bettorAccounts.Contains(bs.BettorAccount))
                .SelectMany(bs => bs.Bets.Select(b => b.EventId))
                .Distinct()
                .ToListAsync();

            if (!betSlips.Any())
            {
                return new List<StreamingRoom>();
            }

            // Fetch all streaming rooms linked to the events from the bet slips
            var accessibleRooms = await _context.StreamingRoomEvents
                .AsNoTracking()
                .Where(sre => betSlips.Contains(sre.EventId))
                .Include(sre => sre.StreamingRoom)
                .Select(sre => sre.StreamingRoom)
                .Distinct()
                .ToListAsync();

            return accessibleRooms;
        }
    }

}
