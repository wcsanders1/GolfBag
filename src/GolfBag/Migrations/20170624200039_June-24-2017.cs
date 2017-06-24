using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GolfBag.Migrations
{
    public partial class June242017 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "CourseRating",
                table: "TeeBox",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "SlopeRating",
                table: "TeeBox",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CourseRating",
                table: "TeeBox");

            migrationBuilder.DropColumn(
                name: "SlopeRating",
                table: "TeeBox");
        }
    }
}
