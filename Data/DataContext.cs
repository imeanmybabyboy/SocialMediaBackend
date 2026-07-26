using ASP_PV411.Services.Kdf;
using Microsoft.EntityFrameworkCore;

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
        public DbSet<Entities.Interest> Interests { get; set; }
        public DbSet<Entities.UserInterest> UsersInterests { get; set; }
        public DbSet<Entities.PostInterest> PostsInterests { get; set; }
        public DbSet<Entities.PostLike> PostLikes { get; set; }
        public DbSet<Entities.CommentLike> CommentLikes { get; set; }
        public DbSet<Entities.PostSave> PostSaves { get; set; }
        public DbSet<Entities.PostShare> PostShares { get; set; }
        public DbSet<Entities.UserFollow> UserFollows { get; set; }

        public DataContext(DbContextOptions options, IKdfService kdfService) : base(options)
        {
            _kdfService = kdfService;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);
        }
    }
}
