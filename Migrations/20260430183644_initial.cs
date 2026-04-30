using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SocialMediaBackend.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Interests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Emoji = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Interests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Races",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ThemeColorHex = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Races", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RaceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Login = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Nickname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Bio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastLoginAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Salt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RegisteredAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Races_RaceId",
                        column: x => x.RaceId,
                        principalTable: "Races",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Users_UserRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "UserRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RaceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Bio = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LikesQnt = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    SharesQnt = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Posts_Races_RaceId",
                        column: x => x.RaceId,
                        principalTable: "Races",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Posts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "UsersInterests",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InterestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersInterests", x => new { x.UserId, x.InterestId });
                    table.ForeignKey(
                        name: "FK_UsersInterests_Interests_InterestId",
                        column: x => x.InterestId,
                        principalTable: "Interests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsersInterests_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PostId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Bio = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LikesQnt = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsEdited = table.Column<bool>(type: "bit", nullable: false),
                    EditedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Comments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "PostsInterests",
                columns: table => new
                {
                    PostId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InterestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostsInterests", x => new { x.PostId, x.InterestId });
                    table.ForeignKey(
                        name: "FK_PostsInterests_Interests_InterestId",
                        column: x => x.InterestId,
                        principalTable: "Interests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostsInterests_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Interests",
                columns: new[] { "Id", "Color", "Emoji", "Name" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), "#3b82ff", "⚽", "Sports" },
                    { new Guid("12121212-1212-1212-1212-121212121212"), "#16a34a", "🌿", "Nature" },
                    { new Guid("13131313-1313-1313-1313-131313131313"), "#7c3aed", "🧠", "Psychology" },
                    { new Guid("14141414-1414-1414-1414-141414141414"), "#64748b", "💼", "Business" },
                    { new Guid("15151515-1515-1515-1515-151515151515"), "#dc2626", "🚗", "Cars" },
                    { new Guid("16161616-1616-1616-1616-161616161616"), "#002f9dff", "🚀", "Space" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), "#8b5cf6", "📚", "Books" },
                    { new Guid("33333333-3333-3333-3333-333333333333"), "#ff3b3b", "🎬", "Movies" },
                    { new Guid("44444444-4444-4444-4444-444444444444"), "#ff7a3b", "🎵", "Music" },
                    { new Guid("55555555-5555-5555-5555-555555555555"), "#22c55e", "🎮", "Gaming" },
                    { new Guid("66666666-6666-6666-6666-666666666666"), "#06b6d4", "💻", "Coding" },
                    { new Guid("77777777-7777-7777-7777-777777777777"), "#facc15", "✈️", "Travel" },
                    { new Guid("88888888-8888-8888-8888-888888888888"), "#ef4444", "🏋️", "Fitness" },
                    { new Guid("99999999-9999-9999-9999-999999999999"), "#f472b6", "🍥", "Anime" },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "#0ea5e9", "🧠", "Technology" },
                    { new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), "#a855f7", "🎨", "Art" },
                    { new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), "#14b8a6", "📸", "Photography" },
                    { new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"), "#6366f1", "🔬", "Science" },
                    { new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), "#b45309", "🏺", "History" },
                    { new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"), "#f97316", "🍳", "Cooking" }
                });

            migrationBuilder.InsertData(
                table: "Races",
                columns: new[] { "Id", "Name", "ThemeColorHex" },
                values: new object[,]
                {
                    { new Guid("10000000-0000-0000-0000-000000000001"), "Elf", "#3bff5e" },
                    { new Guid("10000000-0000-0000-0000-000000000002"), "Dark Elf", "#7a3bff" },
                    { new Guid("10000000-0000-0000-0000-000000000003"), "Dwarf", "#b87333" },
                    { new Guid("10000000-0000-0000-0000-000000000004"), "Human", "#3b82ff" },
                    { new Guid("10000000-0000-0000-0000-000000000005"), "Orc", "#4b5320" },
                    { new Guid("10000000-0000-0000-0000-000000000006"), "Vampire", "#8b0000" },
                    { new Guid("10000000-0000-0000-0000-000000000007"), "Werewolf", "#5c4033" },
                    { new Guid("10000000-0000-0000-0000-000000000008"), "Goblin", "#7fff00" },
                    { new Guid("10000000-0000-0000-0000-000000000009"), "Troll", "#556b2f" },
                    { new Guid("10000000-0000-0000-0000-000000000010"), "Dragonborn", "#ff7a3b" },
                    { new Guid("10000000-0000-0000-0000-000000000011"), "Angel", "#ffd700" },
                    { new Guid("10000000-0000-0000-0000-000000000012"), "Demon", "#ff3b3b" },
                    { new Guid("10000000-0000-0000-0000-000000000013"), "Undead", "#aaaaaa" },
                    { new Guid("10000000-0000-0000-0000-000000000014"), "Fairy", "#ff69b4" },
                    { new Guid("10000000-0000-0000-0000-000000000015"), "Elemental", "#3bfff2" }
                });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "Id", "Description", "Title" },
                values: new object[,]
                {
                    { new Guid("a3f2e1b4-9c5d-4f2a-8d7a-1b2c3d4e5f6a"), "Administrator role", "Admin" },
                    { new Guid("b4c5d6e7-8f9a-4b1c-9d2e-3f4a5b6c7d8e"), "Regular user role", "User" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Bio", "DeletedAt", "Email", "ImageUrl", "LastLoginAt", "Login", "Nickname", "PasswordHash", "RaceId", "RegisteredAt", "RoleId", "Salt" },
                values: new object[,]
                {
                    { new Guid("01234567-89ab-4cde-9f01-234567890abc"), null, null, "SOFIA@EXAMPLE.COM", null, null, "sofia", "SofiaLight", "9A6A64B2E3004012", new Guid("10000000-0000-0000-0000-000000000005"), new DateTime(2024, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("b4c5d6e7-8f9a-4b1c-9d2e-3f4a5b6c7d8e"), "01234567" },
                    { new Guid("12345678-9abc-4def-9012-345678901bcd"), null, null, "MIKE@EXAMPLE.COM", null, null, "mike", "MikeWave", "75C1721BEB275FCA", new Guid("10000000-0000-0000-0000-000000000006"), new DateTime(2024, 1, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("b4c5d6e7-8f9a-4b1c-9d2e-3f4a5b6c7d8e"), "12345678" },
                    { new Guid("23456789-abcd-4ef0-9123-45678901cdef"), null, null, "OLGA@EXAMPLE.COM", null, null, "olga", "OlgaStar", "3C61FA60FC810A96", new Guid("10000000-0000-0000-0000-000000000007"), new DateTime(2024, 1, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("b4c5d6e7-8f9a-4b1c-9d2e-3f4a5b6c7d8e"), "23456789" },
                    { new Guid("3456789a-bcde-4f01-9234-5678901def01"), null, null, "IVAN@EXAMPLE.COM", null, null, "ivan", "IvanRock", "16B2C8B41DFB08CF", new Guid("10000000-0000-0000-0000-000000000008"), new DateTime(2024, 1, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("b4c5d6e7-8f9a-4b1c-9d2e-3f4a5b6c7d8e"), "3456789A" },
                    { new Guid("c1d2e3f4-5678-4abc-9def-1234567890ab"), null, null, "ADMIN@EXAMPLE.COM", null, null, "admin", "SuperAdmin", "FA7C65CB4BB64C06", new Guid("10000000-0000-0000-0000-000000000001"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("a3f2e1b4-9c5d-4f2a-8d7a-1b2c3d4e5f6a"), "C1D2E3F4" },
                    { new Guid("d2e3f4a5-6789-4bcd-8efa-2345678901bc"), null, null, "JOHN@EXAMPLE.COM", null, null, "john", "JohnDoe", "6D8208F48DDA7332", new Guid("10000000-0000-0000-0000-000000000002"), new DateTime(2024, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("b4c5d6e7-8f9a-4b1c-9d2e-3f4a5b6c7d8e"), "D2E3F4A5" },
                    { new Guid("e3f4a5b6-7890-4cde-9fab-3456789012cd"), null, null, "MARIA@EXAMPLE.COM", null, null, "maria", "MariaSky", "96E8FCB521BCA47F", new Guid("10000000-0000-0000-0000-000000000003"), new DateTime(2024, 1, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("b4c5d6e7-8f9a-4b1c-9d2e-3f4a5b6c7d8e"), "E3F4A5B6" },
                    { new Guid("f4a5b6c7-8901-4def-9abc-4567890123de"), null, null, "ALEX@EXAMPLE.COM", null, null, "alex", "AlexStorm", "054D31AA38AEFAA2", new Guid("10000000-0000-0000-0000-000000000004"), new DateTime(2024, 1, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("b4c5d6e7-8f9a-4b1c-9d2e-3f4a5b6c7d8e"), "F4A5B6C7" }
                });

            migrationBuilder.InsertData(
                table: "Posts",
                columns: new[] { "Id", "Bio", "CreatedAt", "DeletedAt", "ImageUrl", "LikesQnt", "RaceId", "SharesQnt", "Title", "UserId" },
                values: new object[,]
                {
                    { new Guid("a1111111-1111-1111-1111-111111111111"), "This is the very first post in the system.", new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 10, new Guid("10000000-0000-0000-0000-000000000001"), 2, "Welcome Post", new Guid("c1d2e3f4-5678-4abc-9def-1234567890ab") },
                    { new Guid("a2222222-2222-2222-2222-222222222222"), "Sharing some daily reflections.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 5, new Guid("10000000-0000-0000-0000-000000000002"), 1, "Daily Thoughts", new Guid("d2e3f4a5-6789-4bcd-8efa-2345678901bc") }
                });

            migrationBuilder.InsertData(
                table: "Posts",
                columns: new[] { "Id", "Bio", "CreatedAt", "DeletedAt", "ImageUrl", "LikesQnt", "RaceId", "Title", "UserId" },
                values: new object[] { new Guid("a3333333-3333-3333-3333-333333333333"), "Hello everyone, glad to join!", new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 7, new Guid("10000000-0000-0000-0000-000000000003"), "Maria’s First Post", new Guid("e3f4a5b6-7890-4cde-9fab-3456789012cd") });

            migrationBuilder.InsertData(
                table: "Posts",
                columns: new[] { "Id", "Bio", "CreatedAt", "DeletedAt", "ImageUrl", "LikesQnt", "RaceId", "SharesQnt", "Title", "UserId" },
                values: new object[,]
                {
                    { new Guid("a4444444-4444-4444-4444-444444444444"), "Quick update from Alex.", new DateTime(2026, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 3, new Guid("10000000-0000-0000-0000-000000000004"), 1, "Alex’s Update", new Guid("f4a5b6c7-8901-4def-9abc-4567890123de") },
                    { new Guid("a5555555-5555-5555-5555-555555555555"), "Excited to share my first post.", new DateTime(2026, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 12, new Guid("10000000-0000-0000-0000-000000000005"), 4, "Sofia’s Post", new Guid("01234567-89ab-4cde-9f01-234567890abc") },
                    { new Guid("a6666666-6666-6666-6666-666666666666"), "Mike shares his ideas.", new DateTime(2026, 3, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 8, new Guid("10000000-0000-0000-0000-000000000006"), 2, "Mike’s Thoughts", new Guid("12345678-9abc-4def-9012-345678901bcd") },
                    { new Guid("a7777777-7777-7777-7777-777777777777"), "Olga writes about her day.", new DateTime(2026, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 15, new Guid("10000000-0000-0000-0000-000000000007"), 5, "Olga’s Story", new Guid("23456789-abcd-4ef0-9123-45678901cdef") },
                    { new Guid("a8888888-8888-8888-8888-888888888888"), "Ivan shares his music journey.", new DateTime(2026, 3, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 20, new Guid("10000000-0000-0000-0000-000000000008"), 7, "Ivan’s Rock", new Guid("3456789a-bcde-4f01-9234-5678901def01") },
                    { new Guid("a9999999-9999-9999-9999-999999999999"), "Important system update.", new DateTime(2026, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 30, new Guid("10000000-0000-0000-0000-000000000001"), 10, "Admin’s Announcement", new Guid("c1d2e3f4-5678-4abc-9def-1234567890ab") },
                    { new Guid("b1111111-1111-1111-1111-111111111111"), "Sharing my travel experience.", new DateTime(2026, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 18, new Guid("10000000-0000-0000-0000-000000000002"), 6, "John’s Travel", new Guid("d2e3f4a5-6789-4bcd-8efa-2345678901bc") },
                    { new Guid("b2222222-2222-2222-2222-222222222222"), "Cooking tips and tricks.", new DateTime(2026, 3, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 25, new Guid("10000000-0000-0000-0000-000000000003"), 9, "Maria’s Recipe", new Guid("e3f4a5b6-7890-4cde-9fab-3456789012cd") },
                    { new Guid("b3333333-3333-3333-3333-333333333333"), "Alex shares coding tips.", new DateTime(2026, 3, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 14, new Guid("10000000-0000-0000-0000-000000000004"), 3, "Alex’s Coding", new Guid("f4a5b6c7-8901-4def-9abc-4567890123de") },
                    { new Guid("b4444444-4444-4444-4444-444444444444"), "My latest painting.", new DateTime(2026, 3, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 22, new Guid("10000000-0000-0000-0000-000000000005"), 8, "Sofia’s Art", new Guid("01234567-89ab-4cde-9f01-234567890abc") },
                    { new Guid("b5555555-5555-5555-5555-555555555555"), "Workout routines.", new DateTime(2026, 3, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 16, new Guid("10000000-0000-0000-0000-000000000007"), 5, "Mike’s Fitness", new Guid("12345678-9abc-4def-9012-345678901bcd") },
                    { new Guid("b6666666-6666-6666-6666-666666666666"), "Reviewing my favorite book.", new DateTime(2026, 3, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 19, new Guid("10000000-0000-0000-0000-000000000008"), 7, "Olga’s Book Review", new Guid("23456789-abcd-4ef0-9123-45678901cdef") },
                    { new Guid("b7777777-7777-7777-7777-777777777777"), "Football match highlights.", new DateTime(2026, 3, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 28, new Guid("10000000-0000-0000-0000-000000000001"), 11, "Ivan’s Sports", new Guid("3456789a-bcde-4f01-9234-5678901def01") },
                    { new Guid("b8888888-8888-8888-8888-888888888888"), "System usage tips.", new DateTime(2026, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 12, new Guid("10000000-0000-0000-0000-000000000002"), 4, "Admin’s Tips", new Guid("c1d2e3f4-5678-4abc-9def-1234567890ab") },
                    { new Guid("b9999999-9999-9999-9999-999999999999"), "Sharing my playlist.", new DateTime(2026, 3, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 21, new Guid("10000000-0000-0000-0000-000000000003"), 8, "John’s Music", new Guid("d2e3f4a5-6789-4bcd-8efa-2345678901bc") },
                    { new Guid("c1111111-1111-1111-1111-111111111111"), "Trip to the mountains.", new DateTime(2026, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 17, new Guid("10000000-0000-0000-0000-000000000004"), 6, "Maria’s Travel", new Guid("e3f4a5b6-7890-4cde-9fab-3456789012cd") },
                    { new Guid("c2222222-2222-2222-2222-222222222222"), "Latest game review.", new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 24, new Guid("10000000-0000-0000-0000-000000000005"), 9, "Alex’s Gaming", new Guid("f4a5b6c7-8901-4def-9abc-4567890123de") },
                    { new Guid("c3333333-3333-3333-3333-333333333333"), "Trip to the seaside.", new DateTime(2026, 3, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 13, new Guid("10000000-0000-0000-0000-000000000006"), 4, "Sofia’s Travel", new Guid("01234567-89ab-4cde-9f01-234567890abc") },
                    { new Guid("c4444444-4444-4444-4444-444444444444"), "Learning C# step by step.", new DateTime(2026, 3, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 9, new Guid("10000000-0000-0000-0000-000000000007"), 3, "Mike’s Coding Journey", new Guid("12345678-9abc-4def-9012-345678901bcd") },
                    { new Guid("c5555555-5555-5555-5555-555555555555"), "My favorite recipes.", new DateTime(2026, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 27, new Guid("10000000-0000-0000-0000-000000000008"), 12, "Olga’s Cooking", new Guid("23456789-abcd-4ef0-9123-45678901cdef") },
                    { new Guid("c6666666-6666-6666-6666-666666666666"), "Exploring the forest trails.", new DateTime(2026, 3, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 19, new Guid("10000000-0000-0000-0000-000000000001"), 6, "Ivan’s Hiking", new Guid("3456789a-bcde-4f01-9234-5678901def01") },
                    { new Guid("c7777777-7777-7777-7777-777777777777"), "How to keep your account safe.", new DateTime(2026, 3, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 33, new Guid("10000000-0000-0000-0000-000000000002"), 14, "Admin’s Security Tips", new Guid("c1d2e3f4-5678-4abc-9def-1234567890ab") },
                    { new Guid("c8888888-8888-8888-8888-888888888888"), "Capturing the sunset.", new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 22, new Guid("10000000-0000-0000-0000-000000000003"), 7, "John’s Photography", new Guid("d2e3f4a5-6789-4bcd-8efa-2345678901bc") }
                });

            migrationBuilder.InsertData(
                table: "UsersInterests",
                columns: new[] { "InterestId", "UserId" },
                values: new object[,]
                {
                    { new Guid("99999999-9999-9999-9999-999999999999"), new Guid("01234567-89ab-4cde-9f01-234567890abc") },
                    { new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), new Guid("01234567-89ab-4cde-9f01-234567890abc") },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new Guid("12345678-9abc-4def-9012-345678901bcd") },
                    { new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"), new Guid("12345678-9abc-4def-9012-345678901bcd") },
                    { new Guid("12121212-1212-1212-1212-121212121212"), new Guid("23456789-abcd-4ef0-9123-45678901cdef") },
                    { new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"), new Guid("23456789-abcd-4ef0-9123-45678901cdef") },
                    { new Guid("15151515-1515-1515-1515-151515151515"), new Guid("3456789a-bcde-4f01-9234-5678901def01") },
                    { new Guid("16161616-1616-1616-1616-161616161616"), new Guid("3456789a-bcde-4f01-9234-5678901def01") },
                    { new Guid("11111111-1111-1111-1111-111111111111"), new Guid("c1d2e3f4-5678-4abc-9def-1234567890ab") },
                    { new Guid("66666666-6666-6666-6666-666666666666"), new Guid("c1d2e3f4-5678-4abc-9def-1234567890ab") },
                    { new Guid("22222222-2222-2222-2222-222222222222"), new Guid("d2e3f4a5-6789-4bcd-8efa-2345678901bc") },
                    { new Guid("44444444-4444-4444-4444-444444444444"), new Guid("d2e3f4a5-6789-4bcd-8efa-2345678901bc") },
                    { new Guid("33333333-3333-3333-3333-333333333333"), new Guid("e3f4a5b6-7890-4cde-9fab-3456789012cd") },
                    { new Guid("77777777-7777-7777-7777-777777777777"), new Guid("e3f4a5b6-7890-4cde-9fab-3456789012cd") },
                    { new Guid("55555555-5555-5555-5555-555555555555"), new Guid("f4a5b6c7-8901-4def-9abc-4567890123de") },
                    { new Guid("88888888-8888-8888-8888-888888888888"), new Guid("f4a5b6c7-8901-4def-9abc-4567890123de") }
                });

            migrationBuilder.InsertData(
                table: "Comments",
                columns: new[] { "Id", "Bio", "CreatedAt", "DeletedAt", "EditedAt", "IsEdited", "LikesQnt", "PostId", "UserId" },
                values: new object[,]
                {
                    { new Guid("e2222222-2222-2222-2222-222222222222"), "Great introduction, looking forward to more posts!", new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, false, 4, new Guid("a1111111-1111-1111-1111-111111111111"), new Guid("d2e3f4a5-6789-4bcd-8efa-2345678901bc") },
                    { new Guid("e3333333-3333-3333-3333-333333333333"), "Nice reflections, John. Keep sharing!", new DateTime(2024, 2, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, false, 2, new Guid("a2222222-2222-2222-2222-222222222222"), new Guid("e3f4a5b6-7890-4cde-9fab-3456789012cd") },
                    { new Guid("e4444444-4444-4444-4444-444444444444"), "Welcome Maria! Glad to see your first post.", new DateTime(2024, 2, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, false, 5, new Guid("a3333333-3333-3333-3333-333333333333"), new Guid("f4a5b6c7-8901-4def-9abc-4567890123de") },
                    { new Guid("e5555555-5555-5555-5555-555555555555"), "Thanks for the update, Alex!", new DateTime(2024, 2, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, false, 3, new Guid("a4444444-4444-4444-4444-444444444444"), new Guid("01234567-89ab-4cde-9f01-234567890abc") },
                    { new Guid("e6666666-6666-6666-6666-666666666666"), "Nice post, Sofia! Looking forward to more.", new DateTime(2024, 2, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, false, 6, new Guid("a5555555-5555-5555-5555-555555555555"), new Guid("12345678-9abc-4def-9012-345678901bcd") },
                    { new Guid("e7777777-7777-7777-7777-777777777777"), "Interesting ideas, Mike!", new DateTime(2024, 2, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, false, 1, new Guid("a6666666-6666-6666-6666-666666666666"), new Guid("23456789-abcd-4ef0-9123-45678901cdef") },
                    { new Guid("e8888888-8888-8888-8888-888888888888"), "Nice story, Olga!", new DateTime(2024, 2, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, false, 7, new Guid("a7777777-7777-7777-7777-777777777777"), new Guid("3456789a-bcde-4f01-9234-5678901def01") }
                });

            migrationBuilder.InsertData(
                table: "PostsInterests",
                columns: new[] { "InterestId", "PostId" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), new Guid("a1111111-1111-1111-1111-111111111111") },
                    { new Guid("66666666-6666-6666-6666-666666666666"), new Guid("a1111111-1111-1111-1111-111111111111") },
                    { new Guid("22222222-2222-2222-2222-222222222222"), new Guid("a2222222-2222-2222-2222-222222222222") },
                    { new Guid("44444444-4444-4444-4444-444444444444"), new Guid("a2222222-2222-2222-2222-222222222222") },
                    { new Guid("33333333-3333-3333-3333-333333333333"), new Guid("a3333333-3333-3333-3333-333333333333") },
                    { new Guid("77777777-7777-7777-7777-777777777777"), new Guid("a3333333-3333-3333-3333-333333333333") },
                    { new Guid("55555555-5555-5555-5555-555555555555"), new Guid("a4444444-4444-4444-4444-444444444444") },
                    { new Guid("88888888-8888-8888-8888-888888888888"), new Guid("a4444444-4444-4444-4444-444444444444") },
                    { new Guid("99999999-9999-9999-9999-999999999999"), new Guid("a5555555-5555-5555-5555-555555555555") },
                    { new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), new Guid("a5555555-5555-5555-5555-555555555555") },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new Guid("a6666666-6666-6666-6666-666666666666") },
                    { new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"), new Guid("a6666666-6666-6666-6666-666666666666") },
                    { new Guid("12121212-1212-1212-1212-121212121212"), new Guid("a7777777-7777-7777-7777-777777777777") },
                    { new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"), new Guid("a7777777-7777-7777-7777-777777777777") },
                    { new Guid("15151515-1515-1515-1515-151515151515"), new Guid("a8888888-8888-8888-8888-888888888888") },
                    { new Guid("16161616-1616-1616-1616-161616161616"), new Guid("a8888888-8888-8888-8888-888888888888") },
                    { new Guid("11111111-1111-1111-1111-111111111111"), new Guid("a9999999-9999-9999-9999-999999999999") },
                    { new Guid("66666666-6666-6666-6666-666666666666"), new Guid("a9999999-9999-9999-9999-999999999999") },
                    { new Guid("22222222-2222-2222-2222-222222222222"), new Guid("b1111111-1111-1111-1111-111111111111") },
                    { new Guid("77777777-7777-7777-7777-777777777777"), new Guid("b1111111-1111-1111-1111-111111111111") },
                    { new Guid("33333333-3333-3333-3333-333333333333"), new Guid("b2222222-2222-2222-2222-222222222222") },
                    { new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"), new Guid("b2222222-2222-2222-2222-222222222222") },
                    { new Guid("55555555-5555-5555-5555-555555555555"), new Guid("b3333333-3333-3333-3333-333333333333") },
                    { new Guid("66666666-6666-6666-6666-666666666666"), new Guid("b3333333-3333-3333-3333-333333333333") },
                    { new Guid("99999999-9999-9999-9999-999999999999"), new Guid("b4444444-4444-4444-4444-444444444444") },
                    { new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), new Guid("b4444444-4444-4444-4444-444444444444") },
                    { new Guid("55555555-5555-5555-5555-555555555555"), new Guid("b5555555-5555-5555-5555-555555555555") },
                    { new Guid("88888888-8888-8888-8888-888888888888"), new Guid("b5555555-5555-5555-5555-555555555555") },
                    { new Guid("22222222-2222-2222-2222-222222222222"), new Guid("b6666666-6666-6666-6666-666666666666") },
                    { new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"), new Guid("b6666666-6666-6666-6666-666666666666") },
                    { new Guid("11111111-1111-1111-1111-111111111111"), new Guid("b7777777-7777-7777-7777-777777777777") },
                    { new Guid("16161616-1616-1616-1616-161616161616"), new Guid("b7777777-7777-7777-7777-777777777777") },
                    { new Guid("14141414-1414-1414-1414-141414141414"), new Guid("b8888888-8888-8888-8888-888888888888") },
                    { new Guid("66666666-6666-6666-6666-666666666666"), new Guid("b8888888-8888-8888-8888-888888888888") },
                    { new Guid("22222222-2222-2222-2222-222222222222"), new Guid("b9999999-9999-9999-9999-999999999999") },
                    { new Guid("44444444-4444-4444-4444-444444444444"), new Guid("b9999999-9999-9999-9999-999999999999") },
                    { new Guid("77777777-7777-7777-7777-777777777777"), new Guid("c1111111-1111-1111-1111-111111111111") },
                    { new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), new Guid("c1111111-1111-1111-1111-111111111111") },
                    { new Guid("44444444-4444-4444-4444-444444444444"), new Guid("c2222222-2222-2222-2222-222222222222") },
                    { new Guid("55555555-5555-5555-5555-555555555555"), new Guid("c2222222-2222-2222-2222-222222222222") },
                    { new Guid("77777777-7777-7777-7777-777777777777"), new Guid("c3333333-3333-3333-3333-333333333333") },
                    { new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), new Guid("c3333333-3333-3333-3333-333333333333") },
                    { new Guid("66666666-6666-6666-6666-666666666666"), new Guid("c4444444-4444-4444-4444-444444444444") },
                    { new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"), new Guid("c4444444-4444-4444-4444-444444444444") },
                    { new Guid("22222222-2222-2222-2222-222222222222"), new Guid("c5555555-5555-5555-5555-555555555555") },
                    { new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"), new Guid("c5555555-5555-5555-5555-555555555555") },
                    { new Guid("12121212-1212-1212-1212-121212121212"), new Guid("c6666666-6666-6666-6666-666666666666") },
                    { new Guid("77777777-7777-7777-7777-777777777777"), new Guid("c6666666-6666-6666-6666-666666666666") },
                    { new Guid("13131313-1313-1313-1313-131313131313"), new Guid("c7777777-7777-7777-7777-777777777777") },
                    { new Guid("14141414-1414-1414-1414-141414141414"), new Guid("c7777777-7777-7777-7777-777777777777") },
                    { new Guid("44444444-4444-4444-4444-444444444444"), new Guid("c8888888-8888-8888-8888-888888888888") },
                    { new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), new Guid("c8888888-8888-8888-8888-888888888888") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_PostId",
                table: "Comments",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserId",
                table: "Comments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_RaceId",
                table: "Posts",
                column: "RaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_UserId",
                table: "Posts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PostsInterests_InterestId",
                table: "PostsInterests",
                column: "InterestId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Login",
                table: "Users",
                column: "Login",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_RaceId",
                table: "Users",
                column: "RaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersInterests_InterestId",
                table: "UsersInterests",
                column: "InterestId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "PostsInterests");

            migrationBuilder.DropTable(
                name: "UsersInterests");

            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "Interests");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Races");

            migrationBuilder.DropTable(
                name: "UserRoles");
        }
    }
}
