using CrowdCover.Web.Models.Sharpsports;

namespace CrowdCover.Web.Models.ViewModels
{
    public class UserBettorViewModel
    {
        public string BettorId { get; set; }
        public string InternalId { get; set; }
        public Metadata Metadata { get; set; }
        public List<BettorAccountViewModel> BettorAccounts { get; set; }
    }

    public class BettorAccountViewModel
    {
        public string Id { get; set; }
        public string BettorId { get; set; }
        public Book Book { get; set; }
        public BookRegion BookRegion { get; set; }
        public bool Verified { get; set; }
        public bool Access { get; set; }
        public bool Paused { get; set; }
        public DateTime BetRefreshRequested { get; set; }
        public RefreshResponse LatestRefreshResponse { get; set; }
        public decimal Balance { get; set; }
        public DateTime TimeCreated { get; set; }
        public int MissingBets { get; set; }
        public bool IsUnverifiable { get; set; }
        public bool RefreshInProgress { get; set; }
        public bool TFA { get; set; }
        public BettorAccountMetadata Metadata { get; set; }
    }

}
