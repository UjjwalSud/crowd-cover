using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CrowdCover.Web.Client;
using CrowdCover.Web.Data;
using CrowdCover.Web.Models.Sharpsports;
using Microsoft.EntityFrameworkCore;

namespace CrowdCover.Web.Services
{
    public class BettorAccountService
    {
        private readonly SharpSportsClient _sharpSportsClient;
        private readonly ApplicationDbContext _dbContext;

        public BettorAccountService(SharpSportsClient sharpSportsClient, ApplicationDbContext dbContext)
        {
            _sharpSportsClient = sharpSportsClient;
            _dbContext = dbContext;
        }


        public async Task<BettorAccountRefreshResponse> RefreshBettorAccountAsync(string bettorAccountId, string auth = null, string extensionVersion = null)
        {
            try
            {
                // Call the refresh endpoint
                var refreshResponse = await _sharpSportsClient.RefreshBettorAccountAsync(bettorAccountId, auth, extensionVersion);
                return refreshResponse;
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., log errors)
                throw new Exception($"An error occurred while refreshing the bettor account {bettorAccountId}: {ex.Message}", ex);
            }
        }

        private async Task<Book> ReuseOrUpdateBookAsync(Book book)
        {
            if (book == null) return null;

            // Check if the Book is already tracked locally
            var trackedBook = _dbContext.Books.Local.FirstOrDefault(b => b.Id == book.Id);
            if (trackedBook != null)
            {
                return trackedBook; // Reuse the tracked instance
            }

            // Check if the Book exists in the database
            var existingBook = await _dbContext.Books.AsNoTracking().FirstOrDefaultAsync(b => b.Id == book.Id);
            if (existingBook != null)
            {
                // Attach the existing Book to the context and return it
                _dbContext.Books.Attach(existingBook);
                return existingBook;
            }

            // Add the new Book to the context and return it
            _dbContext.Books.Add(book);
            return book;
        }



        public async Task<IEnumerable<BettorAccount>> FetchAndSaveBettorAccountsAsync(string bettorId)
        {
            try
            {
                // Fetch bettor accounts data from the API
                var bettorAccounts = await _sharpSportsClient.FetchBettorAccountsAsync(bettorId);

                if (bettorAccounts != null)
                {
                    foreach (var account in bettorAccounts)
                    {
                        // Handle the nested Book object
                        if (account.Book != null)
                        {
                            account.Book = await ReuseOrUpdateBookAsync(account.Book);
                        }

                        // Check if the bettor account already exists
                        var existingAccount = await _dbContext.BettorAccounts.FindAsync(account.Id);
                        if (existingAccount == null)
                        {
                            // Ensure `LatestRefreshRequestId` is set
                            if (account.LatestRefreshRequestId == null)
                            {
                                account.LatestRefreshRequestId = ""; // Set a default value
                            }

                            // Add the new bettor account
                            await _dbContext.BettorAccounts.AddAsync(account);
                        }
                        else
                        {
                            // Update the existing bettor account
                            _dbContext.Entry(existingAccount).CurrentValues.SetValues(account);

                            // Ensure `LatestRefreshRequestId` remains non-null
                            if (existingAccount.LatestRefreshRequestId == null)
                            {
                                existingAccount.LatestRefreshRequestId = "";
                            }
                        }
                    }

                    // Save all changes to the database
                    await _dbContext.SaveChangesAsync();
                }

                return bettorAccounts;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching and saving bettor accounts for bettor {bettorId}: {ex.Message}", ex);
            }
        }



    }
}
