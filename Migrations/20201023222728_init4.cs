using Microsoft.EntityFrameworkCore.Migrations;

namespace taleOfDungir.Migrations
{
    public partial class init4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Characters_CharacterId1",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_CharacterId1",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "CharacterId1",
                table: "Items");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CharacterId1",
                table: "Items",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Items_CharacterId1",
                table: "Items",
                column: "CharacterId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Characters_CharacterId1",
                table: "Items",
                column: "CharacterId1",
                principalTable: "Characters",
                principalColumn: "CharacterId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
