using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMediaBackend.Data.Entities;
using System.Reflection.Emit;

namespace SocialMediaBackend.Data.Configuration
{
    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> modelBuilder)
        {
            modelBuilder
                .HasMany(p => p.Comments)
                .WithOne(c => c.Post)
                .HasForeignKey(c => c.PostId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder
                .Property(p => p.LikesQnt)
                .HasDefaultValue(0);

            modelBuilder
                .Property(p => p.SharesQnt)
                .HasDefaultValue(0);

            modelBuilder
                .Property(p => p.DeletedAt)
                .IsRequired(false);
        }
    }
}
