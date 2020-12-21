using Microsoft.EntityFrameworkCore.Migrations;

namespace taleOfDungir.Migrations
{
    public partial class missionfinished : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Finished",
                table: "Missions",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Finished",
                table: "Missions");
        }
    }
}
