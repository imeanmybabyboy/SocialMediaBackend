using ASP_PV411.Services.Kdf;
using SocialMediaBackend.Data.Entities;

namespace SocialMediaBackend.Data
{
    public static class SeedData
    {
        public static User[] Users(IKdfService kdfService)
        {
            string saltAdmin = "C1D2E3F4";
            string adminPassHash = kdfService.Dk("admin", saltAdmin);

            string saltJohn = "D2E3F4A5";
            string johnPasshash = kdfService.Dk("johnDoe", saltJohn);

            string saltMaria = "E3F4A5B6";
            string mariaPassHash = kdfService.Dk("mariaSky", saltMaria);

            string saltAlex = "F4A5B6C7";
            string alexPassHash = kdfService.Dk("alexStorm", saltAlex);

            string saltSofia = "01234567";
            string sofiaPassHash = kdfService.Dk("sofiaLight", saltSofia);

            string saltMike = "12345678";
            string mikePassHash = kdfService.Dk("mikeWave", saltMike);

            string saltOlga = "23456789";
            string olgaPassHash = kdfService.Dk("olgaStar", saltOlga);

            string saltIvan = "3456789A";
            string ivanPassHash = kdfService.Dk("ivanRock", saltIvan);

            return new[]
            {
            new User
            {
                Id = Guid.Parse("C1D2E3F4-5678-4ABC-9DEF-1234567890AB"),
                RoleId = Guid.Parse("A3F2E1B4-9C5D-4F2A-8D7A-1B2C3D4E5F6A"),
                RaceId = Guid.Parse("10000000-0000-0000-0000-000000000001"),
                Login = "admin",
                Nickname = "SuperAdmin",
                Email = "ADMIN@EXAMPLE.COM",
                PasswordHash = adminPassHash,
                Salt = saltAdmin,
                RegisteredAt = new DateTime(2024, 01, 01)
            },
            new User
            {
                Id = Guid.Parse("D2E3F4A5-6789-4BCD-8EFA-2345678901BC"),
                RoleId = Guid.Parse("B4C5D6E7-8F9A-4B1C-9D2E-3F4A5B6C7D8E"),
                RaceId = Guid.Parse("10000000-0000-0000-0000-000000000002"),
                Login = "john",
                Nickname = "JohnDoe",
                Email = "JOHN@EXAMPLE.COM",
                PasswordHash = johnPasshash,
                Salt = saltJohn,
                RegisteredAt = new DateTime(2024, 01, 02)
            },
            new User
            {
                Id = Guid.Parse("E3F4A5B6-7890-4CDE-9FAB-3456789012CD"),
                RoleId = Guid.Parse("B4C5D6E7-8F9A-4B1C-9D2E-3F4A5B6C7D8E"),
                RaceId = Guid.Parse("10000000-0000-0000-0000-000000000003"),
                Login = "maria",
                Nickname = "MariaSky",
                Email = "MARIA@EXAMPLE.COM",
                PasswordHash = mariaPassHash,
                Salt = saltMaria,
                RegisteredAt = new DateTime(2024, 01, 03)
            },
            new User
            {
                Id = Guid.Parse("F4A5B6C7-8901-4DEF-9ABC-4567890123DE"),
                RoleId = Guid.Parse("B4C5D6E7-8F9A-4B1C-9D2E-3F4A5B6C7D8E"),
                RaceId = Guid.Parse("10000000-0000-0000-0000-000000000004"),
                Login = "alex",
                Nickname = "AlexStorm",
                Email = "ALEX@EXAMPLE.COM",
                PasswordHash = alexPassHash,
                Salt = saltAlex,
                RegisteredAt = new DateTime(2024, 01, 04)
            },
            new User
            {
                Id = Guid.Parse("01234567-89AB-4CDE-9F01-234567890ABC"),
                RoleId = Guid.Parse("B4C5D6E7-8F9A-4B1C-9D2E-3F4A5B6C7D8E"),
                RaceId = Guid.Parse("10000000-0000-0000-0000-000000000005"),
                Login = "sofia",
                Nickname = "SofiaLight",
                Email = "SOFIA@EXAMPLE.COM",
                PasswordHash = sofiaPassHash,
                Salt = saltSofia,
                RegisteredAt = new DateTime(2024, 01, 05)
            },
            new User
            {
                Id = Guid.Parse("12345678-9ABC-4DEF-9012-345678901BCD"),
                RoleId = Guid.Parse("B4C5D6E7-8F9A-4B1C-9D2E-3F4A5B6C7D8E"),
                RaceId = Guid.Parse("10000000-0000-0000-0000-000000000006"),
                Login = "mike",
                Nickname = "MikeWave",
                Email = "MIKE@EXAMPLE.COM",
                PasswordHash = mikePassHash,
                Salt = saltMike,
                RegisteredAt = new DateTime(2024, 01, 06)
            },
            new User
            {
                Id = Guid.Parse("23456789-ABCD-4EF0-9123-45678901CDEF"),
                RoleId = Guid.Parse("B4C5D6E7-8F9A-4B1C-9D2E-3F4A5B6C7D8E"),
                RaceId = Guid.Parse("10000000-0000-0000-0000-000000000007"),
                Login = "olga",
                Nickname = "OlgaStar",
                Email = "OLGA@EXAMPLE.COM",
                PasswordHash = olgaPassHash,
                Salt = saltOlga,
                RegisteredAt = new DateTime(2024, 01, 07)
            },
            new User
            {
                Id = Guid.Parse("3456789A-BCDE-4F01-9234-5678901DEF01"),
                RoleId = Guid.Parse("B4C5D6E7-8F9A-4B1C-9D2E-3F4A5B6C7D8E"),
                RaceId = Guid.Parse("10000000-0000-0000-0000-000000000008"),
                Login = "ivan",
                Nickname = "IvanRock",
                Email = "IVAN@EXAMPLE.COM",
                PasswordHash = ivanPassHash,
                Salt = saltIvan,
                RegisteredAt = new DateTime(2024, 01, 08)
            }
            };
        }
        public static UserRole[] UserRoles()
        {
            return new[] {
                new UserRole
                {
                    Id = Guid.Parse("A3F2E1B4-9C5D-4F2A-8D7A-1B2C3D4E5F6A"),
                    Title = "Admin",
                    Description = "Administrator role"
                },
                new UserRole
                {
                    Id = Guid.Parse("B4C5D6E7-8F9A-4B1C-9D2E-3F4A5B6C7D8E"),
                    Title = "User",
                    Description = "Regular user role"
                }
            };
        }
        public static Post[] Posts()
        {
            return new[] {
                new Post
                {
                    Id = Guid.Parse("A1111111-1111-1111-1111-111111111111"),
                    UserId = Guid.Parse("C1D2E3F4-5678-4ABC-9DEF-1234567890AB"),
                    Title = "Welcome Post",
                    Bio = "This is the very first post in the system.",
                    LikesQnt = 10,
                    SharesQnt = 2,
                    CreatedAt = new DateTime(2026, 03, 01)
                },
                new Post
                {
                    Id = Guid.Parse("A2222222-2222-2222-2222-222222222222"),
                    UserId = Guid.Parse("D2E3F4A5-6789-4BCD-8EFA-2345678901BC"),
                    Title = "Daily Thoughts",
                    Bio = "Sharing some daily reflections.",
                    LikesQnt = 5,
                    SharesQnt = 1
                },
                new Post
                {
                    Id = Guid.Parse("A3333333-3333-3333-3333-333333333333"),
                    UserId = Guid.Parse("E3F4A5B6-7890-4CDE-9FAB-3456789012CD"),
                    Title = "Maria’s First Post",
                    Bio = "Hello everyone, glad to join!",
                    LikesQnt = 7,
                    SharesQnt = 0,
                    CreatedAt = new DateTime(2026, 03, 01)
                },
                new Post
                {
                    Id = Guid.Parse("A4444444-4444-4444-4444-444444444444"),
                    UserId = Guid.Parse("F4A5B6C7-8901-4DEF-9ABC-4567890123DE"),
                    Title = "Alex’s Update",
                    Bio = "Quick update from Alex.",
                    LikesQnt = 3,
                    SharesQnt = 1,
                    CreatedAt = new DateTime(2026, 03, 03)
                },
                new Post
                {
                    Id = Guid.Parse("A5555555-5555-5555-5555-555555555555"),
                    UserId = Guid.Parse("01234567-89AB-4CDE-9F01-234567890ABC"),
                    Title = "Sofia’s Post",
                    Bio = "Excited to share my first post.",
                    LikesQnt = 12,
                    SharesQnt = 4,
                    CreatedAt = new DateTime(2026, 02, 01)
                },
                new Post
                {
                    Id = Guid.Parse("A6666666-6666-6666-6666-666666666666"),
                    UserId = Guid.Parse("12345678-9ABC-4DEF-9012-345678901BCD"),
                    Title = "Mike’s Thoughts",
                    Bio = "Mike shares his ideas.",
                    LikesQnt = 8,
                    SharesQnt = 2,
                    CreatedAt = new DateTime(2026, 03, 02)
                },
                new Post
                {
                    Id = Guid.Parse("A7777777-7777-7777-7777-777777777777"),
                    UserId = Guid.Parse("23456789-ABCD-4EF0-9123-45678901CDEF"),
                    Title = "Olga’s Story",
                    Bio = "Olga writes about her day.",
                    LikesQnt = 15,
                    SharesQnt = 5,
                    CreatedAt = new DateTime(2026, 03, 03)
                },
                new Post
                {
                    Id = Guid.Parse("A8888888-8888-8888-8888-888888888888"),
                    UserId = Guid.Parse("3456789A-BCDE-4F01-9234-5678901DEF01"),
                    Title = "Ivan’s Rock",
                    Bio = "Ivan shares his music journey.",
                    LikesQnt = 20,
                    SharesQnt = 7,
                    CreatedAt = new DateTime(2026, 03, 04)
                },
                new Post
                {
                    Id = Guid.Parse("A9999999-9999-9999-9999-999999999999"),
                    UserId = Guid.Parse("C1D2E3F4-5678-4ABC-9DEF-1234567890AB"),
                    Title = "Admin’s Announcement",
                    Bio = "Important system update.",
                    LikesQnt = 30,
                    SharesQnt = 10,
                    CreatedAt = new DateTime(2026, 03, 05)
                },
                new Post
                {
                    Id = Guid.Parse("B1111111-1111-1111-1111-111111111111"),
                    UserId = Guid.Parse("D2E3F4A5-6789-4BCD-8EFA-2345678901BC"),
                    Title = "John’s Travel",
                    Bio = "Sharing my travel experience.",
                    LikesQnt = 18,
                    SharesQnt = 6,
                    CreatedAt = new DateTime(2026, 03, 03)
                },
                new Post
                {
                    Id = Guid.Parse("B2222222-2222-2222-2222-222222222222"),
                    UserId = Guid.Parse("E3F4A5B6-7890-4CDE-9FAB-3456789012CD"),
                    Title = "Maria’s Recipe",
                    Bio = "Cooking tips and tricks.",
                    LikesQnt = 25,
                    SharesQnt = 9,
                    CreatedAt = new DateTime(2026, 03, 04)
                },
                new Post
                {
                    Id = Guid.Parse("B3333333-3333-3333-3333-333333333333"),
                    UserId = Guid.Parse("F4A5B6C7-8901-4DEF-9ABC-4567890123DE"),
                    Title = "Alex’s Coding",
                    Bio = "Alex shares coding tips.",
                    LikesQnt = 14,
                    SharesQnt = 3,
                    CreatedAt = new DateTime(2026, 03, 06)
                },
                new Post
                {
                    Id = Guid.Parse("B4444444-4444-4444-4444-444444444444"),
                    UserId = Guid.Parse("01234567-89AB-4CDE-9F01-234567890ABC"),
                    Title = "Sofia’s Art",
                    Bio = "My latest painting.",
                    LikesQnt = 22,
                    SharesQnt = 8,
                    CreatedAt = new DateTime(2026, 03, 07)
                },
                new Post
                {
                    Id = Guid.Parse("B5555555-5555-5555-5555-555555555555"),
                    UserId = Guid.Parse("12345678-9ABC-4DEF-9012-345678901BCD"),
                    Title = "Mike’s Fitness",
                    Bio = "Workout routines.",
                    LikesQnt = 16,
                    SharesQnt = 5,
                    CreatedAt = new DateTime(2026, 03, 08)
                },
                new Post
                {
                    Id = Guid.Parse("B6666666-6666-6666-6666-666666666666"),
                    UserId = Guid.Parse("23456789-ABCD-4EF0-9123-45678901CDEF"),
                    Title = "Olga’s Book Review",
                    Bio = "Reviewing my favorite book.",
                    LikesQnt = 19,
                    SharesQnt = 7,
                    CreatedAt = new DateTime(2026, 03, 08)
                },
                new Post
                {
                    Id = Guid.Parse("B7777777-7777-7777-7777-777777777777"),
                    UserId = Guid.Parse("3456789A-BCDE-4F01-9234-5678901DEF01"),
                    Title = "Ivan’s Sports",
                    Bio = "Football match highlights.",
                    LikesQnt = 28,
                    SharesQnt = 11,
                    CreatedAt = new DateTime(2026, 03, 09)
                },
                new Post
                {
                    Id = Guid.Parse("B8888888-8888-8888-8888-888888888888"),
                    UserId = Guid.Parse("C1D2E3F4-5678-4ABC-9DEF-1234567890AB"),
                    Title = "Admin’s Tips",
                    Bio = "System usage tips.",
                    LikesQnt = 12,
                    SharesQnt = 4,
                    CreatedAt = new DateTime(2026, 03, 10)
                },
                new Post
                {
                    Id = Guid.Parse("B9999999-9999-9999-9999-999999999999"),
                    UserId = Guid.Parse("D2E3F4A5-6789-4BCD-8EFA-2345678901BC"),
                    Title = "John’s Music",
                    Bio = "Sharing my playlist.",
                    LikesQnt = 21,
                    SharesQnt = 8,
                    CreatedAt = new DateTime(2026, 03, 09)
                },
                new Post
                {
                    Id = Guid.Parse("C1111111-1111-1111-1111-111111111111"),
                    UserId = Guid.Parse("E3F4A5B6-7890-4CDE-9FAB-3456789012CD"),
                    Title = "Maria’s Travel",
                    Bio = "Trip to the mountains.",
                    LikesQnt = 17,
                    SharesQnt = 6,
                    CreatedAt = new DateTime(2026, 03, 10)
                },
                new Post
                {
                    Id = Guid.Parse("C2222222-2222-2222-2222-222222222222"),
                    UserId = Guid.Parse("F4A5B6C7-8901-4DEF-9ABC-4567890123DE"),
                    Title = "Alex’s Gaming",
                    Bio = "Latest game review.",
                    LikesQnt = 24,
                    SharesQnt = 9,
                    CreatedAt = new DateTime(2026, 03, 11)
                },
                new Post
                {
                    Id = Guid.Parse("C3333333-3333-3333-3333-333333333333"),
                    UserId = Guid.Parse("01234567-89AB-4CDE-9F01-234567890ABC"),
                    Title = "Sofia’s Travel",
                    Bio = "Trip to the seaside.",
                    LikesQnt = 13,
                    SharesQnt = 4,
                    CreatedAt = new DateTime(2026, 03, 12)
                },
                new Post
                {
                    Id = Guid.Parse("C4444444-4444-4444-4444-444444444444"),
                    UserId = Guid.Parse("12345678-9ABC-4DEF-9012-345678901BCD"),
                    Title = "Mike’s Coding Journey",
                    Bio = "Learning C# step by step.",
                    LikesQnt = 9,
                    SharesQnt = 3,
                    CreatedAt = new DateTime(2026, 03, 04)
                },
                new Post
                {
                    Id = Guid.Parse("C5555555-5555-5555-5555-555555555555"),
                    UserId = Guid.Parse("23456789-ABCD-4EF0-9123-45678901CDEF"),
                    Title = "Olga’s Cooking",
                    Bio = "My favorite recipes.",
                    LikesQnt = 27,
                    SharesQnt = 12,
                    CreatedAt = new DateTime(2026, 03, 05)
                },
                new Post
                {
                    Id = Guid.Parse("C6666666-6666-6666-6666-666666666666"),
                    UserId = Guid.Parse("3456789A-BCDE-4F01-9234-5678901DEF01"),
                    Title = "Ivan’s Hiking",
                    Bio = "Exploring the forest trails.",
                    LikesQnt = 19,
                    SharesQnt = 6,
                    CreatedAt = new DateTime(2026, 03, 06)
                },
                new Post
                {
                    Id = Guid.Parse("C7777777-7777-7777-7777-777777777777"),
                    UserId = Guid.Parse("C1D2E3F4-5678-4ABC-9DEF-1234567890AB"),
                    Title = "Admin’s Security Tips",
                    Bio = "How to keep your account safe.",
                    LikesQnt = 33,
                    SharesQnt = 14,
                    CreatedAt = new DateTime(2026, 03, 12)
                },
                new Post
                {
                    Id = Guid.Parse("C8888888-8888-8888-8888-888888888888"),
                    UserId = Guid.Parse("D2E3F4A5-6789-4BCD-8EFA-2345678901BC"),
                    Title = "John’s Photography",
                    Bio = "Capturing the sunset.",
                    LikesQnt = 22,
                    SharesQnt = 7,
                    CreatedAt = new DateTime(2026, 03, 01)
                }
            };
        }
        public static Comment[] Comments()
        {
            return new[] {
                new Comment
                {
                    Id = Guid.Parse("E2222222-2222-2222-2222-222222222222"),
                    UserId = Guid.Parse("D2E3F4A5-6789-4BCD-8EFA-2345678901BC"), // John
                    PostId = Guid.Parse("A1111111-1111-1111-1111-111111111111"), // Welcome Post
                    Bio = "Great introduction, looking forward to more posts!",
                    LikesQnt = 4,
                    CreatedAt = new DateTime(2024, 02, 01),
                    IsEdited = false
                },
                new Comment
                {
                    Id = Guid.Parse("E3333333-3333-3333-3333-333333333333"),
                    UserId = Guid.Parse("E3F4A5B6-7890-4CDE-9FAB-3456789012CD"), // Maria
                    PostId = Guid.Parse("A2222222-2222-2222-2222-222222222222"), // Daily Thoughts
                    Bio = "Nice reflections, John. Keep sharing!",
                    LikesQnt = 2,
                    CreatedAt = new DateTime(2024, 02, 02),
                    IsEdited = false
                },
                new Comment
                {
                    Id = Guid.Parse("E4444444-4444-4444-4444-444444444444"),
                    UserId = Guid.Parse("F4A5B6C7-8901-4DEF-9ABC-4567890123DE"), // Alex
                    PostId = Guid.Parse("A3333333-3333-3333-3333-333333333333"), // Maria’s First Post
                    Bio = "Welcome Maria! Glad to see your first post.",
                    LikesQnt = 5,
                    CreatedAt = new DateTime(2024, 02, 03),
                    IsEdited = false
                },
                new Comment
                {
                    Id = Guid.Parse("E5555555-5555-5555-5555-555555555555"),
                    UserId = Guid.Parse("01234567-89AB-4CDE-9F01-234567890ABC"), // Sofia
                    PostId = Guid.Parse("A4444444-4444-4444-4444-444444444444"), // Alex’s Update
                    Bio = "Thanks for the update, Alex!",
                    LikesQnt = 3,
                    CreatedAt = new DateTime(2024, 02, 04),
                    IsEdited = false
                },
                new Comment
                {
                    Id = Guid.Parse("E6666666-6666-6666-6666-666666666666"),
                    UserId = Guid.Parse("12345678-9ABC-4DEF-9012-345678901BCD"), // Mike
                    PostId = Guid.Parse("A5555555-5555-5555-5555-555555555555"), // Sofia’s Post
                    Bio = "Nice post, Sofia! Looking forward to more.",
                    LikesQnt = 6,
                    CreatedAt = new DateTime(2024, 02, 05),
                    IsEdited = false
                },
                new Comment
                {
                    Id = Guid.Parse("E7777777-7777-7777-7777-777777777777"),
                    UserId = Guid.Parse("23456789-ABCD-4EF0-9123-45678901CDEF"), // Olga
                    PostId = Guid.Parse("A6666666-6666-6666-6666-666666666666"), // Mike’s Thoughts
                    Bio = "Interesting ideas, Mike!",
                    LikesQnt = 1,
                    CreatedAt = new DateTime(2024, 02, 06),
                    IsEdited = false
                },
                new Comment
                {
                    Id = Guid.Parse("E8888888-8888-8888-8888-888888888888"),
                    UserId = Guid.Parse("3456789A-BCDE-4F01-9234-5678901DEF01"), // Ivan
                    PostId = Guid.Parse("A7777777-7777-7777-7777-777777777777"), // Olga’s Story
                    Bio = "Nice story, Olga!",
                    LikesQnt = 7,
                    CreatedAt = new DateTime(2024, 02, 07),
                    IsEdited = false
                }
                };
        }
        public static Race[] Races()
        {
            return
            [
                new Race
                {
                    Id = Guid.Parse("10000000-0000-0000-0000-000000000001"),
                    Name = "Human",
                    ThemeColorHex = "#C0A080" // нейтральний тілесний
                },
                new Race
                {
                    Id = Guid.Parse("10000000-0000-0000-0000-000000000002"),
                    Name = "Elf",
                    ThemeColorHex = "#7FFFD4" // легкий магічний бірюзовий
                },
                new Race
                {
                    Id = Guid.Parse("10000000-0000-0000-0000-000000000003"),
                    Name = "Dark Elf",
                    ThemeColorHex = "#4B0082" // темно-фіолетовий
                },
                new Race
                {
                    Id = Guid.Parse("10000000-0000-0000-0000-000000000004"),
                    Name = "Dwarf",
                    ThemeColorHex = "#8B4513" // коричневий (земля/камінь)
                },
                new Race
                {
                    Id = Guid.Parse("10000000-0000-0000-0000-000000000005"),
                    Name = "Orc",
                    ThemeColorHex = "#556B2F" // темно-зелений
                },
                new Race
                {
                    Id = Guid.Parse("10000000-0000-0000-0000-000000000006"),
                    Name = "Troll",
                    ThemeColorHex = "#2E8B57" // болотний зелений
                },
                new Race
                {
                    Id = Guid.Parse("10000000-0000-0000-0000-000000000007"),
                    Name = "Halfling",
                    ThemeColorHex = "#DEB887" // теплий світлий
                },
                new Race
                {
                    Id = Guid.Parse("10000000-0000-0000-0000-000000000008"),
                    Name = "Dragonborn",
                    ThemeColorHex = "#B22222" // червоний (дракон)
                }
            ];
        }
    }
}
