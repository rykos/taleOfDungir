using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace taleOfDungir.Migrations
{
    public partial class missions2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "StartTime",
                table: "Missions",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Started",
                table: "Missions",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "Missions");

            migrationBuilder.DropColumn(
                name: "Started",
                table: "Missions");
        }
    }
}
