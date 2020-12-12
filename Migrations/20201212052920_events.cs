using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace taleOfDungir.Migrations
{
    public partial class events : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Events",
                table: "Missions",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Events",
                table: "Missions");
        }
    }
}
