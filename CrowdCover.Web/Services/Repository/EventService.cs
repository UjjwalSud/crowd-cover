using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CrowdCover.Web.Models.Sharpsports;
using CrowdCover.Web.Data;
using CrowdCover.Web.Client;
using Microsoft.EntityFrameworkCore;

namespace CrowdCover.Web.Services
{
    public class EventService
    {
        private readonly SharpSportsClient _sharpSportsClient;
        private readonly ApplicationDbContext _dbContext;

        public EventService(SharpSportsClient sharpSportsClient, ApplicationDbContext dbContext)
        {
            _sharpSportsClient = sharpSportsClient;
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Event>> FetchAndSaveEventsAsync(string apiKey)
        {
            try
            {
                // Fetch events data from the API
                var events = await _sharpSportsClient.FetchEventsAsync(apiKey);

                if (events != null)
                {
                    foreach (var evnt in events)
                    {
                        // Check if the event already exists
                        var existingEvent = await _dbContext.Events.FindAsync(evnt.Id);
                        if (existingEvent == null)
                        {
                            // Add new event to the database
                            await _dbContext.Events.AddAsync(evnt);
                        }
                        else
                        {
                            // Update the existing event with new data
                            _dbContext.Entry(existingEvent).CurrentValues.SetValues(evnt);
                        }
                    }

                    // Save all changes to the database
                    await _dbContext.SaveChangesAsync();
                }

                return events;
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., log errors)
                throw new Exception($"An error occurred while fetching and saving events: {ex.Message}", ex);
            }
        }
    }
}
