using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrowdCover.Web.Models.Sharpsports
{
    public class Bettor
    {
        [Key]
        public string Id { get; set; }

        public string InternalId { get; set; }
            
        public DateTime? BetRefreshRequested { get; set; }

        public DateTime? TimeCreated { get; set; }

        public Metadata Metadata { get; set; }

        // Link to the Identity User
        [ForeignKey("User")]
        public string? UserId { get; set; } // This links to the IdentityUser

        [NotMapped] // Optional: if you don't want to expose the user navigation in the database
        public virtual IdentityUser User { get; set; } // Navigation property
    }

    public class Metadata
    {
        public long? Handle { get; set; }

        public int? UnitSize { get; set; }

        public int? NetProfit { get; set; }

        public double? WinPercentage { get; set; }

        public int? TotalAccounts { get; set; }
    }
}
