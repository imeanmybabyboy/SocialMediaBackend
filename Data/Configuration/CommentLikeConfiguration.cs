using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMediaBackend.Data.Entities;

namespace SocialMediaBackend.Data.Configuration
{
    public class CommentLikeConfiguration : IEntityTypeConfiguration<CommentLike>
    {
        public void Configure(EntityTypeBuilder<CommentLike> builder)
        {
            builder.HasKey(l => new { l.UserId, l.CommentId });

            builder.HasOne(l => l.User)
                  .WithMany(u => u.CommentLikes)
                  .HasForeignKey(l => l.UserId);

            builder.HasOne(l => l.Comment)
                  .WithMany(c => c.Likes)
                  .HasForeignKey(l => l.CommentId);
        }
    }
}
