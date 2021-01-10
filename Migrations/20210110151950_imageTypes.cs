using Microsoft.EntityFrameworkCore.Migrations;

namespace taleOfDungir.Migrations
{
    public partial class imageTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ItemType",
                table: "ImageDBModels");

            migrationBuilder.AddColumn<byte>(
                name: "Type",
                table: "ImageDBModels",
                nullable: false,
                defaultValue: (byte)0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "ImageDBModels");

            migrationBuilder.AddColumn<int>(
                name: "ItemType",
                table: "ImageDBModels",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
