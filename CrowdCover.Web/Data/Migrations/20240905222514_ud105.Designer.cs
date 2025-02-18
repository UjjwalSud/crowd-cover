﻿// <auto-generated />
using System;
using CrowdCover.Web.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CrowdCover.Web.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240905222514_ud105")]
    partial class ud105
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CrowdCover.Web.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("BettorId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("BettorId")
                        .IsUnique()
                        .HasFilter("[BettorId] IS NOT NULL");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("CrowdCover.Web.Models.DynamicDataVariable", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("DynamicDataVariables");
                });

            modelBuilder.Entity("CrowdCover.Web.Models.Sharpsports.Bet", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool?>("AutoGrade")
                        .HasColumnType("bit");

                    b.Property<string>("BetSlipId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("BookDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EventId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool?>("Incomplete")
                        .HasColumnType("bit");

                    b.Property<double?>("Line")
                        .HasColumnType("float");

                    b.Property<bool?>("Live")
                        .HasColumnType("bit");

                    b.Property<string>("MarketSelection")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("OddsAmerican")
                        .HasColumnType("int");

                    b.Property<string>("OddsjamMarketId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Outcome")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Position")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PositionId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Proposition")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SdioMarketId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Segment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SegmentDetail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SegmentId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SportradarMarketId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("BetSlipId");

                    b.HasIndex("EventId");

                    b.ToTable("Bet");
                });

            modelBuilder.Entity("CrowdCover.Web.Models.Sharpsports.BetSlip", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<decimal?>("AtRisk")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Bettor")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BettorAccount")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BookRef")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DateClosed")
                        .HasColumnType("datetime2");

                    b.Property<bool?>("Incomplete")
                        .HasColumnType("bit");

                    b.Property<decimal?>("NetProfit")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int?>("OddsAmerican")
                        .HasColumnType("int");

                    b.Property<string>("Outcome")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RefreshResponse")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Subtype")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("TimeClosed")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("TimePlaced")
                        .HasColumnType("datetime2");

                    b.Property<decimal?>("ToWin")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TypeSpecial")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("BetSlips");
                });

            modelBuilder.Entity("CrowdCover.Web.Models.Sharpsports.Bettor", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime?>("BetRefreshRequested")
                        .HasColumnType("datetime2");

                    b.Property<string>("InternalId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("TimeCreated")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Bettors");
                });

            modelBuilder.Entity("CrowdCover.Web.Models.Sharpsports.BettorAccount", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("Access")
                        .HasColumnType("bit");

                    b.Property<decimal>("Balance")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("BetRefreshRequested")
                        .HasColumnType("datetime2");

                    b.Property<string>("Bettor")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsUnverifiable")
                        .HasColumnType("bit");

                    b.Property<string>("LatestRefreshRequestId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("MissingBets")
                        .HasColumnType("int");

                    b.Property<bool>("Paused")
                        .HasColumnType("bit");

                    b.Property<bool>("RefreshInProgress")
                        .HasColumnType("bit");

                    b.Property<bool>("TFA")
                        .HasColumnType("bit");

                    b.Property<DateTime>("TimeCreated")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Verified")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("BettorAccounts");
                });

            modelBuilder.Entity("CrowdCover.Web.Models.Sharpsports.Event", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("League")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LeagueId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NameSpecial")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("NeutralVenue")
                        .HasColumnType("bit");

                    b.Property<string>("OddsjamId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Sport")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SportId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SportradarId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SportsdataioId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("StartTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("TheOddsApiId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("CrowdCover.Web.Models.StreamingRoom", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("GameId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Slug")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("WhenCreatedUTC")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("StreamingRooms");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Name")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("CrowdCover.Web.Models.ApplicationUser", b =>
                {
                    b.HasOne("CrowdCover.Web.Models.Sharpsports.Bettor", "Bettor")
                        .WithOne()
                        .HasForeignKey("CrowdCover.Web.Models.ApplicationUser", "BettorId");

                    b.Navigation("Bettor");
                });

            modelBuilder.Entity("CrowdCover.Web.Models.Sharpsports.Bet", b =>
                {
                    b.HasOne("CrowdCover.Web.Models.Sharpsports.BetSlip", null)
                        .WithMany("Bets")
                        .HasForeignKey("BetSlipId");

                    b.HasOne("CrowdCover.Web.Models.Sharpsports.Event", "Event")
                        .WithMany()
                        .HasForeignKey("EventId");

                    b.OwnsOne("CrowdCover.Web.Models.Sharpsports.PropDetails", "PropDetails", b1 =>
                        {
                            b1.Property<string>("BetId")
                                .HasColumnType("nvarchar(450)");

                            b1.Property<bool>("Future")
                                .HasColumnType("bit");

                            b1.Property<string>("MatchupSpecial")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("MetricSpecial")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("MetricSpecialId")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Player")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("PlayerId")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("SportradarPlayerId")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("SportradarTeamId")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("SportsdataioPlayerId")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("SportsdataioTeamId")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Team")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("TeamId")
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("BetId");

                            b1.ToTable("Bet");

                            b1.WithOwner()
                                .HasForeignKey("BetId");
                        });

                    b.Navigation("Event");

                    b.Navigation("PropDetails");
                });

            modelBuilder.Entity("CrowdCover.Web.Models.Sharpsports.BetSlip", b =>
                {
                    b.OwnsOne("CrowdCover.Web.Models.Sharpsports.Adjustment", "Adjusted", b1 =>
                        {
                            b1.Property<string>("BetSlipId")
                                .HasColumnType("nvarchar(450)");

                            b1.Property<decimal?>("AtRisk")
                                .HasColumnType("decimal(18,2)");

                            b1.Property<bool?>("Line")
                                .HasColumnType("bit");

                            b1.Property<bool>("Odds")
                                .HasColumnType("bit");

                            b1.HasKey("BetSlipId");

                            b1.ToTable("BetSlips");

                            b1.WithOwner()
                                .HasForeignKey("BetSlipId");
                        });

                    b.OwnsOne("CrowdCover.Web.Models.Sharpsports.Book", "Book", b1 =>
                        {
                            b1.Property<string>("BetSlipId")
                                .HasColumnType("nvarchar(450)");

                            b1.Property<string>("Abbr")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Id")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Name")
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("BetSlipId");

                            b1.ToTable("BetSlips");

                            b1.WithOwner()
                                .HasForeignKey("BetSlipId");
                        });

                    b.Navigation("Adjusted")
                        .IsRequired();

                    b.Navigation("Book")
                        .IsRequired();
                });

            modelBuilder.Entity("CrowdCover.Web.Models.Sharpsports.Bettor", b =>
                {
                    b.OwnsOne("CrowdCover.Web.Models.Sharpsports.Metadata", "Metadata", b1 =>
                        {
                            b1.Property<string>("BettorId")
                                .HasColumnType("nvarchar(450)");

                            b1.Property<long>("Handle")
                                .HasColumnType("bigint");

                            b1.Property<int>("NetProfit")
                                .HasColumnType("int");

                            b1.Property<int>("TotalAccounts")
                                .HasColumnType("int");

                            b1.Property<int>("UnitSize")
                                .HasColumnType("int");

                            b1.Property<double>("WinPercentage")
                                .HasColumnType("float");

                            b1.HasKey("BettorId");

                            b1.ToTable("Bettors");

                            b1.WithOwner()
                                .HasForeignKey("BettorId");
                        });

                    b.Navigation("Metadata")
                        .IsRequired();
                });

            modelBuilder.Entity("CrowdCover.Web.Models.Sharpsports.BettorAccount", b =>
                {
                    b.OwnsOne("CrowdCover.Web.Models.Sharpsports.Book", "Book", b1 =>
                        {
                            b1.Property<string>("BettorAccountId")
                                .HasColumnType("nvarchar(450)");

                            b1.Property<string>("Abbr")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Id")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Name")
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("BettorAccountId");

                            b1.ToTable("BettorAccounts");

                            b1.WithOwner()
                                .HasForeignKey("BettorAccountId");
                        });

                    b.OwnsOne("CrowdCover.Web.Models.Sharpsports.BettorAccountMetadata", "Metadata", b1 =>
                        {
                            b1.Property<string>("BettorAccountId")
                                .HasColumnType("nvarchar(450)");

                            b1.Property<int?>("Handle")
                                .HasColumnType("int");

                            b1.Property<int?>("NetProfit")
                                .HasColumnType("int");

                            b1.Property<int?>("UnitSize")
                                .HasColumnType("int");

                            b1.Property<double?>("WalletShare")
                                .HasColumnType("float");

                            b1.Property<double?>("WinPercentage")
                                .HasColumnType("float");

                            b1.HasKey("BettorAccountId");

                            b1.ToTable("BettorAccounts");

                            b1.WithOwner()
                                .HasForeignKey("BettorAccountId");
                        });

                    b.OwnsOne("CrowdCover.Web.Models.Sharpsports.BookRegion", "BookRegion", b1 =>
                        {
                            b1.Property<string>("BettorAccountId")
                                .HasColumnType("nvarchar(450)");

                            b1.Property<string>("Abbr")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Country")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Id")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Name")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Status")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("BettorAccountId");

                            b1.ToTable("BettorAccounts");

                            b1.WithOwner()
                                .HasForeignKey("BettorAccountId");
                        });

                    b.OwnsOne("CrowdCover.Web.Models.Sharpsports.RefreshResponse", "LatestRefreshResponse", b1 =>
                        {
                            b1.Property<string>("BettorAccountId")
                                .HasColumnType("nvarchar(450)");

                            b1.Property<string>("Detail")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Id")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("RequestId")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<int?>("Status")
                                .HasColumnType("int");

                            b1.Property<DateTime?>("TimeCreated")
                                .HasColumnType("datetime2");

                            b1.HasKey("BettorAccountId");

                            b1.ToTable("BettorAccounts");

                            b1.WithOwner()
                                .HasForeignKey("BettorAccountId");
                        });

                    b.Navigation("Book")
                        .IsRequired();

                    b.Navigation("BookRegion")
                        .IsRequired();

                    b.Navigation("LatestRefreshResponse")
                        .IsRequired();

                    b.Navigation("Metadata")
                        .IsRequired();
                });

            modelBuilder.Entity("CrowdCover.Web.Models.Sharpsports.Event", b =>
                {
                    b.OwnsOne("CrowdCover.Web.Models.Sharpsports.Contestant", "ContestantAway", b1 =>
                        {
                            b1.Property<string>("EventId")
                                .HasColumnType("nvarchar(450)");

                            b1.Property<string>("FullName")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Id")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("EventId");

                            b1.ToTable("Events");

                            b1.WithOwner()
                                .HasForeignKey("EventId");
                        });

                    b.OwnsOne("CrowdCover.Web.Models.Sharpsports.Contestant", "ContestantHome", b1 =>
                        {
                            b1.Property<string>("EventId")
                                .HasColumnType("nvarchar(450)");

                            b1.Property<string>("FullName")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Id")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("EventId");

                            b1.ToTable("Events");

                            b1.WithOwner()
                                .HasForeignKey("EventId");
                        });

                    b.Navigation("ContestantAway");

                    b.Navigation("ContestantHome");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("CrowdCover.Web.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("CrowdCover.Web.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CrowdCover.Web.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("CrowdCover.Web.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CrowdCover.Web.Models.Sharpsports.BetSlip", b =>
                {
                    b.Navigation("Bets");
                });
#pragma warning restore 612, 618
        }
    }
}
