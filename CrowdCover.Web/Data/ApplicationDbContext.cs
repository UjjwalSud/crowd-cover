using CrowdCover.Web.Models; // Ensure this namespace includes your ApplicationUser model
using CrowdCover.Web.Models.Sharpsports; // Ensure this namespace includes your Bettor model
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace CrowdCover.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<DynamicDataVariable> DynamicDataVariables { get; set; }
        public DbSet<StreamingRoom> StreamingRooms { get; set; }
        public DbSet<Bettor> Bettors { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<BetSlip> BetSlips { get; set; }
        public DbSet<Bet> Bets { get; set; }
        public DbSet<BettorAccount> BettorAccounts { get; set; }

        public DbSet<StreamingRoomEvent> StreamingRoomEvents { get; set; }
        public DbSet<StreamingRoomBook> StreamingRoomBooks { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public DbSet<UserExtra> UserExtras { get; set; }

        public DbSet<Book> Books { get; set; } // Ensure Book entity is explicitly declared

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Bettor>().OwnsOne(b => b.Metadata);
            builder.Entity<Event>().OwnsOne(e => e.ContestantAway);
            builder.Entity<Event>().OwnsOne(e => e.ContestantHome);
            // builder.Entity<BetSlip>().OwnsOne(s => s.Book);
            builder.Entity<BetSlip>().OwnsOne(s => s.Adjusted);
            builder.Entity<BetSlip>().HasMany(s => s.Bets).WithOne();
            //builder.Entity<BettorAccount>().OwnsOne(b => b.Book);
            builder.Entity<BettorAccount>().OwnsOne(b => b.BookRegion);
            builder.Entity<BettorAccount>().OwnsOne(b => b.LatestRefreshResponse);
            builder.Entity<BettorAccount>().OwnsOne(b => b.Metadata);

            // Configure the relationship between ApplicationUser and Bettor
            //builder.Entity<IdentityUser>()
            //    .HasOne(u => u.Bettor)
            //    .WithOne()
            //    .HasForeignKey<IdentityUser>(u => u.BettorId)
            //    .IsRequired(false); // Make the relationship optional

            // Configure the foreign key relationship between Bet and Event
            builder.Entity<Bet>()
            .Property(b => b.EventId)
            .IsRequired(false); // Ensure EventId is nullable

            // Configure PropDetails as an owned entity of Bet
            builder.Entity<Bet>()
                .OwnsOne(b => b.PropDetails);  // Define PropDetails as an owned entity

            builder.Entity<BetSlip>()
        .OwnsOne(b => b.Adjusted);  // Define Adjustment as an owned entity of BetSlip

            builder.Entity<Event>()
       .Property(e => e.Id)
       .HasMaxLength(450); // Ensure consistency

            builder.Entity<Bet>()
                .Property(b => b.EventId)
                .HasMaxLength(450); // Ensure the same length as Event.Id


            // Many-to-many relationship configuration
            builder.Entity<StreamingRoomEvent>()
                .HasKey(sre => new { sre.StreamingRoomId, sre.EventId });

            builder.Entity<StreamingRoomEvent>()
                .HasOne(sre => sre.StreamingRoom)
                .WithMany(sr => sr.StreamingRoomEvents)
                .HasForeignKey(sre => sre.StreamingRoomId);

            builder.Entity<StreamingRoomEvent>()
                .HasOne(sre => sre.Event)
                .WithMany(e => e.StreamingRoomEvents)
                .HasForeignKey(sre => sre.EventId);

            // Unique constraint for Username
            builder.Entity<UserExtra>()
                .HasIndex(u => u.Username)
                .IsUnique();

            builder.Entity<UserExtra>()
             .HasOne(ue => ue.User)
             .WithOne()
             .HasForeignKey<UserExtra>(ue => ue.UserId)
             .OnDelete(DeleteBehavior.Cascade); // Remove IsRequired to allow UserExtra to be created later

            builder.Entity<Book>(entity =>
            {
                entity.HasKey(b => b.Id);
                entity.Property(b => b.Name).HasMaxLength(100);
                entity.Property(b => b.Abbr).HasMaxLength(50);
            });

            // Configure the nested object BetPlaceStatus as an owned entity
            builder.Entity<Book>()
                .OwnsOne(b => b.BetPlaceStatus, betPlaceStatus =>
                {
                    betPlaceStatus.Property(bps => bps.WebBrowser).HasMaxLength(50);
                    betPlaceStatus.Property(bps => bps.iOS).HasMaxLength(50);
                    betPlaceStatus.Property(bps => bps.Android).HasMaxLength(50);
                });

            builder.Entity<StreamingRoomBook>()
      .HasKey(srb => new { srb.StreamingRoomId, srb.BookId }); // Composite primary key

            builder.Entity<StreamingRoomBook>()
                .HasOne(srb => srb.StreamingRoom)
                .WithMany(sr => sr.StreamingRoomBooks)
                .HasForeignKey(srb => srb.StreamingRoomId);

            builder.Entity<StreamingRoomBook>()
                .HasOne(srb => srb.Book)
                .WithMany(b => b.StreamingRoomBooks)
                .HasForeignKey(srb => srb.BookId);

        }
    }
}
