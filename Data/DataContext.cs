using ASP_PV411.Services.Kdf;
using Microsoft.EntityFrameworkCore;
using SocialMediaBackend.Data.Entities;

namespace SocialMediaBackend.Data
{
    public class DataContext : DbContext
    {
        private readonly IKdfService _kdfService;

        public DbSet<Entities.User> Users { get; set; }
        public DbSet<Entities.UserRole> UserRoles { get; set; }
        public DbSet<Entities.Comment> Comments { get; set; }
        public DbSet<Entities.Post> Posts { get; set; }
        public DbSet<Entities.Race> Races { get; set; }

        public DataContext(DbContextOptions options, IKdfService kdfService) : base(options)
        {
            _kdfService = kdfService;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);

            modelBuilder.Entity<User>().HasData(SeedData.Users(_kdfService));
            modelBuilder.Entity<UserRole>().HasData(SeedData.UserRoles());
            modelBuilder.Entity<Post>().HasData(SeedData.Posts());
            modelBuilder.Entity<Comment>().HasData(SeedData.Comments());
            modelBuilder.Entity<Race>().HasData(SeedData.Races());
        }
    }
}
