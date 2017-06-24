using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using GolfBag.Entities;

namespace GolfBag.Migrations
{
    [DbContext(typeof(ScoreCardDbContext))]
    [Migration("20170624225205_June-24-2017-2")]
    partial class June2420172
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.3")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("GolfBag.Entities.Course", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CourseName");

                    b.Property<int>("NumberOfHoles");

                    b.Property<string>("PlayerId");

                    b.Property<string>("PlayerName");

                    b.HasKey("Id");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("GolfBag.Entities.CourseHole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("CourseId");

                    b.Property<int>("Handicap");

                    b.Property<int>("HoleNumber");

                    b.Property<int>("Par");

                    b.HasKey("Id");

                    b.HasIndex("CourseId");

                    b.ToTable("CourseHole");
                });

            modelBuilder.Entity("GolfBag.Entities.RoundOfGolf", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Comment");

                    b.Property<int>("CourseId");

                    b.Property<DateTime>("Date");

                    b.Property<string>("PlayerId");

                    b.Property<string>("PlayerName");

                    b.Property<int>("TeeBoxPlayed");

                    b.HasKey("Id");

                    b.ToTable("RoundsOfGolf");
                });

            modelBuilder.Entity("GolfBag.Entities.Score", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("HoleNumber");

                    b.Property<int>("HolePutt");

                    b.Property<int>("HoleScore");

                    b.Property<int?>("RoundOfGolfId");

                    b.HasKey("Id");

                    b.HasIndex("RoundOfGolfId");

                    b.ToTable("Score");
                });

            modelBuilder.Entity("GolfBag.Entities.Tee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("HoleNumber");

                    b.Property<int?>("TeeBoxId");

                    b.Property<int>("Yardage");

                    b.HasKey("Id");

                    b.HasIndex("TeeBoxId");

                    b.ToTable("Tee");
                });

            modelBuilder.Entity("GolfBag.Entities.TeeBox", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("CourseId");

                    b.Property<decimal>("CourseRating");

                    b.Property<string>("Name");

                    b.Property<int>("SlopeRating");

                    b.HasKey("Id");

                    b.HasIndex("CourseId");

                    b.ToTable("TeeBox");
                });

            modelBuilder.Entity("GolfBag.Entities.User", b =>
                {
                    b.Property<string>("Id");

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole", b =>
                {
                    b.Property<string>("Id");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("GolfBag.Entities.CourseHole", b =>
                {
                    b.HasOne("GolfBag.Entities.Course")
                        .WithMany("CourseHoles")
                        .HasForeignKey("CourseId");
                });

            modelBuilder.Entity("GolfBag.Entities.Score", b =>
                {
                    b.HasOne("GolfBag.Entities.RoundOfGolf")
                        .WithMany("Scores")
                        .HasForeignKey("RoundOfGolfId");
                });

            modelBuilder.Entity("GolfBag.Entities.Tee", b =>
                {
                    b.HasOne("GolfBag.Entities.TeeBox")
                        .WithMany("Tees")
                        .HasForeignKey("TeeBoxId");
                });

            modelBuilder.Entity("GolfBag.Entities.TeeBox", b =>
                {
                    b.HasOne("GolfBag.Entities.Course")
                        .WithMany("TeeBoxes")
                        .HasForeignKey("CourseId");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Claims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("GolfBag.Entities.User")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("GolfBag.Entities.User")
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("GolfBag.Entities.User")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
