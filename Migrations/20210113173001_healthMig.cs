using Microsoft.EntityFrameworkCore.Migrations;

namespace taleOfDungir.Migrations
{
    public partial class healthMig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte>(
                name: "ItemType",
                table: "Items",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<byte>(
                name: "ItemType",
                table: "ItemNames",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<long>(
                name: "MaxHealth",
                table: "Characters",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxHealth",
                table: "Characters");

            migrationBuilder.AlterColumn<int>(
                name: "ItemType",
                table: "Items",
                type: "integer",
                nullable: false,
                oldClrType: typeof(byte));

            migrationBuilder.AlterColumn<int>(
                name: "ItemType",
                table: "ItemNames",
                type: "integer",
                nullable: false,
                oldClrType: typeof(byte));
        }
    }
}
