using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrowdCover.Web.Models.Sharpsports
{
    public class BetSlipPage
    {
        [JsonProperty("objects")]
        public List<BetSlip> Objects { get; set; }
    }

    public class BetSlip
    {
        [Key]
        public string Id { get; set; }
        public string Bettor { get; set; }

        public Book Book { get; set; }
        public string BettorAccount { get; set; }
        public string BookRef { get; set; }
        public DateTime? TimePlaced { get; set; }
        public string? Type { get; set; }
        public string? Subtype { get; set; }  // Nullable
        public int? OddsAmerican { get; set; }
        public decimal? AtRisk { get; set; }
        public decimal? ToWin { get; set; }
        public string? Status { get; set; }
        public string? Outcome { get; set; }
        public string? RefreshResponse { get; set; }
        public bool? Incomplete { get; set; }
        public decimal? NetProfit { get; set; }
        public DateTime? DateClosed { get; set; }
        public DateTime? TimeClosed { get; set; }  // Nullable
        public string? TypeSpecial { get; set; }  // Nullable

        public ICollection<Bet> Bets { get; set; }
        public Adjustment Adjusted { get; set; }
    }

    // Junction table for the Many-to-Many relationship between Event and Book
    public class StreamingRoomBook
    {
        // Primary key for the join table
        public int StreamingRoomId { get; set; }
        public StreamingRoom StreamingRoom { get; set; }

        public string BookId { get; set; }
        public Book Book { get; set; }

        // Additional fields for join table (if needed) can be added here
    }

    public class BetPlaceStatus
    {
        public string WebBrowser { get; set; }
        public string iOS { get; set; }
        public string Android { get; set; }
    }

    public class Book
    {
        [Key]
        public string Id { get; set; }
        public string? Name { get; set; }
        public string? Abbr { get; set; }
        public string? Status { get; set; }
        public bool RefreshCadenceActive { get; set; }
        public bool SdkRequired { get; set; }
        public DateTime? PullBackToDate { get; set; }
        public int? MaxHistoryMonths { get; set; }
        public int? MaxHistoryBets { get; set; }
        public string? HistoryDetail { get; set; }
        public bool MobileOnly { get; set; }

        // Nested object for bet place status
        public BetPlaceStatus BetPlaceStatus { get; set; }

        // Many-to-Many relationship with Events
        //  public ICollection<EventBook> EventBooks { get; set; }
        public ICollection<StreamingRoomBook> StreamingRoomBooks { get; set; }
    }

    public class Bet
    {
        [Key]
        public string Id { get; set; }
        public string? Type { get; set; }  // Nullable
        [ForeignKey("Event")]
        [StringLength(450)] // Ensure length matches the primary key of Events
        public string? EventId { get; set; }

        // Navigation property for Event
        public Event? Event { get; set; }  // Navigation property (optional)
        public string? Segment { get; set; }  // Nullable
        public string? Proposition { get; set; }  // Nullable
        public string? SegmentDetail { get; set; }  // Nullable
        public string? Position { get; set; }  // Nullable
        public double? Line { get; set; }  // Nullable
        public int? OddsAmerican { get; set; }  // Nullable
        public string? Status { get; set; }  // Nullable
        public string? Outcome { get; set; }  // Nullable
        public bool? Live { get; set; }  // Nullable
        public bool? Incomplete { get; set; }  // Nullable
        public string? BookDescription { get; set; }  // Nullable
        public string? MarketSelection { get; set; }  // Nullable
        public bool? AutoGrade { get; set; }  // Nullable
        public string? SegmentId { get; set; }  // Nullable
        public string? PositionId { get; set; }  // Nullable
        public PropDetails? PropDetails { get; set; }  // Nullable (assuming PropDetails class exists)
        public string? SdioMarketId { get; set; }  // Nullable
        public string? SportradarMarketId { get; set; }  // Nullable
        public string? OddsjamMarketId { get; set; }  // Nullable

        public string BetslipId { get; set; }
    }

    public class Adjustment
    {
        public bool Odds { get; set; }
        public bool? Line { get; set; }  // Nullable boolean to handle null values
        public decimal? AtRisk { get; set; }  // Nullable decimal to handle null values
    }



    public class Event
    {
        [Key]
        [StringLength(450)] // Ensure length matches the foreign key column
        public string Id { get; set; }
        public string? SportsdataioId { get; set; }  // Nullable
        public string? SportradarId { get; set; }  // Nullable
        public string? OddsjamId { get; set; }  // Nullable
        public string? TheOddsApiId { get; set; }  // Nullable
        public string? Sport { get; set; }
        public string? League { get; set; }
        public string? Name { get; set; }
        public string? NameSpecial { get; set; }  // Nullable
        public DateTime? StartTime { get; set; }
        public DateTime? StartDate { get; set; }
        public string? SportId { get; set; }
        public string? LeagueId { get; set; }
        public Contestant? ContestantAway { get; set; }  // Nullable contestant
        public Contestant? ContestantHome { get; set; }  // Nullable contestant
        public bool? NeutralVenue { get; set; }  // Nullable

        // Many-to-Many relationship with StreamingRoom
        public ICollection<StreamingRoomEvent> StreamingRoomEvents { get; set; }

        // Many-to-Many relationship with Books
        // public ICollection<StreamingRoomBook> EventBooks { get; set; }



    }

    public class Contestant
    {
        public string Id { get; set; }
        public string FullName { get; set; }
    }

    public class PropDetails
    {
        public bool Future { get; set; }
        public string? MatchupSpecial { get; set; }  // Nullable
        public string? Player { get; set; }  // Nullable
        public string? PlayerId { get; set; }  // Nullable
        public string? SportsdataioPlayerId { get; set; }  // Nullable
        public string? SportradarPlayerId { get; set; }  // Nullable
        public string? Team { get; set; }  // Nullable
        public string? TeamId { get; set; }  // Nullable
        public string? SportsdataioTeamId { get; set; }  // Nullable
        public string? SportradarTeamId { get; set; }  // Nullable
        public string? MetricSpecial { get; set; }  // Nullable
        public string? MetricSpecialId { get; set; }  // Nullable
    }
}


