using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMediaBackend.Migrations
{
    /// <inheritdoc />
    public partial class RaceModelAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Race_RaceId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Race",
                table: "Race");

            migrationBuilder.RenameTable(
                name: "Race",
                newName: "Races");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Races",
                table: "Races",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Races_RaceId",
                table: "Users",
                column: "RaceId",
                principalTable: "Races",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Races_RaceId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Races",
                table: "Races");

            migrationBuilder.RenameTable(
                name: "Races",
                newName: "Race");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Race",
                table: "Race",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Race_RaceId",
                table: "Users",
                column: "RaceId",
                principalTable: "Race",
                principalColumn: "Id");
        }
    }
}
