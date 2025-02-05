using System;
using System.Collections.Generic;

namespace CrowdCover.Web.Models.Sharpsports
{
    public class BettorAccountRefreshResponse
    {
        public DateTime BetRefreshRequested { get; set; }
        public List<string> Refresh { get; set; }
        public List<List<string>> Array { get; set; }
        public List<string> Unverified { get; set; }
        public List<string> IsUnverifiable { get; set; }
        public List<string> BookInactive { get; set; }
        public List<string> BookRegionInactive { get; set; }
        public List<string> RateLimited { get; set; }
        public List<string> OtpRequired { get; set; }
        public List<string> AuthParameterRequired { get; set; }
        public List<string> ExtensionUpdateRequired { get; set; }
        public string RequestId { get; set; }
        public string Cid { get; set; }
        public string ExtensionDownloadUrl { get; set; }
    }
}
