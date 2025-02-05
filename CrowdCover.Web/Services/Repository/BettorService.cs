using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CrowdCover.Web.Models.Sharpsports;  // Replace with the actual namespace
using CrowdCover.Web.Data;               // Replace with the actual namespace for ApplicationDbContext
using CrowdCover.Web.Client;             // Replace with the actual namespace for SharpSportsClient
using Microsoft.EntityFrameworkCore;

namespace CrowdCover.Web.Services
{
    public interface IBettorService
    {
        Task<IEnumerable<Bettor>> FetchAndSaveBettorsAsync(string apiKey);
    }


    public class BettorService : IBettorService
    {
        private readonly SharpSportsClient _sharpSportsClient;
        private readonly ApplicationDbContext _dbContext;

        public BettorService(SharpSportsClient sharpSportsClient, ApplicationDbContext dbContext)
        {
            _sharpSportsClient = sharpSportsClient;
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Bettor>> FetchAndSaveBettorsAsync(string apiKey)
        {
            try
            {
                // Fetch bettors data from the API using SharpSportsClient
                var bettors = await _sharpSportsClient.FetchBettorsAsync(apiKey);

                if (bettors != null)
                {
                    foreach (var bettor in bettors)
                    {
                        // Check if the bettor already exists
                        var existingBettor = await _dbContext.Bettors.FindAsync(bettor.Id);
                        if (existingBettor == null)
                        {
                            // Add new bettor to the database
                            bettor.UserId = "";
                            await _dbContext.Bettors.AddAsync(bettor);
                        }
                        else
                        {
                            if (bettor.UserId == null)
                            {
                                bettor.UserId = "";
                            }
                            // Update the existing bettor with new data
                            _dbContext.Entry(existingBettor).CurrentValues.SetValues(bettor);
                        }
                    }

                    // Save all changes to the database
                    await _dbContext.SaveChangesAsync();
                }

                return bettors;
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., log errors)
                throw new Exception($"An error occurred while fetching and saving bettors: {ex.Message}", ex);
            }
        }
    }
}
