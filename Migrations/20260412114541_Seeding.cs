using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SocialMediaBackend.Migrations
{
    /// <inheritdoc />
    public partial class Seeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                columns: new[] { "Id", "Bio", "DeletedAt", "Email", "ImageUrl", "LastLoginAt", "Login", "Nickname", "PasswordHash", "RegisteredAt", "RoleId", "Salt" },
                values: new object[,]
                {
                    { new Guid("01234567-89ab-4cde-9f01-234567890abc"), null, null, "SOFIA@EXAMPLE.COM", null, null, "sofia", "SofiaLight", "9A6A64B2E3004012", new DateTime(2024, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("b4c5d6e7-8f9a-4b1c-9d2e-3f4a5b6c7d8e"), "01234567" },
                    { new Guid("12345678-9abc-4def-9012-345678901bcd"), null, null, "MIKE@EXAMPLE.COM", null, null, "mike", "MikeWave", "75C1721BEB275FCA", new DateTime(2024, 1, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("b4c5d6e7-8f9a-4b1c-9d2e-3f4a5b6c7d8e"), "12345678" },
                    { new Guid("23456789-abcd-4ef0-9123-45678901cdef"), null, null, "OLGA@EXAMPLE.COM", null, null, "olga", "OlgaStar", "3C61FA60FC810A96", new DateTime(2024, 1, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("b4c5d6e7-8f9a-4b1c-9d2e-3f4a5b6c7d8e"), "23456789" },
                    { new Guid("3456789a-bcde-4f01-9234-5678901def01"), null, null, "IVAN@EXAMPLE.COM", null, null, "ivan", "IvanRock", "16B2C8B41DFB08CF", new DateTime(2024, 1, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("b4c5d6e7-8f9a-4b1c-9d2e-3f4a5b6c7d8e"), "3456789A" },
                    { new Guid("c1d2e3f4-5678-4abc-9def-1234567890ab"), null, null, "ADMIN@EXAMPLE.COM", null, null, "admin", "SuperAdmin", "FA7C65CB4BB64C06", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("a3f2e1b4-9c5d-4f2a-8d7a-1b2c3d4e5f6a"), "C1D2E3F4" },
                    { new Guid("d2e3f4a5-6789-4bcd-8efa-2345678901bc"), null, null, "JOHN@EXAMPLE.COM", null, null, "john", "JohnDoe", "6D8208F48DDA7332", new DateTime(2024, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("b4c5d6e7-8f9a-4b1c-9d2e-3f4a5b6c7d8e"), "D2E3F4A5" },
                    { new Guid("e3f4a5b6-7890-4cde-9fab-3456789012cd"), null, null, "MARIA@EXAMPLE.COM", null, null, "maria", "MariaSky", "96E8FCB521BCA47F", new DateTime(2024, 1, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("b4c5d6e7-8f9a-4b1c-9d2e-3f4a5b6c7d8e"), "E3F4A5B6" },
                    { new Guid("f4a5b6c7-8901-4def-9abc-4567890123de"), null, null, "ALEX@EXAMPLE.COM", null, null, "alex", "AlexStorm", "054D31AA38AEFAA2", new DateTime(2024, 1, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("b4c5d6e7-8f9a-4b1c-9d2e-3f4a5b6c7d8e"), "F4A5B6C7" }
                });

            migrationBuilder.InsertData(
                table: "Posts",
                columns: new[] { "Id", "Bio", "CreatedAt", "DeletedAt", "ImageUrl", "LikesQnt", "SharesQnt", "Title", "UserId" },
                values: new object[,]
                {
                    { new Guid("a1111111-1111-1111-1111-111111111111"), "This is the very first post in the system.", new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 10, 2, "Welcome Post", new Guid("c1d2e3f4-5678-4abc-9def-1234567890ab") },
                    { new Guid("a2222222-2222-2222-2222-222222222222"), "Sharing some daily reflections.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 5, 1, "Daily Thoughts", new Guid("d2e3f4a5-6789-4bcd-8efa-2345678901bc") }
                });

            migrationBuilder.InsertData(
                table: "Posts",
                columns: new[] { "Id", "Bio", "CreatedAt", "DeletedAt", "ImageUrl", "LikesQnt", "Title", "UserId" },
                values: new object[] { new Guid("a3333333-3333-3333-3333-333333333333"), "Hello everyone, glad to join!", new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 7, "Maria’s First Post", new Guid("e3f4a5b6-7890-4cde-9fab-3456789012cd") });

            migrationBuilder.InsertData(
                table: "Posts",
                columns: new[] { "Id", "Bio", "CreatedAt", "DeletedAt", "ImageUrl", "LikesQnt", "SharesQnt", "Title", "UserId" },
                values: new object[,]
                {
                    { new Guid("a4444444-4444-4444-4444-444444444444"), "Quick update from Alex.", new DateTime(2026, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 3, 1, "Alex’s Update", new Guid("f4a5b6c7-8901-4def-9abc-4567890123de") },
                    { new Guid("a5555555-5555-5555-5555-555555555555"), "Excited to share my first post.", new DateTime(2026, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 12, 4, "Sofia’s Post", new Guid("01234567-89ab-4cde-9f01-234567890abc") },
                    { new Guid("a6666666-6666-6666-6666-666666666666"), "Mike shares his ideas.", new DateTime(2026, 3, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 8, 2, "Mike’s Thoughts", new Guid("12345678-9abc-4def-9012-345678901bcd") },
                    { new Guid("a7777777-7777-7777-7777-777777777777"), "Olga writes about her day.", new DateTime(2026, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 15, 5, "Olga’s Story", new Guid("23456789-abcd-4ef0-9123-45678901cdef") },
                    { new Guid("a8888888-8888-8888-8888-888888888888"), "Ivan shares his music journey.", new DateTime(2026, 3, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 20, 7, "Ivan’s Rock", new Guid("3456789a-bcde-4f01-9234-5678901def01") },
                    { new Guid("a9999999-9999-9999-9999-999999999999"), "Important system update.", new DateTime(2026, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 30, 10, "Admin’s Announcement", new Guid("c1d2e3f4-5678-4abc-9def-1234567890ab") },
                    { new Guid("b1111111-1111-1111-1111-111111111111"), "Sharing my travel experience.", new DateTime(2026, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 18, 6, "John’s Travel", new Guid("d2e3f4a5-6789-4bcd-8efa-2345678901bc") },
                    { new Guid("b2222222-2222-2222-2222-222222222222"), "Cooking tips and tricks.", new DateTime(2026, 3, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 25, 9, "Maria’s Recipe", new Guid("e3f4a5b6-7890-4cde-9fab-3456789012cd") },
                    { new Guid("b3333333-3333-3333-3333-333333333333"), "Alex shares coding tips.", new DateTime(2026, 3, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 14, 3, "Alex’s Coding", new Guid("f4a5b6c7-8901-4def-9abc-4567890123de") },
                    { new Guid("b4444444-4444-4444-4444-444444444444"), "My latest painting.", new DateTime(2026, 3, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 22, 8, "Sofia’s Art", new Guid("01234567-89ab-4cde-9f01-234567890abc") },
                    { new Guid("b5555555-5555-5555-5555-555555555555"), "Workout routines.", new DateTime(2026, 3, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 16, 5, "Mike’s Fitness", new Guid("12345678-9abc-4def-9012-345678901bcd") },
                    { new Guid("b6666666-6666-6666-6666-666666666666"), "Reviewing my favorite book.", new DateTime(2026, 3, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 19, 7, "Olga’s Book Review", new Guid("23456789-abcd-4ef0-9123-45678901cdef") },
                    { new Guid("b7777777-7777-7777-7777-777777777777"), "Football match highlights.", new DateTime(2026, 3, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 28, 11, "Ivan’s Sports", new Guid("3456789a-bcde-4f01-9234-5678901def01") },
                    { new Guid("b8888888-8888-8888-8888-888888888888"), "System usage tips.", new DateTime(2026, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 12, 4, "Admin’s Tips", new Guid("c1d2e3f4-5678-4abc-9def-1234567890ab") },
                    { new Guid("b9999999-9999-9999-9999-999999999999"), "Sharing my playlist.", new DateTime(2026, 3, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 21, 8, "John’s Music", new Guid("d2e3f4a5-6789-4bcd-8efa-2345678901bc") },
                    { new Guid("c1111111-1111-1111-1111-111111111111"), "Trip to the mountains.", new DateTime(2026, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 17, 6, "Maria’s Travel", new Guid("e3f4a5b6-7890-4cde-9fab-3456789012cd") },
                    { new Guid("c2222222-2222-2222-2222-222222222222"), "Latest game review.", new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 24, 9, "Alex’s Gaming", new Guid("f4a5b6c7-8901-4def-9abc-4567890123de") },
                    { new Guid("c3333333-3333-3333-3333-333333333333"), "Trip to the seaside.", new DateTime(2026, 3, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 13, 4, "Sofia’s Travel", new Guid("01234567-89ab-4cde-9f01-234567890abc") },
                    { new Guid("c4444444-4444-4444-4444-444444444444"), "Learning C# step by step.", new DateTime(2026, 3, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 9, 3, "Mike’s Coding Journey", new Guid("12345678-9abc-4def-9012-345678901bcd") },
                    { new Guid("c5555555-5555-5555-5555-555555555555"), "My favorite recipes.", new DateTime(2026, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 27, 12, "Olga’s Cooking", new Guid("23456789-abcd-4ef0-9123-45678901cdef") },
                    { new Guid("c6666666-6666-6666-6666-666666666666"), "Exploring the forest trails.", new DateTime(2026, 3, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 19, 6, "Ivan’s Hiking", new Guid("3456789a-bcde-4f01-9234-5678901def01") },
                    { new Guid("c7777777-7777-7777-7777-777777777777"), "How to keep your account safe.", new DateTime(2026, 3, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 33, 14, "Admin’s Security Tips", new Guid("c1d2e3f4-5678-4abc-9def-1234567890ab") },
                    { new Guid("c8888888-8888-8888-8888-888888888888"), "Capturing the sunset.", new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 22, 7, "John’s Photography", new Guid("d2e3f4a5-6789-4bcd-8efa-2345678901bc") }
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: new Guid("e2222222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: new Guid("e3333333-3333-3333-3333-333333333333"));

            migrationBuilder.DeleteData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: new Guid("e4444444-4444-4444-4444-444444444444"));

            migrationBuilder.DeleteData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: new Guid("e5555555-5555-5555-5555-555555555555"));

            migrationBuilder.DeleteData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: new Guid("e6666666-6666-6666-6666-666666666666"));

            migrationBuilder.DeleteData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: new Guid("e7777777-7777-7777-7777-777777777777"));

            migrationBuilder.DeleteData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: new Guid("e8888888-8888-8888-8888-888888888888"));

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: new Guid("a8888888-8888-8888-8888-888888888888"));

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: new Guid("a9999999-9999-9999-9999-999999999999"));

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: new Guid("b1111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: new Guid("b2222222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: new Guid("b3333333-3333-3333-3333-333333333333"));

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: new Guid("b4444444-4444-4444-4444-444444444444"));

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: new Guid("b5555555-5555-5555-5555-555555555555"));

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: new Guid("b6666666-6666-6666-6666-666666666666"));

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: new Guid("b7777777-7777-7777-7777-777777777777"));

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: new Guid("b8888888-8888-8888-8888-888888888888"));

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: new Guid("b9999999-9999-9999-9999-999999999999"));

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: new Guid("c1111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: new Guid("c2222222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: new Guid("c3333333-3333-3333-3333-333333333333"));

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: new Guid("c4444444-4444-4444-4444-444444444444"));

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: new Guid("c5555555-5555-5555-5555-555555555555"));

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: new Guid("c6666666-6666-6666-6666-666666666666"));

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: new Guid("c7777777-7777-7777-7777-777777777777"));

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: new Guid("c8888888-8888-8888-8888-888888888888"));

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: new Guid("a1111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: new Guid("a2222222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: new Guid("a3333333-3333-3333-3333-333333333333"));

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: new Guid("a4444444-4444-4444-4444-444444444444"));

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: new Guid("a5555555-5555-5555-5555-555555555555"));

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: new Guid("a6666666-6666-6666-6666-666666666666"));

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: new Guid("a7777777-7777-7777-7777-777777777777"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("3456789a-bcde-4f01-9234-5678901def01"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("01234567-89ab-4cde-9f01-234567890abc"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("12345678-9abc-4def-9012-345678901bcd"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("23456789-abcd-4ef0-9123-45678901cdef"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("c1d2e3f4-5678-4abc-9def-1234567890ab"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("d2e3f4a5-6789-4bcd-8efa-2345678901bc"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("e3f4a5b6-7890-4cde-9fab-3456789012cd"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("f4a5b6c7-8901-4def-9abc-4567890123de"));

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumn: "Id",
                keyValue: new Guid("a3f2e1b4-9c5d-4f2a-8d7a-1b2c3d4e5f6a"));

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumn: "Id",
                keyValue: new Guid("b4c5d6e7-8f9a-4b1c-9d2e-3f4a5b6c7d8e"));
        }
    }
}
