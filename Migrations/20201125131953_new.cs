using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace taleOfDungir.Migrations
{
    public partial class @new : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Items");

            migrationBuilder.AddColumn<byte[]>(
                name: "Stats",
                table: "Items",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Stats",
                table: "Items");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Items",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
