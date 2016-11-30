using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace GolfBag.Migrations
{
    public partial class Nov29 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScoreCards");

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "RoundsOfGolf",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "RoundsOfGolf",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comment",
                table: "RoundsOfGolf");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "RoundsOfGolf");

            migrationBuilder.CreateTable(
                name: "ScoreCards",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Course = table.Column<int>(nullable: false),
                    CourseName = table.Column<string>(nullable: true),
                    PlayerName = table.Column<string>(maxLength: 80, nullable: false),
                    Scores = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScoreCards", x => x.Id);
                });
        }
    }
}
