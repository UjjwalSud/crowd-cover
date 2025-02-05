using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CrowdCover.Web.Models.Sharpsports
{
    public class BettorAccount
    {
        [Key]
        public string Id { get; set; }
        public string Bettor { get; set; }
        public Book Book { get; set; }
        public BookRegion BookRegion { get; set; }
        public bool Verified { get; set; }
        public bool Access { get; set; }
        public bool Paused { get; set; }
        public DateTime BetRefreshRequested { get; set; }
        public RefreshResponse LatestRefreshResponse { get; set; }
        public string LatestRefreshRequestId { get; set; }
        public decimal Balance { get; set; }
        public DateTime TimeCreated { get; set; }
        public int MissingBets { get; set; }
        public bool IsUnverifiable { get; set; }
        public bool RefreshInProgress { get; set; }
        public bool TFA { get; set; }
        public BettorAccountMetadata Metadata { get; set; }
    }


    public class BookRegion
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Abbr { get; set; }
        public string Status { get; set; }
        public string Country { get; set; }
    }

    public class RefreshResponse
    {
        public string Id { get; set; }
        public DateTime? TimeCreated { get; set; }
        public int? Status { get; set; }
        public string? Detail { get; set; }
        public string? RequestId { get; set; }
    }

    public class BettorAccountMetadata
    {
        public int? Handle { get; set; }
        public int? UnitSize { get; set; }
        public int? NetProfit { get; set; }
        public double? WinPercentage { get; set; }
        public double? WalletShare { get; set; }
    }
}
