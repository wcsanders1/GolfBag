using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GolfBag.Migrations
{
    public partial class Friday : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoundsOfGolf_Courses_CoursePlayedId",
                table: "RoundsOfGolf");

            migrationBuilder.DropIndex(
                name: "IX_RoundsOfGolf_CoursePlayedId",
                table: "RoundsOfGolf");

            migrationBuilder.DropColumn(
                name: "CoursePlayedId",
                table: "RoundsOfGolf");

            migrationBuilder.AddColumn<int>(
                name: "CourseId",
                table: "RoundsOfGolf",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "RoundsOfGolf");

            migrationBuilder.AddColumn<int>(
                name: "CoursePlayedId",
                table: "RoundsOfGolf",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RoundsOfGolf_CoursePlayedId",
                table: "RoundsOfGolf",
                column: "CoursePlayedId");

            migrationBuilder.AddForeignKey(
                name: "FK_RoundsOfGolf_Courses_CoursePlayedId",
                table: "RoundsOfGolf",
                column: "CoursePlayedId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
