using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace GolfBag.Migrations
{
    public partial class _12316 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Yardage",
                table: "CourseHole");

            migrationBuilder.CreateTable(
                name: "TeeBox",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CourseId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeeBox", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeeBox_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tee",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    HoleNumber = table.Column<int>(nullable: false),
                    TeeBoxId = table.Column<int>(nullable: true),
                    Yardage = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tee", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tee_TeeBox_TeeBoxId",
                        column: x => x.TeeBoxId,
                        principalTable: "TeeBox",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.AddColumn<int>(
                name: "TeeBoxPlayed",
                table: "RoundsOfGolf",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Handicap",
                table: "CourseHole",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Tee_TeeBoxId",
                table: "Tee",
                column: "TeeBoxId");

            migrationBuilder.CreateIndex(
                name: "IX_TeeBox_CourseId",
                table: "TeeBox",
                column: "CourseId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TeeBoxPlayed",
                table: "RoundsOfGolf");

            migrationBuilder.DropColumn(
                name: "Handicap",
                table: "CourseHole");

            migrationBuilder.DropTable(
                name: "Tee");

            migrationBuilder.DropTable(
                name: "TeeBox");

            migrationBuilder.AddColumn<int>(
                name: "Yardage",
                table: "CourseHole",
                nullable: false,
                defaultValue: 0);
        }
    }
}
