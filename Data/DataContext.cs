using ASP_PV411.Services.Kdf;
using Microsoft.EntityFrameworkCore;
using SocialMediaBackend.Data.Entities;

namespace SocialMediaBackend.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Entities.User> Users { get; set; }
        public DbSet<Entities.UserRole> UserRoles { get; set; }
        public DbSet<Entities.Comment> Comments { get; set; }
        public DbSet<Entities.Post> Posts { get; set; }
        public DbSet<Entities.Race> Races { get; set; }
        public DbSet<Entities.Interest> Interests { get; set; }
        public DbSet<Entities.UserInterest> UsersInterests { get; set; }

        public DataContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);

            //modelBuilder.Entity<User>().HasData(SeedData.Users(_kdfService));
            //modelBuilder.Entity<UserRole>().HasData(SeedData.UserRoles());
            //modelBuilder.Entity<Post>().HasData(SeedData.Posts());
            //modelBuilder.Entity<Comment>().HasData(SeedData.Comments());
            //modelBuilder.Entity<Race>().HasData(SeedData.Races());
            //modelBuilder.Entity<Interest>().HasData(SeedData.Interests());
            //modelBuilder.Entity<UserInterest>().HasData(SeedData.UsersInterests());
        }
    }
}
