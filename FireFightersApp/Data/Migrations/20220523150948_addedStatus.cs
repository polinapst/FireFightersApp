using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FireFightersApp.Data.Migrations
{
    public partial class addedStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "Call");

            migrationBuilder.AddColumn<string>(
                name: "Responsible",
                table: "Call",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Call",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Responsible",
                table: "Call");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Call");

            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "Call",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
