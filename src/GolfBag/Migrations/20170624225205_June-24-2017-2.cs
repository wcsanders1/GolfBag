using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GolfBag.Migrations
{
    public partial class June2420172 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "SlopeRating",
                table: "TeeBox",
                nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "SlopeRating",
                table: "TeeBox",
                nullable: false);
        }
    }
}
