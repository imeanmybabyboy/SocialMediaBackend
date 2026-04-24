using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SocialMediaBackend.Migrations
{
    /// <inheritdoc />
    public partial class racesTableChanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Interest",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Emoji = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Interest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Interest_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.UpdateData(
                table: "Races",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000001"),
                columns: new[] { "Name", "ThemeColorHex" },
                values: new object[] { "Elf", "#3bff5e" });

            migrationBuilder.UpdateData(
                table: "Races",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000002"),
                columns: new[] { "Name", "ThemeColorHex" },
                values: new object[] { "Dark Elf", "#7a3bff" });

            migrationBuilder.UpdateData(
                table: "Races",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000003"),
                columns: new[] { "Name", "ThemeColorHex" },
                values: new object[] { "Dwarf", "#b87333" });

            migrationBuilder.UpdateData(
                table: "Races",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000004"),
                columns: new[] { "Name", "ThemeColorHex" },
                values: new object[] { "Human", "#3b82ff" });

            migrationBuilder.UpdateData(
                table: "Races",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000005"),
                column: "ThemeColorHex",
                value: "#4b5320");

            migrationBuilder.UpdateData(
                table: "Races",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000006"),
                columns: new[] { "Name", "ThemeColorHex" },
                values: new object[] { "Vampire", "#8b0000" });

            migrationBuilder.UpdateData(
                table: "Races",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000007"),
                columns: new[] { "Name", "ThemeColorHex" },
                values: new object[] { "Werewolf", "#5c4033" });

            migrationBuilder.UpdateData(
                table: "Races",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000008"),
                columns: new[] { "Name", "ThemeColorHex" },
                values: new object[] { "Goblin", "#7fff00" });

            migrationBuilder.InsertData(
                table: "Races",
                columns: new[] { "Id", "Name", "ThemeColorHex" },
                values: new object[,]
                {
                    { new Guid("10000000-0000-0000-0000-000000000009"), "Troll", "#556b2f" },
                    { new Guid("10000000-0000-0000-0000-000000000010"), "Dragonborn", "#ff7a3b" },
                    { new Guid("10000000-0000-0000-0000-000000000011"), "Angel", "#ffd700" },
                    { new Guid("10000000-0000-0000-0000-000000000012"), "Demon", "#ff3b3b" },
                    { new Guid("10000000-0000-0000-0000-000000000013"), "Undead", "#aaaaaa" },
                    { new Guid("10000000-0000-0000-0000-000000000014"), "Fairy", "#ff69b4" },
                    { new Guid("10000000-0000-0000-0000-000000000015"), "Elemental", "#3bfff2" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Interest_UserId",
                table: "Interest",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Interest");

            migrationBuilder.DeleteData(
                table: "Races",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000009"));

            migrationBuilder.DeleteData(
                table: "Races",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000010"));

            migrationBuilder.DeleteData(
                table: "Races",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000011"));

            migrationBuilder.DeleteData(
                table: "Races",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000012"));

            migrationBuilder.DeleteData(
                table: "Races",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000013"));

            migrationBuilder.DeleteData(
                table: "Races",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000014"));

            migrationBuilder.DeleteData(
                table: "Races",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000015"));

            migrationBuilder.UpdateData(
                table: "Races",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000001"),
                columns: new[] { "Name", "ThemeColorHex" },
                values: new object[] { "Human", "#C0A080" });

            migrationBuilder.UpdateData(
                table: "Races",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000002"),
                columns: new[] { "Name", "ThemeColorHex" },
                values: new object[] { "Elf", "#7FFFD4" });

            migrationBuilder.UpdateData(
                table: "Races",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000003"),
                columns: new[] { "Name", "ThemeColorHex" },
                values: new object[] { "Dark Elf", "#4B0082" });

            migrationBuilder.UpdateData(
                table: "Races",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000004"),
                columns: new[] { "Name", "ThemeColorHex" },
                values: new object[] { "Dwarf", "#8B4513" });

            migrationBuilder.UpdateData(
                table: "Races",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000005"),
                column: "ThemeColorHex",
                value: "#556B2F");

            migrationBuilder.UpdateData(
                table: "Races",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000006"),
                columns: new[] { "Name", "ThemeColorHex" },
                values: new object[] { "Troll", "#2E8B57" });

            migrationBuilder.UpdateData(
                table: "Races",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000007"),
                columns: new[] { "Name", "ThemeColorHex" },
                values: new object[] { "Halfling", "#DEB887" });

            migrationBuilder.UpdateData(
                table: "Races",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000008"),
                columns: new[] { "Name", "ThemeColorHex" },
                values: new object[] { "Dragonborn", "#B22222" });
        }
    }
}
